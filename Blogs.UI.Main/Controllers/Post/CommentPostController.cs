using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blogs.UI.Main.Models;
using FYJ;
using Blogs.UI.Main.ViewModel;
using Blogs.Entity;

namespace Blogs.UI.Main.Controllers.Post
{
    [CustomExceptionFilter]
    public class CommentPostController : BaseController
    {

        //是否所有者
        private bool isOwner(string articleUserID)
        {
            if (Utility.UserID != null)
            {
                if (Utility.UserID == articleUserID)
                {
                    return true;
                }
            }
            return false;
        }


        #region 回复
        [ValidateAntiForgeryToken]
        public ActionResult Comment()
        {
            string commentContent = Request["commentContent"];
            string articleID = Request["articleID"];
            if (string.IsNullOrEmpty(commentContent))
            {
                return Json(new { code = -1, message = "请输入回复内容！" }, JsonRequestBehavior.AllowGet);
            }

            Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetEntity(base.BlogID);
            if(blog.IsCloseComment)
            {
                return Json(new { code = -999, message = "系统已经关闭评论！" }, JsonRequestBehavior.AllowGet);
            }

            if (!Utility.CommentBll.IsAllowCommentContent(commentContent))
            {
                return Json(new { code = -1, message = "输入的内容有危险性！" }, JsonRequestBehavior.AllowGet);
            }


            DateTime? lastDate = null;
            if (Session["lastDate"] != null)
            {
                lastDate = Convert.ToDateTime(Session["lastDate"]);
            }

            int articleCommentLimit = Utility.ArticleBll.GetArticleCommentLimit(articleID);
            string articleUserID = Utility.ArticleBll.GetarticleUserID(articleID);

            if (Utility.CommentBll.isDisableComment(articleCommentLimit) && !isOwner(articleUserID))
            {
                return Json(new { code = -4, message = "该文章禁止回复！" }, JsonRequestBehavior.AllowGet);
            }

            if (Utility.CommentBll.isDisabledAnonymousComment(articleCommentLimit) && !isOwner(articleUserID))
            {
                return Json(new { code = -5, message = "该文章不允许匿名回复,请登录后回复！" }, JsonRequestBehavior.AllowGet);
            }
            if (isOwner(articleUserID) || lastDate == null || (DateTime.Now.Subtract(lastDate.Value).TotalSeconds > 60))
            {
                Blogs.Entity.blog_tb_comment model = new Entity.blog_tb_comment();
                model.commentContent = commentContent;  //html过滤

                //非自己回复需要审核
                if (!isOwner(articleUserID))
                {
                    model.commentState = 0;
                }

                int floor = Utility.CommentBll.GetMaxFloor(articleID);

                model.commentIP = Utility.GetClientIP(HttpContext);
               

                model.floor = floor + 1;
                model.commentText = Request["commentText"];
                model.articleID = Convert.ToInt32(articleID);
                model.userID = Utility.UserID;
                model.userName = Utility.UserName;
                Utility.CommentBll.Insert(model);
                Session["lastDate"] = DateTime.Now;
                string message = "发布成功";
                if (Utility.CommentBll.isVerifyComment(articleCommentLimit))
                {
                    message = "发布成功,需要审核后才会显示";
                }
                return Json(new { code = 1, message = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -3, message = (60 - (int)(DateTime.Now.Subtract(lastDate.Value).TotalSeconds) + "秒内不能重复回复！") }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 支持和返回  state为1表示支持 0表示反对
        public ActionResult Zhichi()
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                return Json(new { code = -1, message = "缺少id参数" }, JsonRequestBehavior.AllowGet);
            }

            string ip = Utility.GetClientIP(HttpContext);

            if (Request["state"] == "0")
            {
                Utility.CommentBll.Against(Request["id"], Utility.UserID, ip);
            }
            else
            {
                Utility.CommentBll.Support(Request["id"], Utility.UserID, ip);
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 快速回复
        public ActionResult FastComment()
        {
            string articleID = Request["articleID"];
            string commentContent = Request["commentContent"];

            if (string.IsNullOrEmpty(commentContent))
            {
                return Json(new { code = -2, message = "请输入回复内容！" }, JsonRequestBehavior.AllowGet);
            }

            DateTime? lastDate = null;
            if (Session["lastDate"] != null)
            {
                lastDate = Convert.ToDateTime(Session["lastDate"]);
            }

            string articleUserID = Utility.ArticleBll.GetarticleUserID(articleID);

            if (isOwner(articleUserID) || lastDate == null || (DateTime.Now.Subtract(lastDate.Value).TotalSeconds > 60))
            {
                Blogs.Entity.blog_tb_comment model = new Entity.blog_tb_comment();
                int floor = Utility.CommentBll.GetMaxFloor(articleID);
                model.floor = floor + 1;

                model.commentIP = Utility.GetClientIP(HttpContext);
               

                model.commentContent = "<img src=\"" + commentContent + "\" />";  //html过滤
                //快速回复不需要审核
                model.commentState = 3;
                model.userID = Utility.UserID;
                model.userName = Utility.UserName;
                model.articleID = Convert.ToInt32(articleID);
                Session["lastDate"] = DateTime.Now;
                Utility.CommentBll.Insert(model);

                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -3, message = (60 - (int)(DateTime.Now.Subtract(lastDate.Value).TotalSeconds) + "秒内不能重复回复！") }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 删除回复
        public ActionResult DeleteComment()
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                return Json(new { code = -1, message = "缺少id参数" }, JsonRequestBehavior.AllowGet);
            }
            string articleID = Request["articleID"];
            if (string.IsNullOrEmpty(articleID))
            {
                return Json(new { code = -2, message = "缺少articleID参数！" }, JsonRequestBehavior.AllowGet);
            }
            string articleUserID = Utility.ArticleBll.GetarticleUserID(articleID);
            if (isOwner(articleUserID) == false)
            {
                return Json(new { code = -3, message = "没有登录或无权限操作！" }, JsonRequestBehavior.AllowGet);
            }

            Utility.CommentBll.Delete(Request["id"]);

            return Json(new { code = 1, message = "操作成功！" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 审核
        public ActionResult Verify()
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                return Json(new { code = -1, message = "缺少id参数" }, JsonRequestBehavior.AllowGet);
            }
            string articleID = Request["articleID"];
            if (string.IsNullOrEmpty(articleID))
            {
                return Json(new { code = -2, message = "缺少articleID参数！" }, JsonRequestBehavior.AllowGet);
            }
            string articleUserID = Utility.ArticleBll.GetarticleUserID(articleID);
            if (isOwner(articleUserID) == false)
            {
                return Json(new { code = -3, message = "没有登录或无权限操作！" }, JsonRequestBehavior.AllowGet);
            }

            string commentID = Request["id"];
            Blogs.Entity.blog_tb_comment model = Utility.CommentBll.GetEntity(commentID);
            model.commentState = Convert.ToInt32(Request["state"]);
            Utility.CommentBll.Update(model);

            return Json(new { code = 1, message = "操作成功！" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 状态投票
        public ActionResult Vote()
        {
            if (String.IsNullOrEmpty(Request["id"]))
            {
                return Json(new { code = -1, message = "缺少id参数" }, JsonRequestBehavior.AllowGet);
            }
            string articleID = Request["articleID"];

            Utility.CommentBll.Vote(articleID, Request["id"], Utility.UserID, Utility.GetClientIP(HttpContext));

            return Json(new { code = 1, message = "操作成功！" }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
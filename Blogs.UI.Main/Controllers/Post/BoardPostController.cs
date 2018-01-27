using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Blogs.UI.Main.Models;
using FYJ.Common;

namespace Blogs.UI.Main.Controllers.Post
{
    [CustomExceptionFilter]
    public class BoardPostController : BaseController
    {
      
        #region 回复
        public ActionResult Comment()
        {
            string commentContent = Request["commentContent"];
            string commentText = Request["commentText"];

            if (string.IsNullOrEmpty(commentContent))
            {
                return Json(new { code = -1, message = "请输入留言内容！" }, JsonRequestBehavior.AllowGet);
            }

            Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetEntity(base.BlogID);
            if (blog.IsCloseBoard)
            {
                return Json(new { code = -999, message = "系统已经关闭留言！" }, JsonRequestBehavior.AllowGet);
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

            if (lastDate == null || (DateTime.Now.Subtract(lastDate.Value).TotalSeconds > 60))
            {
                Blogs.Entity.blog_tb_Board model = new Entity.blog_tb_Board();
                model.ID = Guid.NewGuid().ToString("N");
                model.Mark = commentContent;  //html过滤
                model.IP = Utility.GetClientIP(HttpContext);
              
                model.UserID = Utility.UserID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;

                Utility.BoardBll.Insert(model);

                Session["lastDate"] = DateTime.Now;

                return Json(new { code = 1, message = "发布成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -3, message = (60 - (int)(DateTime.Now.Subtract(lastDate.Value).TotalSeconds) + "秒内不能重复留言！") }, JsonRequestBehavior.AllowGet);
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

            if (Request["state"] == "0")
            {
                Blogs.Entity.blog_tb_Board model = Utility.BoardBll.GetEntity(Request["id"]);
                model.AgainstCount = model.AgainstCount+1;
                Utility.BoardBll.Update(model);
            }
            else
            {
                Blogs.Entity.blog_tb_Board model = Utility.BoardBll.GetEntity(Request["id"]);
                model.SupportCount = model.SupportCount + 1;
                Utility.BoardBll.Update(model);
            }

            return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 快速回复
        public ActionResult FastComment()
        {
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

            if (lastDate == null || (DateTime.Now.Subtract(lastDate.Value).TotalSeconds > 60))
            {
                Blogs.Entity.blog_tb_Board model = new Entity.blog_tb_Board();
                model.ID = Guid.NewGuid().ToString("N");
                model.IP = Utility.GetClientIP();
                
                model.Mark = "<img src=\"" + commentContent + "\" />";  //html过滤
                //快速回复不需要审核
                model.State = 1;
                model.UserID = Utility.UserID;
                model.ADD_DATE = DateTime.Now;
                model.UPDATE_DATE = DateTime.Now;

                Session["lastDate"] = DateTime.Now;

                Utility.BoardBll.Insert(model);

                return Json(new { code = 1, message = "操作成功" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = -3, message = (60 - (int)(DateTime.Now.Subtract(lastDate.Value).TotalSeconds) + "秒内不能重复留言！") }, JsonRequestBehavior.AllowGet);
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

            if (Utility.UserID != Info.userID)
            {
                return Json(new { code = -2, message = "无权操作" }, JsonRequestBehavior.AllowGet);
            }

            Utility.BoardBll.Delete(Request["id"]);

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

            if (Utility.UserID != Info.userID)
            {
                return Json(new { code = -2, message = "无权操作" }, JsonRequestBehavior.AllowGet);
            }

            Blogs.Entity.blog_tb_Board model = Utility.BoardBll.GetEntity(Request["id"]);
            model.State = Convert.ToInt32(Request["state"]);
            Utility.BoardBll.Update(model);

            return Json(new { code = 1, message = "操作成功！" }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}

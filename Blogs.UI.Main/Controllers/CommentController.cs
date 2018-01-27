using Blogs.Entity;
using Blogs.UI.Main.Models;
using Blogs.UI.Main.ViewModel;
using FYJ.Common;
using FYJ.Web.NetPager;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class CommentController : BaseController
    {
        [CountFilter]
        public ActionResult Index()
        {
            string articleID = RouteData.Values["id"].ToString();
            //当前页
            int page = Convert.ToInt32(RouteData.Values["page"]);
            if (page == 0)
            {
                page = 1;
            }

            int pageSize = 10;
            CommentModel model = Utility.CommentBll.GetCommentPage(articleID, page, pageSize);

            Pager pager = new Pager(Request.Url)
            {
                RecordCount = model.RecordCount,
                PageSize = pageSize,
                MaxShowPageSize = 10,
                PageIndex = page,
                ShowSpanText = true,
                EnableUrlRewriting = true,
                UrlRewritePattern = "comment-" + articleID + "-{0}.html"
            };

            CommentViewModel viewmodel = new CommentViewModel();
            viewmodel.SiteID = BlogID;
            viewmodel.Collection = FixCommentModel(model.CommentCollection, articleID);
            viewmodel.CommentPagerHtml = pager.ToString();
            viewmodel.ArticleID = Convert.ToInt32(articleID);
            viewmodel.Title = "评论-" + Utility.ArticleBll.GetEntity(articleID).articleTitle;
            viewmodel.CommentStateCollection = model.CommentStateCollection;
            viewmodel.CommentTagCollection = FixCommentTag(model.CommentTagCollection);

            return View("~/Views/Blog/Common/Comment.cshtml", viewmodel);
        }

        //给标签打上随机颜色
        private IEnumerable<CommentTag> FixCommentTag(IEnumerable<CommentTag> list)
        {
            String[] colors = new string[] { "#910048", "#B7005B", "#D20069", "#F7007B", "#FF158A", "#FF3C9D", "#FF5BAD", "#FF80BF", "#FF9DCE", "#FFC1E0" };

            List<String> tagList = new List<string>();
            int index = 0;
            for (int i = 0; i < list.Count() && index < 10; i++)
            {
                if (!tagList.Contains(list.ElementAt(i).commentText))
                {
                    list.ElementAt(i).backgroundColor = colors[index];
                    tagList.Add(list.ElementAt(i).commentText); //为了去掉重复
                    index++;
                }
            }

            return list;
        }

        private IEnumerable<blog_tb_comment> FixCommentModel(IEnumerable<blog_tb_comment> list, string articleID)
        {
            int articleCommentLimit = Utility.ArticleBll.GetArticleCommentLimit(articleID);
            string articleUserID = Utility.ArticleBll.GetarticleUserID(articleID);
            //如果评论被禁止并且当前用户不是文章所对应的用用户id  则不显示评论内容
            if (Utility.CommentBll.isDisableComment(articleCommentLimit) && !isOwner(articleUserID))
            {
                return new List<blog_tb_comment>();
            }

            return FYJ.IocFactory<IBlogFix>.Instance.GetComments(articleCommentLimit,list);
            
        }

        //是否所有者
        private bool isOwner(string articleUserID)
        {
            if (Session["Token"] != null)
            {
                string userID = base.CurrentUser.userID;
                if (userID == articleUserID)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
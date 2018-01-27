using Blogs.Entity;
using Blogs.UI.Main.Models;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Blogs.UI.Main.ViewModel;
using FYJ;
using FYJ.Web.NetPager;

namespace Blogs.UI.Main.Controllers
{
    public class ArticleController : SharedController
    {

        private string articleID
        {
            get { return this.RouteData.Values["id"].ToString(); }
        }

        [MemCacheFilter]
        [CountFilter]

        public ActionResult Index()
        {

            Entity.blog_tb_article article = Utility.ArticleBll.GetEntity(this.articleID);
            if (article == null)
            {
                return Content("文章不存在");
            }

            if (article.articleIsDelete)
            {
                return Content("文章被删除到回收站");
            }

            bool isDisabled = Utility.ArticleBll.IsArticleDisabled(this.articleID);
            if (isDisabled)
            {
                return Content("文章、分类或博客被暂时关闭或禁用");
            }


            if ((!String.IsNullOrEmpty(article.articlePassword)) && (Session["Autharticle"] == null || !Session["Autharticle"].ToString().Contains(this.articleID)))
            {
                return new RedirectResult("/ArticlePassword?articleID=" + articleID + "&BackUrl=" + Server.UrlEncode("http://" + base.Domain + "/artic-" + articleID + ".html"));
            }



            //更新最后打开时间
            Utility.ArticleBll.UpdateExtend(articleID, Utility.GetClientIP(this.HttpContext), Utility.UserID);

            string theme = Utility.ArticleBll.GetArticleThemeID(this.articleID);
            if (!String.IsNullOrEmpty(Request.QueryString["ver"]))
            {
                theme = Request.QueryString["ver"];
            }

            ShowViewModel viewmodel = GetModel(article);
            if (theme.Equals("SimpleShow", StringComparison.CurrentCultureIgnoreCase))
            {
                return View("~/Views/Blog/Common/SimpleShow.cshtml", viewmodel);
            }

            return View("~/Views/Blog/Themes/" + theme + "/Show.cshtml", viewmodel);
        }



        protected ShowViewModel GetModel(blog_tb_article article)
        {
            ShowViewModel viewmodel = new ShowViewModel();
            base.SetModel(viewmodel);
            viewmodel.IsCloseComment = Info.IsCloseComment;
            viewmodel.ThemeName = article.themeID;
            viewmodel.Title = article.articleTitle + "-" + viewmodel.Title;
            viewmodel.Keywords = article.articleKeywords;
            viewmodel.Description = article.articleDescription;
            viewmodel.ObjectID = articleID;


            UserModel user = base.CurrentUser;
            ArticleModel model = Utility.ArticleBll.GetArticleByID(articleID, Utility.GetClientIP(HttpContext), user == null ? null : user.userID);

            viewmodel.CurrentArticle = model.CurrentArticle;


            if (viewmodel.CurrentArticle.articleIsOriginal)
            {
                viewmodel.CurrentArticle.articleSource = "本站原创";
            }
            else
            {
                if (viewmodel.CurrentArticle.IsShowSource)  //如果显示来源
                {
                    if (!String.IsNullOrEmpty(viewmodel.CurrentArticle.articleSourceUrl))
                    {
                        //return "本文转载自:<a href=\"" + _articleSourceUrl + "\" target=\"_blank\">" + _articleSourceUrl + "</a>";
                        viewmodel.CurrentArticle.articleSourceUrl = "本文转载自:" + viewmodel.CurrentArticle.articleSourceUrl;
                    }
                }
                else
                {
                    viewmodel.CurrentArticle.articleSource = "";
                    viewmodel.CurrentArticle.articleSourceUrl = "";
                }
            }

            blog_tb_article_content content = Utility.ArticleBll.GetArticleContent(articleID);

            if (!ObjectHelper.IsNullOrEmptyOruUndefinedOrWhiteSpace(content.articleHideContent))
            {
                if (this.isReplyed())
                {
                    viewmodel.CurrentArticle.articleHideContent = content.articleHideContent;
                }
                else
                {
                    viewmodel.CurrentArticle.articleHideContent = "本文有隐藏内容，需要登录回复后刷新方能查看";
                }
            }

            if (model.CurrentArticle.articleIsOriginal)
            {
                viewmodel.Shenming = "欢迎转载,但请以超链接的形式给出原文链接:<a href=\"" + Request.Url.ToString() + "\">" + Request.Url.ToString() + "</a>";
            }

            viewmodel.CommentStateCollection = model.CommentStateCollection;
            viewmodel.CommentTagCollection = model.CommentTagCollection;
            viewmodel.CommentPagerHtml = GetCommentPager(1, model.CommentCount);

            viewmodel.BeforeArticle = model.BeforeArticle;
            viewmodel.AfterArticle = model.AfterArticle;
            viewmodel.CommentCollection = FYJ.IocFactory<IBlogFix>.Instance.GetComments(model.CurrentArticle.articleAttachmentLimit, model.CommentCollection);

            string articleContent = content.articleContent;
            #region 获取附件
            List<blog_attachment> atts = Utility.ArticleBll.GetFileRelation(articleID, "attachment");
            FixAttachment(viewmodel, atts);
            viewmodel.AttachmentCollection = atts;
            #endregion

            if (viewmodel.CurrentArticle.articleIsOriginal)
            {
                viewmodel.Shenming = "本文为原创,欢迎转载,请注明出处和原文链接:" + viewmodel.CurrentArticle.Url;
            }

            viewmodel.CurrentArticle.articleContent = articleContent;
            //如果是远程 则替换图片地址为阿里云
            if (Utility.IsRemote)
            {
                viewmodel.CurrentArticle.articleContent = Utility.ReplaceImgOrFileSrc(viewmodel.CurrentArticle.articleContent);
            }

            viewmodel.CurrentArticle.articleContent = Utility.ReplaceImgLazyload(viewmodel.CurrentArticle.articleContent);

            return viewmodel;
        }

        #region 处理附件
        private void FixAttachment(ShowViewModel model, List<blog_attachment> atts)
        {
            if (atts.Count > 0)
            {
                //attDataTable.Columns.Add("icon");
                //foreach (DataRow dr in attDataTable.Rows)
                //{
                //    string reg = "<p\\s+?style=\"line-height:\\s*?16px;\">.*?src=\"(.*?)\".*?"+dr["fileUrl"]+".*?</p>";
                //    Match m = Regex.Match(articleContent,reg);
                //    if(m.Success)
                //    {
                //        dr["icon"] = m.Groups[1].Value;
                //        articleContent = articleContent.Replace(m.Value, "");
                //    }
                //}
                Blogs.Entity.AttachmentLimit attachementlimit = (Blogs.Entity.AttachmentLimit)Enum.Parse(typeof(Blogs.Entity.AttachmentLimit), model.CurrentArticle.articleAttachmentLimit + "");
                bool b1 = (attachementlimit & Blogs.Entity.AttachmentLimit.禁止未登录用户下载) == Blogs.Entity.AttachmentLimit.禁止未登录用户下载;
                bool b2 = (attachementlimit & Blogs.Entity.AttachmentLimit.禁止未回复用户下载) == Blogs.Entity.AttachmentLimit.禁止未回复用户下载;
                bool b3 = (attachementlimit & Blogs.Entity.AttachmentLimit.禁止下载) == Blogs.Entity.AttachmentLimit.禁止下载;
                //0表示未设置附件权限
                if (model.CurrentArticle.articleAttachmentLimit > 0)
                {
                    if (b1)
                    {
                        if (CurrentUser == null)
                        {
                            blog_attachment at = new blog_attachment { fileUrl = "", fileName = "请登录后查看附件" };
                            atts = new List<blog_attachment> { at };
                        }
                    }

                    if (b2)
                    {
                        if (CurrentUser == null)
                        {
                            blog_attachment at = new blog_attachment { fileUrl = "", fileName = "请回复后查看附件" };
                            atts = new List<blog_attachment> { at };
                        }
                        else
                        {
                            //if(Utility.CommentBll.IsReplyed(articleID,CurrentUser.userID))
                            //{
                            //    model.AttachmentCollection = attDataTable.DataTableToModel<AttachmentModel>();
                            //}
                            //else
                            //{
                            //    AttachmentModel at = new AttachmentModel { fileUrl = "", fileName = "请回复后查看附件" };
                            //    model.AttachmentCollection = new List<AttachmentModel> { at };
                            //}
                        }
                    }
                }
            }
        }
        #endregion

        private void RenderAttachment()
        {

        }

        //显示评论分页  分页大小10
        private string GetCommentPager(int page, int total)
        {
            Pager pager = new Pager(Request.Url)
            {
                RecordCount = total,
                PageSize = 10,
                MaxShowPageSize = 10,
                PageIndex = page,
                ShowSpanText = true,
                EnableUrlRewriting = true,
                UrlRewritePattern = "javascript:commentPage({0})",
                FirstPageText = "<<",
                LastPageText = ">>",
                PrePageText = "<",
                NextPageText = ">",
                ClassName = "pagination",
                CurrentPageButtonCss = "current"
            };
            string str = pager.ToString();

            return str;
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


        //是否所有者
        private bool isOwner(string articleUserID)
        {
            UserModel user = base.CurrentUser;
            if (user != null)
            {
                String userID = user.userID;
                if (userID == articleUserID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否已经回复   如果是本文章作者 直接视为已回复
        /// </summary>
        /// <param name="articleUserID"></param>
        /// <returns></returns>
        private bool isReplyed()
        {
            UserModel user = base.CurrentUser;
            if (user != null)
            {
                string articleUserID = GetarticleUserID();
                if (isOwner(articleUserID))
                {
                    return true;
                }

                return Utility.CommentBll.IsReplyed(this.articleID, user.userID);
            }
            return false;
        }

        /// <summary>
        /// 获取文章作者用户ID
        /// </summary>
        /// <returns></returns>
        private string GetarticleUserID()
        {
            return Utility.ArticleBll.GetarticleUserID(this.articleID);
        }

    }
}
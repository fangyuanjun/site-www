using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main
{
    public class BlogFix : IBlogFix
    {
        //private string _basePath;  不能用此缓存，因为可能CurrentBlog变化了而_basePath的域名还没变
        /// <summary>
        /// 获取根URL  结尾不带 /
        /// </summary>
        private string BasePath
        {
            get
            {
                blog_tb_blog info = FYJ.IocFactory<IBlogInfo>.Instance.CurrentBlog;
                string domain = info.blogDomain;
                if (!String.IsNullOrEmpty(domain))
                {
                    return "http://" + domain;
                }

                return "";
            }
        }

        public string GetArticleUrl(string articleID)
        {
            return this.BasePath + "/artic-" + articleID + ".html";
        }

        public string GetCategoryUrl(string categoryID, string categoryDomain)
        {
            if (!String.IsNullOrEmpty(categoryDomain))
            {
                return "http://" + categoryDomain;
            }

            return BasePath + "/cate-" + categoryID + "-1.html";
        }

        public string GetMenuUrl(string menuUrl)
        {
            if (!String.IsNullOrEmpty(menuUrl))
            {
                if (!menuUrl.StartsWith("http"))
                {
                    return this.BasePath + "/" + menuUrl.TrimStart('/');
                }
            }

            return menuUrl;
        }

        public string GetMonthUrl(string yyyy, string mm)
        {
            return BasePath + "/month-" + yyyy + mm + "-1.html";
        }

        public string GetTagUrl(string tagID)
        {
            return BasePath + "/tag-" + tagID + "-1.html";
        }


        public IEnumerable<blog_tb_comment> GetComments(int articleCommentLimit, IEnumerable<blog_tb_comment> comments)
        {
            foreach (blog_tb_comment item in comments)
            {

                string commentContent = item.commentContent;

                if (item.commentState == 0)
                {
                    if (Utility.CommentBll.isVerifyComment(articleCommentLimit))  ////如果需要审核
                    {
                        item.commentContent = "<font color='red'>评论正在正在审核中...</font>";
                    }
                }
                else if (item.commentState == 2)
                {
                    item.commentContent = "<font color='red'>评论被拒绝</font>";
                }

                //如果登录用户是该评论的拥有者则显示评论内容
                //if (Session["Token"] != null)
                //{
                //    string userID = base.CurrentUser.userID;
                //    if (userID == item.AuthorUserID)
                //    {
                //        item.commentContent = commentContent;
                //    }
                //}

                if (item.floor > 0)
                {
                    item.Author = "[" + item.floor + "楼]";
                }

                string mark = "";

                if (item.userName != "")
                {
                    mark += "&nbsp;" + item.userName;
                }

                if (!String.IsNullOrEmpty(item.commentIP))
                {
                    mark += "&nbsp;" + item.commentIP.Substring(0, item.commentIP.LastIndexOf(".") + 1) + "...";
                }
                if (!String.IsNullOrEmpty(item.commentCountry))
                {
                    if (item.commentCountry != "中国")
                    {
                        mark += "&nbsp;来自" + item.commentCountry + "的网友";
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.commentCity))
                        {
                            mark += "&nbsp;来自" + item.commentCity + "的网友";
                        }
                    }
                }

                item.Mark = mark;
            }

            return comments;
        }
    }
}
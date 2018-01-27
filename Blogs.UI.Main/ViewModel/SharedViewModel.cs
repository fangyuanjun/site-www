using Blogs.Entity;
using Blogs.UI.Main.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    /// <summary>
    /// 母版页的模型
    /// </summary>
    public class SharedViewModel :BaseViewModel
    {

        #region 固定链接
        public string RegisterUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Register";
            }
        }

        public string LoginUrl
        {
            get
            {
                String port = (HttpContext.Current.Request.Url.Port == 80) ? "" : ":" + HttpContext.Current.Request.Url.Port;
                return ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login?BackURL=" + HttpContext.Current.Server.UrlEncode("http://" + HttpContext.Current.Request.Url.Host + port + "/AuthJump?ref=" + HttpContext.Current.Request.Url.ToString());
            }
        }

        public string QQLoginUrl
        {
            get
            {
                String port = (HttpContext.Current.Request.Url.Port == 80) ? "" : ":" + HttpContext.Current.Request.Url.Port;
                return ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login/LoginByQQ?BackURL=" + HttpContext.Current.Server.UrlEncode("http://" + HttpContext.Current.Request.Url.Host + port + "/AuthJump?ref=" + HttpContext.Current.Request.Url.ToString());
            }
        }

        public string LogoutUrl
        {
            get
            {
                String port = (HttpContext.Current.Request.Url.Port == 80) ? "" : ":" + HttpContext.Current.Request.Url.Port;
                return ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/') + "/Login/Logout?BackURL=" + HttpContext.Current.Server.UrlEncode("http://" + HttpContext.Current.Request.Url.Host + port + "/AuthJump?action=logout&ref=" + HttpContext.Current.Request.Url.ToString());
            }
        }

        public string PassportRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PassportRootUrl"];
            }
        }


        public string SearchUrl
        {
            get
            {
                return "http://so.kecq.com/";
            }
        }
        #endregion

        /// <summary>
        /// 消息数
        /// </summary>
        public int MessageCount{ get;set;}

        /// <summary>
        /// 博客链接
        /// </summary>
        public string BlogUrl { get; set; }

        /// <summary>
        /// 站点地图链接
        /// </summary>
        public string SitemapUrl { get; set; }

        /// <summary>
        /// Rss链接
        /// </summary>
        public string RssUrl
        {
            get
            {
                return this.BlogUrl + "/Rss";
            }
        }

        /// <summary>
        /// 总文章数
        /// </summary>
        public int TotalArticleCount { get; set; }

        /// <summary>
        /// 总文章点击次数
        /// </summary>
        public int TotalArticleViewCount { get; set; }

        /// <summary>
        /// 总文章评论次数
        /// </summary>
        public int TotalArticleCommentCount { get; set; }

        /// <summary>
        /// 菜单项
        /// </summary>
        public IEnumerable<blog_tb_menu> Menus{ get;set;}

        /// <summary>
        /// 友情链接
        /// </summary>
        public IEnumerable<blog_tb_link> Links { get; set; }
    }
}
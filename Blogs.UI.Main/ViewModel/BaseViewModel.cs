using Blogs.Entity;
using System.Configuration;

namespace Blogs.UI.Main.ViewModel
{
    public class BaseViewModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 站点ID
        /// </summary>
        public string SiteID { get; set; }

        /// <summary>
        /// 对象ID
        /// </summary>
        public string ObjectID { get; set; }


        /// <summary>
        /// 统计代码
        /// </summary>
        public string TongjiHtml
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VisitMode"] == "js")
                {
                    //如果是远程IP地址或者本地访问也统计则返回统计代码  否则返回空字符串
                    if (Utility.IsRemote || ConfigurationManager.AppSettings["isTongjiLocal"] == "true")
                    {
                        return BlogTongjiHtml + Utility.SiteVistCode;
                    }
                }

                return "";
            }
        }

        public string BlogTongjiHtml { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// 博客信息
        /// </summary>
        public blog_tb_blog Blog { get; set; }

        public string faviconIcon{get;set;}

        public string Keywords { get; set; }

        public string Description { get; set; }

    }
}
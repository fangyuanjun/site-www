using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class RssController : BaseController
    {

        public ActionResult Index()
        {
            string str = GetRss();

            ContentResult result = new ContentResult();
            result.ContentType = "text/xml";
            result.ContentEncoding = Encoding.UTF8;
            result.Content = str;

            return result;
        }

        private string GetRss()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?> <rss version=\"2.0\">");
            sb.Append("<channel>");
            sb.Append("<title>" + Info.blogTitle + "</title>");
            string link = Info.blogDomain;
            if (link == "")
            {
                string url = Request.Url.ToString().ToLower();
                link = url.Substring(0, url.IndexOf("/", 8)) + "/" +Info.blogName;
            }
            sb.Append("<link>" + link + "</link>");
            sb.Append("<link>" + Info.blogDescription  + "</link>");
            sb.Append("<copyright>@www.kecq.com 2013</copyright>");
            sb.Append("<language>zh-cn</language>");
            sb.Append("<generator>www.kecq.com</generator>");

            StringBuilder items = new StringBuilder();
            List<blog_tb_article> list = Utility.BlogBll.GetBlogRss(this.BlogID);
            foreach (var v in list)
            {
                items.Append("<item>");
                items.Append("<title><![CDATA[" + v.articleTitle + "]]></title>");
                items.Append("<link><![CDATA[" + FYJ.IocFactory<IBlogFix>.Instance.GetArticleUrl(v.articleID+"") + "]]></link>");
                items.Append("<pubDate><![CDATA[" + v.articleDatetime + "]]></pubDate>");
                items.Append("<source><![CDATA[" + v.articleSource + "]]></source>");
                items.Append("<author><![CDATA[" + v.articleAuthor + "]]></author>");
                items.Append("<description><![CDATA[" + v.articleSubContentText + "]]></description>");
                items.Append("</item>");
            }
            sb.Append(items.ToString());
            sb.Append("</channel>");
            sb.Append("</rss>");

            return sb.ToString();
        }
    }
}
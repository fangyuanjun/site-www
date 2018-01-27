using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Blogs.Entity;
using System.Xml;

namespace Blogs.UI.Main.Controllers
{
    public class SitemapController : BaseController
    {

        public ActionResult Xml()
        {
            string str = GetSitemapXml();

            ContentResult result = new ContentResult();
            result.ContentType = "text/xml";
            result.ContentEncoding = Encoding.UTF8;
            result.Content = str;

            return result;
        }
        
        public ActionResult Html()
        {
            List<sitemap> model = Utility.BlogBll.GetSitemap(this.BlogID);

            return View("~/Views/sitemap.cshtml",model);
        }

        public ActionResult Text()
        {
            string str = GetSitemapText();

            ContentResult result = new ContentResult();
            result.ContentType = "text/plain";
            result.ContentEncoding = Encoding.UTF8;
            result.Content = str;

            return result;
        }

        private string GetSitemapXml()
        {
            List<sitemap> list = Utility.BlogBll.GetSitemapWithHide(this.BlogID);
            
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

            foreach(var item in list)
            {
                sb.AppendLine("<url>");
                sb.AppendLine("<loc>" + item.loc + "</loc>");
                sb.AppendLine("<lastmod>" + item.lastmod + "</lastmod>");
                sb.AppendLine("<changefreq>"+item.changefreq+"</changefreq>");
                sb.AppendLine("<priority>"+item.priority+"</priority>");
                sb.AppendLine("</url>");
            }

            sb.AppendLine("</urlset>");

            return sb.ToString();
        }


       
        private string GetSitemapText()
        {
            StringBuilder sb = new StringBuilder();
            List<sitemap> list = Utility.BlogBll.GetSitemapWithHide(this.BlogID);

            foreach (var item in list)
            {
                sb.Append(item.Url + Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
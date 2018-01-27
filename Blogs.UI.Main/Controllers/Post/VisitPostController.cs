
using Blogs.Entity;
using Blogs.IBLL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers.Post
{
    public class VisitPostController : Controller
    {

        //
        // GET: /VisitPost/
        public ActionResult Index()
        {
            string title = Request["title"];
            string url = Request["uri"];
            if ((!String.IsNullOrEmpty(title)) && (!String.IsNullOrEmpty(url)))
            {
                string objectID = null;
                if (Regex.IsMatch(url, "artic-\\d+.html"))
                {
                    objectID = Regex.Match(url, "artic-(\\d+).html").Groups[1].Value;
                }
                string siteID = Utility.GetCurrentBlog(this).blogID + "";

                //Utility.Tongji(this.HttpContext, siteID, title, objectID);

                String country = "";
                String city = "";
                string ip = Utility.GetClientIP();
                if (!Utility.IsRemote)
                {
                    throw new NotImplementedException();
                }


                //Dictionary<string, object> dic = new Dictionary<string, object>();
                //dic.Add("siteID", Request.QueryString["siteID"]);
                //dic.Add("url", reffer);
                //dic.Add("title", title);
                //dic.Add("ip", ip);

                //if (Regex.IsMatch(reffer, "artic-\\d+.html"))
                //{
                //    dic.Add("objectID", Regex.Match(reffer, "artic-(\\d+).html").Groups[1].Value);
                //}
                //dic.Add("country", country);
                //dic.Add("city", city);
                //dic.Add("userAgent", Request.UserAgent);
                //dic.Add("sessionID", Session.SessionID);
                //FYJ.Data.IDbHelper db = FYJ.Data.DbFactory.GetIDbHelper("visit");
                //db.RunProcedure("blog_proc_visit", dic);

                blog_tb_Visit entity = new blog_tb_Visit();
                entity.visitID = Guid.NewGuid().ToString("N");
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                entity.City = city;
                entity.County = country;
                entity.objectID = objectID;
                entity.SessionID = Session.SessionID;
                entity.siteID = siteID;
                entity.userAgent = Request.UserAgent;
                entity.visitIP = ip;
                entity.visitTitle = title;
                entity.visitUrl = url;

                IocFactory<IBLLVisit>.Instance.AddVisit(entity);

                return new JavaScriptResult();
            }
            else
            {
                return Content("title or uri parameter can't be null");
            }
        }
    }
}
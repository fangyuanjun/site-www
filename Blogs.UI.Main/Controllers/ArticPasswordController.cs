using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class articlePasswordController : BaseController
    {
      
        //
        // GET: /articlePassword/
        public ActionResult Index()
        {
            return View("~/Views/Blog/Themes/" + base.ThemeName + "/ArticlePassword.cshtml");
        }


        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            string articleID = Request["articleID"];
            string articlePassword = Request["articlePassword"];
            if (articlePassword == Utility.ArticleBll.GetArticlePassword(articleID))
            {
                Session["Autharticle"] = Session["Autharticle"] + "," + articleID;

                return new RedirectResult(Server.UrlDecode(Request["BackUrl"]));
            }
            else
            {
                return Content("<script>alert('文章密码错误！');history.go(-1)</script>");
            }
        }
    }
}
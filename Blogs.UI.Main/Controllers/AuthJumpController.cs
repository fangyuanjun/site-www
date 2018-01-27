using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class AuthJumpController : Controller
    {
        //
        // GET: /AuthJump/
        public ActionResult Index()
        {
            if (Request.QueryString["action"] == "logout")
            {
                Session.RemoveAll();
                Response.Redirect(Server.UrlDecode(Request.QueryString["ref"]));
            }
            else
            {
                Passport.Sso.AuthLoad auth = new Passport.Sso.AuthLoad();
                auth.IsClearSession = true;  //重要
                auth.IsRedirect = false;
                auth.Login();

                if (Session["Token"] != null)
                {
                    if (!String.IsNullOrEmpty(Request.QueryString["ref"]))
                    {
                        Response.Redirect(Request.QueryString["ref"]);
                    }
                    else
                    {
                        return Json(new { code = 1, message = "已登录" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { code = -1, message = "未登录" }, JsonRequestBehavior.AllowGet);
                }
            }

            return new EmptyResult();
        }
    }
}
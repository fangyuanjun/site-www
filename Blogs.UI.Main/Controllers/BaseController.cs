using Blogs.Entity;
using Blogs.UI.Main.ViewModel;
using FYJ;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class BaseController : Controller
    {
        public blog_tb_blog Info
        {
            get
            {
                return Utility.GetCurrentBlog(this);
            }
        }

        public UserModel CurrentUser
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["Token"] != null)
                {
                    JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(System.Web.HttpContext.Current.Session["Token"].ToString()) as JObject;
                    string userID = jobj["userID"].ToString();
                    UserModel user = new UserModel { userID = userID };

                    return user;
                }

                return null;
            }
        }

        public string BlogID
        {
            get
            {
                return Info.blogID + "";
            }
        }

        public string Domain
        {
            get
            {
                return Info.blogDomain;
            }
        }

        public string ThemeName
        {
            get
            {
                if (!String.IsNullOrEmpty(Request.QueryString["ver"]))
                {
                    return Request.QueryString["ver"];
                }

                return String.IsNullOrEmpty(Info.themeName) ? "Default" : Info.themeName;
            }
        }

        protected void SetModel(BaseViewModel model)
        {
            model.Blog = Info;
            model.SiteID = BlogID;
            model.ThemeName = ThemeName;

            model.Keywords = Info.blogKeywords;
            model.Description = Info.blogDescription;
            model.Title = Info.blogTitle;
            if (!String.IsNullOrEmpty(Info.blogSubTitle))
            {
                model.Title = Info.blogTitle + " - " + Info.blogSubTitle;
            }

            model.BlogTongjiHtml = Info.tongji;

        }
    }
}
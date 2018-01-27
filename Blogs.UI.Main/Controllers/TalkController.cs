using Blogs.UI.Main.Models;
using Blogs.UI.Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class TalkController : SharedController
    {
        //
        // GET: /Talk/

        public ActionResult Index()
        {
            TalkViewModel viewmodel = new TalkViewModel();
            base.SetModel(viewmodel);
            viewmodel.TalkCollection = Utility.TalkBll.Query(Info.userID);
            viewmodel.Title = "碎言碎语-" + Info.blogTitle;
            foreach (var v in viewmodel.Menus)
            {
                if (v.menuUrl.ToLower().EndsWith("talk"))
                {
                    v.IsActive = true;
                }
                else
                {
                    v.IsActive = false;
                }
            }
           
            return View("~/Views/Blog/Themes/" + viewmodel.ThemeName + "/Talk.cshtml", viewmodel);
        }

    }
}

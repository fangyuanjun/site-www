using Blogs.UI.Main.ViewModel;
using System.Data;
using System.Web.Mvc;
using Blogs.Entity;
using System.Collections.Generic;
using Blogs.UI.Main.Models;

namespace Blogs.UI.Main.Controllers
{
    public class NoteController : SharedController
    {
        [MemCacheFilter]
        [CountFilter]
        public ActionResult Index()
        {
            NoteViewModel viewmodel = new NoteViewModel();
            base.SetModel(viewmodel);
            viewmodel.Title = "日志-" + viewmodel.Title;
            //获取文章列表
            viewmodel.ListArticleCollection = Utility.ArticleBll.GetArticleList(this.BlogID);

            foreach (var v in viewmodel.Menus)
            {
                if (v.menuUrl.ToLower().EndsWith("note"))
                {
                    v.IsActive = true;
                }
                else
                {
                    v.IsActive = false;
                }
            }

            string viewName = "~/Views/Blog/Themes/" + viewmodel.ThemeName + "/Note.cshtml";

            return View(viewName, viewmodel);
        }

    }
}
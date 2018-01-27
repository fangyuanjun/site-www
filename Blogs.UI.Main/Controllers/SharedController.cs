using Blogs.Entity;
using Blogs.UI.Main.Models;
using Blogs.UI.Main.ViewModel;
using FYJ;
using System;
using System.Data;
using System.Linq;

namespace Blogs.UI.Main.Controllers
{
    public class SharedController : BaseController
    {
        protected  void SetModel(SharedViewModel viewmodel) 
        {
            base.SetModel(viewmodel);
            SharedModel model = Utility.BlogBll.GetBlogSharedData(this.Info.blogID + "");

            viewmodel.SitemapUrl = Info.BaseUrl + "/sitemap.xml";
            viewmodel.BlogUrl = this.Info.BaseUrl;
            viewmodel.TotalArticleCount = model.TotalArticleCount;
            viewmodel.TotalArticleViewCount = model.TotalArticleViewCount;
            viewmodel.TotalArticleCommentCount = model.TotalArticleCommentCount;
            viewmodel.Menus = model.Menus;

            if (viewmodel.Menus.Count()>0)
            {
                viewmodel.Menus.First().IsActive = true;
            }
            viewmodel.Links = model.Links;

        }
    }
}
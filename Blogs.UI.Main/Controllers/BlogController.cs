using Blogs.Entity;
using Blogs.UI.Main.ViewModel;
using FYJ.Web.NetPager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    //首页
    public class BlogController : SharedController
    {
        [CompressAttribute]
        [MemCacheFilter]
        [CountFilter]
        public ActionResult Index()
        {
            if (Utility.BlogBll.IsBlogDisabled(this.Info.blogID + ""))
            {
                return Content("网站被暂时关闭或被禁用");
            }

            if (RouteData.Values["categoryid"] != null)
            {
                Entity.blog_tb_category cate = Utility.CategoryBll.GetEntity(RouteData.Values["categoryid"].ToString());
                if (cate.categoryIsDisabled)
                {
                    return Content("分类被暂时关闭或被禁用");
                }
            }


            IndexViewModel viewmodel = GetIndexViewModel();

            if(!String.IsNullOrEmpty(viewmodel.ThemeName))
            {
                if (viewmodel.ThemeName.Equals("cnblogs", StringComparison.CurrentCultureIgnoreCase))
                {
                    ViewBag.IndexModel = viewmodel;
                }

                if (viewmodel.ThemeName.Equals("default", StringComparison.CurrentCultureIgnoreCase))
                {
                    ViewBag.IndexModel = viewmodel;
                }
            }

            //model.TotalPV = Utility.BlogBll.GetTotalPV(BlogID);

            return View("~/Views/Blog/Themes/" + viewmodel.ThemeName + "/Index.cshtml", viewmodel);   //PartialView
        }

        private IndexViewModel GetIndexViewModel()
        {
            IndexViewModel viewmodel = new IndexViewModel();
            base.SetModel(viewmodel);
           
            int recordCount = 0;
            //当前页
            int page = Convert.ToInt32(RouteData.Values["page"]);
            if (page == 0)
            {
                page = 1;
            }
            int pageSize = 10;
            ////tag categoryID 可以传''不能传null  因为存储过程Null判断有点问题
            string categoryID = this.Info.categoryID ?? "";
            if (RouteData.Values["categoryid"] != null)
            {
                categoryID = RouteData.Values["categoryid"].ToString();
            }
            string tagID = (RouteData.Values["tagid"] == null) ? "" : RouteData.Values["tagid"].ToString();
            int year = (RouteData.Values["month"] == null) ? 0 : Int32.Parse(RouteData.Values["month"].ToString().Substring(0, 4));
            int month = (RouteData.Values["month"] == null) ? 0 : Int32.Parse(RouteData.Values["month"].ToString().Substring(4));

            IndexModel model = Utility.BlogBll.GetBlogIndexData(this.BlogID, page, pageSize, categoryID, tagID, year, month);

            viewmodel.TopArticles = model.TopArticles;
            viewmodel.IndexArticles = model.IndexArticles;
            viewmodel.NewArticles = model.NewArticles;
            viewmodel.MostViewArticles = model.MostViewArticles;
            viewmodel.MostCommentArticles = model.MostCommentArticles;
            viewmodel.RandomArticles = model.RandomArticles;
            viewmodel.categorys = model.categorys;
            viewmodel.Months = model.Months;
            viewmodel.Tags = model.Tags;
            recordCount = model.RecordCount;

            Pager pager = ActionPage(page, pageSize, recordCount);
            viewmodel.PagerHtml = pager.ToString().Replace("page-1.html", "");
            viewmodel.PagerCollection = pager.Collection;

            viewmodel.SliderCollection = Utility.SliderBll.Query(this.BlogID);
            viewmodel.MainArticlePics = Utility.BlogBll.GetMainArticlePic(this.BlogID);
            //是否远程IP地址  2015-6-16
            bool isRemote = Utility.IsRemote;
            //如果是远程 则替换图片地址为阿里云
            if (isRemote)
            {
                foreach (var item in viewmodel.SliderCollection)
                {
                    item.Pic = Utility.ReplaceImgOrFileSrc(item.Pic);
                }
            }

            return viewmodel;
        }


        private Pager ActionPage(int page, int pageSize, int recordCount)
        {
            string baseUrl = this.Info.BaseUrl;
            string urlRewritePattern = baseUrl.TrimEnd('/') + "/page-{0}.html";
            if (RouteData.Values["categoryid"] != null)
            {
                urlRewritePattern = baseUrl.TrimEnd('/') + "/cate-" + RouteData.Values["categoryid"] + "-{0}.html";
            }
            if (RouteData.Values["tagid"] != null)
            {
                urlRewritePattern = baseUrl.TrimEnd('/') + "/tag-" + RouteData.Values["tagid"] + "-{0}.html";
            }
            if (RouteData.Values["month"] != null)
            {
                urlRewritePattern = baseUrl.TrimEnd('/') + "/month-" + RouteData.Values["month"] + "-{0}.html";
            }

            Pager pager = new Pager(Request.Url)
            {
                RecordCount = recordCount,
                PageSize = pageSize,
                MaxShowPageSize = 10,
                PageIndex = page,
                ShowSpanText = false,
                EnableUrlRewriting = true,
                UrlRewritePattern = urlRewritePattern
            };

            return pager;
        }

    }
}
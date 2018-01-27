using Blogs.Entity;
using Blogs.UI.Main.ViewModel;
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class BoardController : BaseController
    {
        //
        // GET: /Board/
        [CountFilter]
        public ActionResult Index()
        {
            BoardViewModel model = new BoardViewModel();
            model.SiteID = BlogID;
            Blogs.Entity.blog_tb_blog blog = Utility.BlogBll.GetEntity(base.BlogID);
            model.IsCloseBoard = blog.IsCloseBoard;

            model.Title = "留言板-" + Info.blogTitle;

            int state = 1;
            if (HasPermission)
            {
                state = -1;
            }

            model.Collection = Utility.BoardBll.Query(state);

            return View("~/Views/Board/V1/Index.cshtml", model);
        }

        private bool HasPermission
        {
            get
            {
                if (Session["Token"] != null)
                {
                    string userID = base.CurrentUser.userID;
                    if (userID == Info.userID)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}

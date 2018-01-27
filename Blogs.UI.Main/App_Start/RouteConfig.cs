using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blogs.UI.Main
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // routes.MapRoute(
            //    name: null,
            //    url: "stock{page}.html",
            //    defaults: new { controller = "Member", action = "Index" },
            //    constraints: new { page = @"\d+" }
            //);

            //Route myRoute = new Route("{controller}/{action}", new MvcRouteHandler());

            routes.MapRoute(
              name: "",
              url: "AuthJump",
              defaults: new { controller = "AuthJump", action = "Index" }
            );

            routes.MapRoute(
             name: "",
             url: "CommentPost",
             defaults: new { controller = "CommentPost", action = "Index" }
           );

            routes.MapRoute(
            name: "",
            url: "ArticlePassword",
            defaults: new { controller = "ArticlePassword", action = "Index" }
          );

            //写在最后 避免每次都加载 MyRoute类的GetRouteData方法
            routes.Add(new MyRoute());

            //注释掉默认的 否则它先被匹配导致后面的不起作用  或者放到最后
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

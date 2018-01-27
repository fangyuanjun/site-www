using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var v = System.Configuration.ConfigurationManager.AppSettings["isCompress"];
            if(v!=null&&Convert.ToBoolean(v))
            {
                var request = filterContext.HttpContext.Request;
                var response = filterContext.HttpContext.Response;
                response.Filter = new CompressFilter(response.Filter);
            }
        }
    }
}
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class CustomExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            if (filterContext.Exception is CustomException)
            {
                JsonResult json = new JsonResult();
                json.Data = new { code = -1, message = filterContext.Exception.Message };
                filterContext.Result = json;
            }
            else
            {
                filterContext.ExceptionHandled = false;
                //log.Error("error", filterContext.Exception);
                //throw filterContext.Exception;
            }
        }
    }
}
using FYJ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class HtmlFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            try
            {

                ViewResult v = filterContext.Result as ViewResult;
                if (v != null)
                {
                    var controller = filterContext.Controller as Controller;
                    if (controller != null)
                    {
                        //string html = "";
                        //var r = filterContext.Result as ContentResult;
                        //if(r!=null)
                        //{
                        //    html = r.Content;
                        //}
                        //else
                        //{
                        //    html = ControllerHelper.RenderPartialViewToString(controller, v.ViewName, v.Model);
                        //}

                        //html = html.Replace("images.kecq.com", "static-drive.oss-cn-shenzhen.aliyuncs.com");
                        //html = html.Replace("http://static.kecq.com/", "http://static-drive.b0.upaiyun.com/");
                        //ContentResult c = new ContentResult();
                        //c.ContentType = "text/html; charset=utf-8";
                        //c.ContentEncoding = Encoding.UTF8;
                        //c.Content = html;
                        //filterContext.Result = c;
                    }
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex, "HtmlFilter");
            }
        }
    }
}

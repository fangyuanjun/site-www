using Blogs.UI.Main.Controllers;
using FYJ;
using FYJ.Web.Data;
using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class CountFilter : ActionFilterAttribute
    {
        //Action执行之后
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            try
            {
               

                //如果不是远程 并且没有开启统计本地则直接返回
                if (!Utility.IsRemote && ConfigurationManager.AppSettings["isTongjiLocal"] != "true")
                {
                    return ;
                }

                BaseController c = filterContext.Controller as BaseController;
                if (c == null)
                {
                    c = new BaseController();
                }

          
                    //ThreadContext tc = Utility.GetThreadContext(filterContext.HttpContext);
                    //ThreadPool.QueueUserWorkItem((o) =>
                    //{
                    //    try
                    //    {
                    //        ThreadContext tcc = o as ThreadContext;
                    //        AppDomain.CurrentDomain.SetData("ThreadContext", tcc);

               
                    //        Utility.BlogBll.UpdateBlogCount(c.BlogID, tcc.Ip, tcc.SessionID);

                    //        if (ConfigurationManager.AppSettings["VisitMode"] == "current")
                    //        {
                    //            Utility.Tongji(tcc, c.Tag.SiteID, c.Tag.Title, c.Tag.ObjectID);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //       LogHelper.WriteLog(ex, "子线程D更新访问次数出错");
                    //    }
                    //}, tc);
                
            }
            catch (Exception ex)
            {
               LogHelper.WriteLog(ex, "主线程更新访问次数出错");
            }
        }
    }
}
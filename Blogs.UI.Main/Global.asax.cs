using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using FYJ.Common;
using FYJ;

namespace Blogs.UI.Main
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            GlobalFilterCollection filters = GlobalFilters.Filters;
            //filters.Add(new HandleErrorAttribute());    注释掉这句才能将某些日志写入到log中

            filters.Add(new HtmlFilter());
            //filters.Add(new CountFilter());
            //filters.Add(new MemCacheFilter());

            
            //var container = new UnityContainer();
            //container.RegisterType<Blogs.IDAL.IComment, Blogs.DAL.DComment>();
            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码

        }

        /// <summary>
        /// 返回是否显示错误
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsShowError(HttpContext context)
        {
            string str = String.Empty;
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.Load(context.Server.MapPath("~/web.config"));
            return (doc.SelectSingleNode("//customErrors").Attributes["mode"].Value == "Off");
        }

        void Application_Error(object sender, EventArgs e)
        {
            if (HttpContext.Current.Server.GetLastError() is HttpRequestValidationException)
            {
                HttpContext.Current.Response.Write("请输入合法的字符串【<a href=\"javascript:history.back(0);\">返回</a>】");
                HttpContext.Current.Server.ClearError();
                return;
            }

            if (HttpContext.Current.Server.GetLastError() is CustomException)
            {
                CustomException ex = HttpContext.Current.Server.GetLastError() as CustomException;
                HttpContext.Current.Response.ContentType = "text/plain";
                HttpContext.Current.Response.Write(ex.ToString());
                HttpContext.Current.Server.ClearError();
                return;
            }

            if (HttpContext.Current.Server.GetLastError() is BlogNotFindException)
            {
                //HttpContext.Current.Response.Write("根据Uri没有获取到博客,请到后台设置正确的域名");

                string content = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/blognotfind.html"));
                HttpContext.Current.Response.ContentType = "text/html";
                content = content.Replace("你访问的地址无效", "BlogNotFindException:你访问的地址无效");
                HttpContext.Current.Response.Write(content);
                HttpContext.Current.Response.End();   //一定要加这句 否则可能输出缓存   输出2次内容
                HttpContext.Current.Server.ClearError();
                return;
            }

            //Server.GetLastError().GetBaseException();
            string url = "";
            try
            {
                url = HttpContext.Current.Request.Url.ToString();
            }
            catch { }

            Exception excep = HttpContext.Current.Server.GetLastError();
            if (excep.Message.StartsWith("The controller for path"))
            {
                //FYJ.Common.LogHelper.WriteLog("HTTP 404:" + url);
            }
            else if (excep.Message.Contains("__RequestVerificationToken"))
            {
                //FYJ.Common.LogHelper.WriteLog("跨站点攻击:" + url);
                HttpContext.Current.Response.Write(new MessageModel(-99,"服务器拒绝,原因:跨站点攻击").ToString());
                HttpContext.Current.Server.ClearError();
            }
            else
            {
               LogHelper.WriteLog(excep, Environment.NewLine + "系统异常:" + url);
            }

            if (!IsShowError(HttpContext.Current))
            {
                HttpContext.Current.Response.Write("系统运行发生异常");
                HttpContext.Current.Server.ClearError();
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码

        }

        void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。 
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为 InProc 时，才会引发 Session_End 事件。
            // 如果会话模式设置为 StateServer 
            // 或 SQLServer，则不会引发该事件。

        }

        void Application_BeginRequest(object sender, EventArgs e)
        {

        }
    }
}
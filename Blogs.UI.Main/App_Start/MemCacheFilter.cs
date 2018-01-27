using Enyim.Caching;
using FYJ;
using FYJ.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Blogs.UI.Main
{

    public class MemCacheFilter : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            if (Utility.IsUseMemcache)
            {
                try
                {
                    string url = filterContext.HttpContext.Request.Url.ToString().ToLower();
                    string key = url.Replace(" ", "").Trim();
                    string html = "";
                    var r = filterContext.Result as ContentResult;
                    if (r != null)
                    {
                        html = r.Content;
                    }
                    else
                    {
                        ViewResult v = filterContext.Result as ViewResult;
                        if (v != null)
                        {
                            var controller = filterContext.Controller as Controller;
                            if (controller != null)
                            {
                                html = ControllerHelper.RenderPartialViewToString(controller, v.ViewName, v.Model);
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(html))
                    {
                        MemcachedClient client = MemCache.Instance;

                        int se = 60;
                        if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["memcacheTimeout"]))
                        {
                            se = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["memcacheTimeout"]);
                        }

                        if (client.Store(Enyim.Caching.Memcached.StoreMode.Set, key, html, DateTime.Now.AddSeconds(se)))
                        {

                        }
                        else
                        {
                            LogHelper.WriteLog("向Memcache写入数据失败");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(ex, "向Memcache写入数据异常");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class ControllerHelper
    {
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            StringWriter sw = new StringWriter();
            ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            //ViewResult r = View("~/Views/Blog/Themes/" + article.themeName + "/Show.cshtml", model);
            //ViewEngineResult result = ViewEngines.Engines.FindView(ControllerContext, r.ViewName, null);
            //HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);
            try
            {
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
               
                viewResult.View.Render(viewContext, viewContext.Writer);

                return sw.GetStringBuilder().ToString();
                //sw.ToString
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                sw.Close();
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
            }
        }

        /// <summary>
        /// 创建静态页
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName"></param>
        /// <param name="GetModelMethod">获取模型的方法</param>
        /// <param name="folderPath">静态页保存的路径</param>
        /// <param name="indexName">如果路径为/  使用的文件名 如index.html</param>
        /// <returns></returns>
        public static ActionResult CreateStaticHtml(Controller controller, string viewName, Func<object> GetModelMethod,Func<object> GetBaseModelMethod, string folderPath, string indexName)
        {
            folderPath = folderPath.TrimEnd('/');

            int refresh = 0;
            string staticHtmlRefreshTimeout = System.Configuration.ConfigurationManager.AppSettings["staticHtmlRefreshTimeout"];
            if (!String.IsNullOrEmpty(staticHtmlRefreshTimeout))
            {
                refresh = Convert.ToInt32(staticHtmlRefreshTimeout);
            }

            //是否启用静态页
            if (refresh>0)
            {
                string absPath = controller.Request.Url.AbsolutePath.Substring(controller.Request.Url.AbsolutePath.LastIndexOf("/") + 1);
                if (absPath == "")
                {
                    absPath = indexName;
                }
                //如果目录不存在则创建
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string s = Directory.GetFiles(folderPath, "*-" + absPath).FirstOrDefault();
                string filePath = String.Empty;
                //如果强制刷新
                bool isRefresh = File.Exists(controller.HttpContext.Server.MapPath("~/App_Data/refresh.txt"));

                if (s != null && isRefresh == false)
                {
                    long updateDate = Convert.ToInt64(Regex.Match(Path.GetFileName(s), "(.*)-" + absPath).Groups[1].Value);
                    filePath = folderPath + "/" + updateDate + "-" + absPath;

                    //如果没有超过刷新时间 则从静态页读取
                    if (Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmss")) - updateDate < refresh)
                    {
                        string content = System.IO.File.ReadAllText(filePath, System.Text.Encoding.UTF8);

                        return new ContentResult { Content = content };
                    }
                }

                filePath = folderPath + "/" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + absPath;

                //删除以前生成的文件
                if (s != null)
                {
                    System.IO.File.Delete(s);
                }

                object obj = GetModelMethod.Invoke();
                controller.ViewBag.BaseModel = GetBaseModelMethod.Invoke();
                string html = ControllerHelper.RenderPartialViewToString(controller, viewName, obj);
                TextWriter w = new StreamWriter(filePath, false, System.Text.Encoding.UTF8, 4096);
                w.Write(html);
                w.Close();

                return new ContentResult { Content = html };
            }

            return null;
        }

    }
}

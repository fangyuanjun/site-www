using Blogs.Entity;
using Blogs.IBLL;
using FYJ;
using FYJ.Web.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Xml;

namespace Blogs.UI.Main
{
    public  class Utility :IBlogInfo
    {
        public static IBLLBlog BlogBll
        {
            get { return IocFactory<IBLLBlog>.Instance; }
        }


        public static IBLLCategory CategoryBll
        {
            get { return IocFactory<IBLLCategory>.Instance; }
        }

        public static IBLLArticle ArticleBll
        {
            get { return IocFactory<IBLLArticle>.Instance; }
        }




        public static IBLLComment CommentBll
        {
            get { return IocFactory<IBLLComment>.Instance; }
        }

        public static IBLLAlbum AlbumBll
        {
            get { return IocFactory<IBLLAlbum>.Instance; }
        }

        public static IBLLPhoto PhotoBll
        {
            get { return IocFactory<IBLLPhoto>.Instance; }
        }

        public static IBLLBoard BoardBll
        {
            get { return IocFactory<IBLLBoard>.Instance; }
        }

        public static IBLLSlider SliderBll
        {
            get { return IocFactory<IBLLSlider>.Instance; }
        }

        public static IBLLTalk TalkBll
        {
            get { return IocFactory<IBLLTalk>.Instance; }
        }

        public static IBLLVisit VisitBll
        {
            get { return IocFactory<IBLLVisit>.Instance; }
        }

        /// <summary>
        /// 站点统计代码
        /// </summary>
        public static string SiteVistCode
        {
            get
            {

                return GetVisitHtml(null);
            }
        }

        public static string UserID
        {
            get
            {
                if (HttpContext.Current.Session["Token"] != null)
                {
                    JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(HttpContext.Current.Session["Token"].ToString()) as JObject;
                    String userID = jobj["userID"].ToString();
                    return userID;
                }
                return null;
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["Token"] != null)
                {
                    JObject jobj = Newtonsoft.Json.JsonConvert.DeserializeObject(HttpContext.Current.Session["Token"].ToString()) as JObject;
                    String userName = jobj["userName"].ToString();
                    return userName;
                }
                return null;
            }
        }

        /// <summary>
        /// 是否远程
        /// </summary>
        public static bool IsRemote
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["isRemote"] == "true")
                {
                    return true;
                }

                //是否远程IP地址  2015-6-16
                bool isRemote = (HttpContext.Current.Request.UserHostAddress != "127.0.0.1" && HttpContext.Current.Request.UserHostAddress != "::1");

                return isRemote;
            }
        }

        public static string GetClientIP()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }



        public static string GetClientIP(HttpContextBase httpcontext)
        {
            string result = httpcontext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (null == result || result == String.Empty)
            {
                result = httpcontext.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = httpcontext.Request.UserHostAddress;
            }

            return result;
        }

        /// <summary>
        /// 是否使用memcache 可以在url后加r参数强制刷新
        /// </summary>
        public static bool IsUseMemcache
        {
            get
            {
                bool isUseMemcache = false;
                if (System.Configuration.ConfigurationManager.AppSettings["isUseMemcache"] == "true")
                {
                    isUseMemcache = true;
                }

                if (HttpContext.Current.Request["r"] != null)
                {
                    isUseMemcache = false;
                }

                return isUseMemcache;
            }
        }

        public static blog_tb_blog GetCurrentBlog()
        {
            blog_tb_blog blog = null;
            if (System.Web.HttpContext.Current.Session != null)
            {
                if (HttpContext.Current.Session["blog"] != null)
                {
                    blog = (blog_tb_blog)HttpContext.Current.Session["blog"];
                }
                else
                {
                    blog = Utility.BlogBll.GetSingleBlogByDomain(System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
                    HttpContext.Current.Session["blog"] = blog;
                }
            }
            else
            {
                blog = Utility.BlogBll.GetSingleBlogByDomain(System.Web.HttpContext.Current.Request.Url.Host, System.Web.HttpContext.Current.Request.Url.Port);
            }

            if (blog == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["isUseFirstBlogWhenNotMatch"] == "true")
                {
                    blog = Utility.BlogBll.GetFirstEntity();
                }
            }

            if (blog == null)
            {
                throw new BlogNotFindException();
            }

       
            string url = System.Web.HttpContext.Current.Request.Url.ToString().ToLower();
            if (String.IsNullOrEmpty(blog.blogDomain))
            {
                blog.BaseUrl = url.Substring(0, url.IndexOf("/", 8));
            }
            else
            {
                blog.BaseUrl = "http://" + blog.blogDomain;
            }

            blog.themeName = blog.themeID;

            //如果是远程 则替换图片地址为阿里云
            if (Utility.IsRemote)
            {
                blog.blogLogo = Utility.ReplaceImgOrFileSrc(blog.blogLogo);
            }

            return blog;
        }

        public static blog_tb_blog GetCurrentBlog(Controller controller)
        {
            blog_tb_blog info = controller.RouteData.Values["blogInfo"] as blog_tb_blog;
            if (info == null)
            {
                info = GetCurrentBlog();
                controller.RouteData.Values["blogInfo"] = info;
            }

            return info;
        }


        public static ThreadContext GetThreadContext(HttpContextBase httpcontext)
        {
            ThreadContext tc = new ThreadContext();
            tc.Domain = httpcontext.Request.Url.Host;
            tc.Ip = GetClientIP(httpcontext);
            tc.UserAgent = httpcontext.Request.UserAgent;
            tc.Url = httpcontext.Request.Url;
            if (httpcontext.Session != null)
            {
                tc.SessionID = httpcontext.Session.SessionID;
            }

            if (httpcontext.Request.UrlReferrer != null)
            {
                tc.Referrer = httpcontext.Request.UrlReferrer.ToString();
            }

            return tc;
        }



        /// <summary>
        /// 统计
        /// </summary>
        /// <param name="context"></param>
        /// <param name="title"></param>
        /// <param name="objectID"></param>
        /// <returns></returns>
        public static int Tongji(ThreadContext context, string siteID, string title, string objectID)
        {
            String country = "";
            String city = "";
            if (!String.IsNullOrEmpty(context.Ip) && context.Ip != "127.0.0.1" && context.Ip != "::1")
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch
                {

                }
            }

            blog_tb_Visit visit = new blog_tb_Visit();
            visit.visitID = Guid.NewGuid().ToString("N");
            visit.ADD_DATE = DateTime.Now;
            visit.UPDATE_DATE = DateTime.Now;
            visit.siteID = siteID;

            visit.SessionID = context.SessionID;
            visit.userAgent = context.UserAgent;
            visit.visitUrl = context.Url.ToString();
            visit.visitIP = context.Ip;
            visit.visitTitle = title;
            visit.objectID = objectID;
            visit.County = country;
            visit.City = city;
            visit.Reffer = context.Referrer;

            return VisitBll.AddVisit(visit);
        }

        /// <summary>
        /// 替换正文为阿里云、又拍云
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ReplaceImgOrFileSrc(string src)
        {
            //if (!String.IsNullOrWhiteSpace(src))
            //{
            //Match m = Regex.Match(url, "http://images.kecq.com/(.*?)/");
            //if (m.Success)
            //{
            //    string bucket = m.Groups[1].Value;
            //    string purl = Regex.Replace(src, "http://images.kecq.com/" + bucket, "http://" + bucket + ".oss-cn-shenzhen.aliyuncs.com");
            //    return purl;
            //}
            //}

            return src;
        }

        /// <summary>
        /// 替换延迟加载图片
        /// </summary>
        /// <param name="content"></param>
        /// <returns>2015-7-20</returns>
        public static string ReplaceImgLazyload(string content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return content;
            }

            if (System.Configuration.ConfigurationManager.AppSettings["isLazyloadPic"] == "true")
            {
                string reg = "<\\s*?img.+?(src=\"(http://img.kecq.com.+?)\")"; ;
                foreach (Match m in Regex.Matches(content, reg, RegexOptions.IgnoreCase))
                {
                    string src = m.Groups[1].Value;
                    string img = m.Groups[2].Value;

                    if (Regex.IsMatch(img, "\\.gif", RegexOptions.IgnoreCase))
                    {
                        continue;
                    }

                    Match m2 = Regex.Match(img, "-(\\d+)x(\\d+)\\.");
                    if (m2.Success)
                    {
                        //分辨率太小不替换
                        if (Convert.ToInt32(m2.Groups[1].Value) < 100 || Convert.ToInt32(m2.Groups[2].Value) < 100)
                        {
                            continue;
                        }
                    }
                    content = content.Replace(src, " src='http://static.kecq.com/images/common/big-loading.gif' data-original=\"" + img + "\"");
                }
            }

            return content;
        }

        /// <summary>
        /// 获取统计代码
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static string GetVisitHtml(string siteID)
        {
            if (IsRemote)
            {
                string visitHtml = "";
                Cache cache = HttpRuntime.Cache;
                object obj = cache["visitHtml"];
                if (obj == null)
                {
                    CacheDependency depend = new CacheDependency(HttpContext.Current.Server.MapPath("~/App_Data/config.xml"));
                    XmlDocument doc = new XmlDocument();
                    doc.Load(HttpContext.Current.Server.MapPath("~/App_Data/config.xml"));
                    visitHtml = doc.SelectSingleNode("//visitHtml").InnerText;
                    visitHtml = visitHtml.Replace("{siteID}", siteID);
                    cache.Insert("visitHtml", visitHtml, depend);
                }
                else
                {
                    visitHtml = obj.ToString();
                }

                return visitHtml;
            }
            else
            {
                return "";
            }
        }

        public static string AlbumVersion
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["albumVersion"] == null)
                {
                    return "V2";
                }

                return System.Configuration.ConfigurationManager.AppSettings["albumVersion"];
            }
        }

        public blog_tb_blog CurrentBlog
        {
            get
            {
                return Utility.GetCurrentBlog();
            }
        }
    }
}
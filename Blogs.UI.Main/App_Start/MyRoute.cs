using Blogs.Entity;
using Enyim.Caching;
using FYJ;
using FYJ.Web.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Blogs.UI.Main
{
    public class MyRoute : RouteBase
    {
        private Dictionary<string, RouteData> list = new Dictionary<string, RouteData>();
        //最后刷新时间
        private DateTime lastFreshDate = DateTime.Now;

        public MyRoute()
        {

        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            string url = httpContext.Request.Url.ToString().ToLower();
            //if (Regex.IsMatch(url, "CommentPost?articleID="))
            //{
            //    httpContext.Response.StatusCode = 403;
            //    return null;
            //}


            if (Utility.IsUseMemcache)
            {
                try
                {
                    //System.Net.IPEndPoint ip = new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 11211);
                    //sunny.memcached.Client m = new sunny.memcached.Client(ip, 10000);
                    //sunny.memcached.ResultBuffer result = m.Get(url);
                    //var v = result.value;
                    //if(v!=null)
                    //{
                    //    string h = System.Text.Encoding.UTF8.GetString(result.value);
                    //}

                    MemcachedClient client = MemCache.Instance;
                    string key = url.Replace(" ", "").Trim();
                    object c = client.Get(key);
                    if (c != null)
                    {
                        string html = c.ToString().Replace("<body>", "<body>\r\n<!--from memcached-->");

                        #region 统计开始
                        try
                        {
                            if ((Utility.IsRemote || ConfigurationManager.AppSettings["isTongjiLocal"] == "true"))
                            {
                                blog_tb_blog blogEntity = Utility.GetCurrentBlog();

                                string articleID = null;
                                if (Regex.IsMatch(url, "/artic-\\d+.html", RegexOptions.IgnoreCase))
                                {
                                    Match match = Regex.Match(url, "/artic-(\\d+).html");
                                    articleID = match.Groups[1].Value;

                                    ThreadPool.QueueUserWorkItem((o) =>
                                    {
                                        try
                                        {
                                            ThreadContext tc = o as ThreadContext;
                                            AppDomain.CurrentDomain.SetData("ThreadContext", tc);
                                            Utility.ArticleBll.UpdateExtend(articleID, tc.Ip, "");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.WriteLog(ex, "子线程A更新访问次数出错");
                                        }
                                    }, Utility.GetThreadContext(httpContext));
                                }

                                string title = "";
                                Match m = Regex.Match(html, @"\<title[^\>]*\>\s*(?<Title>.*?)\s*\</title\>");
                                if (m != null && m.Success)
                                {
                                    title = m.Groups[1].Value;
                                }

                                ThreadPool.QueueUserWorkItem((o) =>
                                {
                                    try
                                    {
                                        ThreadContext tc = o as ThreadContext;
                                        AppDomain.CurrentDomain.SetData("ThreadContext", tc);
                                        if (ConfigurationManager.AppSettings["VisitMode"] == "current")
                                        {
                                            Utility.Tongji(tc, blogEntity.blogID + "", title, articleID);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        LogHelper.WriteLog(ex, "子线程B更新访问次数出错");
                                    }
                                }, Utility.GetThreadContext(httpContext));


                                if (blogEntity != null)
                                {
                                    ThreadPool.QueueUserWorkItem((o) =>
                                    {
                                        try
                                        {
                                            ThreadContext tc = o as ThreadContext;
                                            AppDomain.CurrentDomain.SetData("ThreadContext", tc);
                                            Utility.BlogBll.UpdateBlogCount(blogEntity.blogID + "", tc.Ip, "");
                                        }
                                        catch (Exception ex)
                                        {
                                            LogHelper.WriteLog(ex, "子线程C更新访问次数出错");
                                        }
                                    }, Utility.GetThreadContext(httpContext));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteLog(ex, "主线程更新访问次数出错");
                        }
                        #endregion 统计结束

                        httpContext.Response.AddHeader("Content-Type", "text/html; charset=utf-8");
                        if (System.Configuration.ConfigurationManager.AppSettings["VisitMode"] == "js")
                        {
                            //如果是远程IP地址或者本地访问也统计则返回统计代码  否则返回空字符串
                            if (Utility.IsRemote || ConfigurationManager.AppSettings["isTongjiLocal"] == "true")
                            {
                                if (!html.Contains(Utility.SiteVistCode))
                                {
                                    html = html.Replace("</body>", Utility.SiteVistCode + "</body>");
                                }
                            }
                        }

                        httpContext.Response.Write(html);
                        httpContext.Response.End();
                        return null;
                    }
                }
                catch (BlogNotFindException)
                {
                    throw;
                }
                catch (CustomException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    if (!(ex is ThreadAbortException))
                    {
                        LogHelper.WriteLog(ex, "Memcache错误");
                        return null;
                    }
                }
            }

            if (url.Contains("{1}"))
            {
                return null;
            }
            if (url.Contains("browsername="))
            {
                return null;
            }

            int routeCacheTimeout = 60;
            if (!String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["routeCacheTimeout"]))
            {
                routeCacheTimeout = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["routeCacheTimeout"]);
            }
            //刷新一次保存的路由
            if (DateTime.Now.Subtract(lastFreshDate).TotalSeconds >= routeCacheTimeout)
            {
                lastFreshDate = DateTime.Now;
                list.Clear();
            }

            if (list.ContainsKey(url))
            {
                return list[url];
            }

            blog_tb_blog info = Utility.GetCurrentBlog();
            IocManager.PutInstance("domain", info.blogDomain);
            return BlogRewriter(httpContext, info, httpContext.Request.Url);
        }


        private RouteData BlogRewriter(HttpContextBase httpContext, blog_tb_blog info, Uri uri)
        {
            string url = uri.ToString().ToLower();

            RouteData result = null;
            //string baseUrl = info.BaseUrl;
            //if (!info.BaseUrl.Contains(uri.Host))
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["isUseFirstBlogWhenNotMatch"] == "true")
            //    {
            //        baseUrl = "";
            //    }
            //}
            string baseUrl = "";

            #region 主页重写
            if (uri.AbsolutePath == "/")
            {
                result = new RouteData(this, new MvcRouteHandler());
                string userAgent = httpContext.Request.ServerVariables["HTTP_USER_AGENT"];
                //ipad Safari
                //Mozilla/5.0 (iPad; CPU OS 6_0 like Mac OS X) AppleWebKit/536.26 (KHTML, like Gecko) Version/6.0 Mobile/10A403 Safari/8536.25
                //ipad uc
                //Mozilla/5.0 (iPad; U; CPU OS 6 like Mac OS X; zh-cn Model:iPad3,1) UC AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9B176 Safari/7543.48.3
                //android uc
                //Mozilla/5.0 (Linux; U; Android 4.0.4; zh-cn; MI 1S Build/IMM76D) UC AppleWebKit/534.31 (KHTML, like Gecko) Mobile Safari/534.31

                //if (userAgent.ToLower().Contains("android"))
                //{
                //    result.Values.Add("controller", "Mobile");
                //    result.Values.Add("action", "Index");
                //    result.Values.Add("blogInfo",info);
                //}
                //else
                //{
                result.Values.Add("controller", "Blog");
                result.Values.Add("action", "Index");
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
                //}
            }

            #endregion

            #region SEO
            if (Regex.IsMatch(url, baseUrl + "/sitemap.xml$", RegexOptions.IgnoreCase))
            {
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Sitemap");
                result.Values.Add("action", "Xml");
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/sitemap.html", RegexOptions.IgnoreCase))
            {
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Sitemap");
                result.Values.Add("action", "Html");
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/sitemap.txt$", RegexOptions.IgnoreCase))
            {
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Sitemap");
                result.Values.Add("action", "Text");
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/robots.txt$", RegexOptions.IgnoreCase))
            {
                string format = @"
#
# robots.txt for {0}
#

User-agent: *
Allow: /
Sitemap:http://{0}/sitemap.xml
Sitemap:http://{0}/sitemap.html
";
                httpContext.Response.ContentType = "text/plain";
                httpContext.Response.Write(String.Format(format, baseUrl.Replace("http://", "").Trim('/')));
                httpContext.Response.End();
            }

            #endregion

            if (Regex.IsMatch(url, baseUrl + "/page-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/page-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Blog");
                result.Values.Add("action", "Index");
                result.Values.Add("page", match.Groups[1].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/cate-\\d+-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/cate-(\\d+)-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Blog");
                result.Values.Add("action", "Index");
                result.Values.Add("categoryid", match.Groups[1].Value);
                result.Values.Add("page", match.Groups[2].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/tag-\\d+-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/tag-(\\d+)-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Blog");
                result.Values.Add("action", "Index");
                result.Values.Add("tagid", match.Groups[1].Value);
                result.Values.Add("page", match.Groups[2].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/month-\\d{6}-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/month-(\\d{6})-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Blog");
                result.Values.Add("action", "Index");
                result.Values.Add("month", match.Groups[1].Value);
                result.Values.Add("page", match.Groups[2].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }

            if (Regex.IsMatch(url, baseUrl + "/artic-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/artic-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Article");
                result.Values.Add("action", "Index");
                result.Values.Add("id", match.Groups[1].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }


            if (Regex.IsMatch(url, baseUrl + "/comment-\\w+-\\d+.html", RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(url, baseUrl + "/comment-(\\w+)-(\\d+).html");
                result = new RouteData(this, new MvcRouteHandler());
                result.Values.Add("controller", "Comment");
                result.Values.Add("action", "Index");
                result.Values.Add("id", match.Groups[1].Value);
                result.Values.Add("page", match.Groups[2].Value);
                result.Values.Add("blogInfo", info);
                if (!list.ContainsKey(url))
                {
                    list.Add(url, result);
                }
                return result;
            }


            return result;
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            // return new VirtualPathData(this, "This-is-a-Test-URL");
            return null;
        }
    }
}
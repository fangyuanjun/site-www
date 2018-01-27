using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Passport.Sso
{
    public class AuthLoad
    {
        private string loginUrl = ConfigurationManager.AppSettings["PassportRootUrl"].TrimEnd('/')+"/Login";

        #region  属性
        /// <summary>
        /// 如果未登录是否跳转到登录页 默认否
        /// </summary>
        public bool IsRedirect { get; set; }

        /// <summary>
        /// 是否清除Session 默认否  
        /// 因为不同域a.com与b.com单点注销的问题，不同域的session不共享 a.com session注销了b.com session还在   所以每次清除session每次都去登录服务验证
        /// </summary>
        public bool IsClearSession { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return HttpContext.Current.Session["Token"] != null;
            }
        }

        /// <summary>
        /// 获取登录后返回的用户信息
        /// </summary>
        public string User
        {
            get
            {
                return HttpContext.Current.Session["Token"] as string;
            }
        }
        #endregion

        #region 登录
        public void Login()
        {
            if (IsClearSession == false)
            {
                if (HttpContext.Current.Session["Token"] != null)   //因为不同域a.com与b.com单点注销的问题，不同域的session不共享 a.com session注销了b.com session还在 ,注释掉这里，让继承它的子页面每次都提交请求  
                {
                    //分站凭证存在
                    // Response.Write("恭喜，分站凭证存在，您被授权访问该页面！");
                    return;
                }
            }

            //令牌验证结果
            if (HttpContext.Current.Request.QueryString["Token"] != null)
            {
                if (HttpContext.Current.Request.QueryString["Token"] != "$Token$")
                {
                    //持有令牌
                    string tokenValue = HttpContext.Current.Request.QueryString["Token"];
                    //获取主站凭证
                    string s = AuthHelper.DoGet(loginUrl + "/TokenGetCredence?token="+tokenValue);
                    if (s.Trim()!="")
                    {
                        //令牌正确
                        HttpContext.Current.Session["Token"] = s;
                        // Response.Write("恭喜，令牌存在，您被授权访问该页面！");
                    }
                    else    //令牌错误
                    {
                        if (IsClearSession)
                        {
                            HttpContext.Current.Session["Token"] = null;
                            //因为不同域a.com与b.com单点注销的问题，不同域的session不共享 a.com session注销了b.com session还在   所以使用上句清除session
                        }
                       
                        if (IsRedirect)
                        {
                            HttpContext.Current.Response.Redirect(this.replaceToken());
                        }
                    }
                }
                else
                {
                    //未持有令牌
                    if (IsRedirect)
                    {
                        HttpContext.Current.Response.Redirect(this.replaceToken());
                    }
                }
            }
            //未进行令牌验证，去主站验证
            else
            {
                HttpContext.Current.Response.Redirect(this.getTokenURL());
            }
        }
        #endregion

        #region  注销
        public void LogOut()
        {

            if (HttpContext.Current.Request.QueryString["Token"] == null)
            {
                HttpContext.Current.Response.Redirect(getTokenURL());
            }
            else
            {
                if (HttpContext.Current.Request.QueryString["Token"] != "$Token$")
                {
                    string token = HttpContext.Current.Request.QueryString["Token"];
                    AuthHelper.DoGet(loginUrl + "/ClearToken?token=" + token);
                }
                else
                {
                    HttpContext.Current.Response.Redirect(replaceToken());
                }
            }
        }
        #endregion

        #region  私有方法
        /// <summary>
        /// 获取带令牌请求的URL
        /// 在当前URL中附加上令牌请求参数
        /// </summary>
        /// <returns></returns>
        private string getTokenURL()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            Regex reg = new Regex(@"^.*\?.+=.+$");
            if (reg.IsMatch(url))
                url += "&Token=$Token$";
            else
                url += "?Token=$Token$";

            return loginUrl + "/GetToken?BackURL=" + HttpContext.Current.Server.UrlEncode(url);
        }

        /// <summary>
        /// 去掉URL中的令牌
        /// 在当前URL中去掉令牌参数
        /// </summary>
        /// <returns></returns>
        private string replaceToken()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            url = Regex.Replace(url, @"(\?|&)Token=.*", "", RegexOptions.IgnoreCase);
            return loginUrl + "?BackURL=" + HttpContext.Current.Server.UrlEncode(url);
        }
        #endregion
    }
}

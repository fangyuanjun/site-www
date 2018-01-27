using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Passport.Sso
{
   public class AuthHelper
    {

       public static String GetValue(string json, string name)
       {
           String temp = json.Replace("\"", "").Replace("'", "").Replace("\\", "");
           if (temp.Contains(name + ":"))
           {
               temp = temp.Substring(temp.IndexOf(name) + name.Length + 1);
               if (temp.IndexOf(",") != -1)
                   return temp.Substring(0, temp.IndexOf(","));
               else
                   if (temp.IndexOf("}") != -1)
                       return temp.Substring(0, temp.IndexOf("}"));
                   else
                       return temp;
           }
           return null;
       }

       public static String DoGet(string url)
       {
           HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
           HttpWebResponse res = (HttpWebResponse)req.GetResponse();
           StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
           string backstr = sr.ReadToEnd();
           sr.Close();
           res.Close();
           return backstr;
       }

       /// <summary>
       /// 获取登录用户id
       /// </summary>
       /// <returns></returns>
       public static string GetUserID()
       {
           if (System.Web.HttpContext.Current.Session["Token"] == null)
           {
               return null;
           }
           string userID = GetValue(System.Web.HttpContext.Current.Session["Token"].ToString(),"userID");
           return userID;
       }

       /// <summary>
       /// 获取登录用户名
       /// </summary>
       /// <returns></returns>
       public static string GetUserName()
       {
           if (System.Web.HttpContext.Current.Session["Token"] == null)
           {
               return null;
           }
           string userID = GetValue(System.Web.HttpContext.Current.Session["Token"].ToString(), "userName");
           return userID;
       }
    }
}

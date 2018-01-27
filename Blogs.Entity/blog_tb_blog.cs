using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FYJ;

namespace Blogs.Entity
{

    public class blog_tb_blog 
    {

        ///<summary>
        /// 
        ///</summary>
        public Int32 blogID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogName { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String userID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogTitle { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        public String blogSubTitle { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogLogo { get; set; }

        ///<summary>
        /// 首要域名  
        ///</summary>
        public String blogDomain { get; set; }


        /// <summary>
        /// 首要端口
        /// </summary>
        public int Port { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogNotice { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogKeywords { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String blogDescription { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime blogAddDatetime { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String themeID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean blogIsDisabled { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 blogOrder { get; set; }

        ///<summary>
        /// 统计代码
        ///</summary>
        public String tongji { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String beian { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String faviconIcon { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime ADD_DATE { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime UPDATE_DATE { get; set; }

        ///<summary>
        ///关于我
        ///</summary>
        public String AboutMe { get; set; }

        ///<summary>
        ///QQ
        ///</summary>
        public String QQ { get; set; }

        ///<summary>
        ///QQ群
        ///</summary>
        public String QQGroup { get; set; }

        ///<summary>
        ///QQ链接
        ///</summary>
        public String QQLink { get; set; }

        ///<summary>
        ///新浪微博
        ///</summary>
        public String Weibo { get; set; }

        ///<summary>
        ///新浪微博链接
        ///</summary>
        public String WeiboLink { get; set; }

        ///<summary>
        ///邮箱
        ///</summary>
        public String Email { get; set; }

        ///<summary>
        ///电话
        ///</summary>
        public String Tel { get; set; }

        ///<summary>
        ///地址
        ///</summary>
        public String Address { get; set; }

        ///<summary>
        ///腾讯微博
        ///</summary>
        public String QQWeibo { get; set; }

        ///<summary>
        ///腾讯微博链接
        ///</summary>
        public String QQWeiboLink { get; set; }

        ///<summary>
        ///微信
        ///</summary>
        public String Weixin { get; set; }

        /// <summary>
        /// 是否强制启用HTTPS
        /// </summary>
        public bool IsMustSSL { get; set; }

        /// <summary>
        /// 是否关闭评论
        /// </summary>
        public bool IsCloseComment { get; set; }

        /// <summary>
        /// 是否关闭留言板
        /// </summary>
        public bool IsCloseBoard { get; set; }


        /// <summary>
        /// 如果当前url解析后是分类 
        /// </summary>
        [Ignore]
        public string categoryID { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        [Ignore]
        public string themeName { get; set; }

        /// <summary>
        /// 根Url
        /// </summary>
        [Ignore]
        public string BaseUrl { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Ignore]
        public bool isDisabled { get; set; }

        /// <summary>
        /// 包含的css和js
        /// </summary>
        [Ignore]
        public string linkScript { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.Models
{
    /// <summary>
    /// 文章缓存实体
    /// </summary>
    public class ArticleCacheModel
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public int articleID { get; set; }

        /// <summary>
        /// 文章Html
        /// </summary>
        public string articleHtml { get; set; }

        /// <summary>
        /// 缓存更新时间
        /// </summary>
        public DateTime updateDate { get; set; }
    }
}
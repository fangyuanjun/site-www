using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
    public class sitemap
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Mtype { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string lastmod { get; set; }

        /// <summary>
        /// 更新频率  daily    never
        /// </summary>
        public string changefreq { get; set; }

        /// <summary>
        /// 权重
        /// </summary>
        public string priority { get; set; }

        /// <summary>
        /// 链接  同Url属性
        /// </summary>
        public string loc
        {
            get
            {
                return Url;
            }
        }
    }
}

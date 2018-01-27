using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class ArticlePic
    {
        public string articleID { get; set; }

        public string articleTitle { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string articlePic { get; set; }

        /// <summary>
        /// 文章链接
        /// </summary>
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetArticleUrl(articleID);
            }
        }
    }
}

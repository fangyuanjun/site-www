using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class SharedModel
    {
        public blog_tb_blog Blog { get; set; }

        /// <summary>
        /// 总文章数
        /// </summary>
        public int TotalArticleCount { get; set; }

        /// <summary>
        /// 总文章点击次数
        /// </summary>
        public int TotalArticleViewCount { get; set; }

        /// <summary>
        /// 总文章评论次数
        /// </summary>
        public int TotalArticleCommentCount { get; set; }


        public List<blog_tb_menu> Menus { get; set; }

        public List<blog_tb_link> Links { get; set; } 

    }
}

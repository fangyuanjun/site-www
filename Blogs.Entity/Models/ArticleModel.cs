using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class ArticleModel
    {
        public ArticleShow CurrentArticle { get; set; }

        public List<CommentState> CommentStateCollection { get; set; }

        public List<CommentTag> CommentTagCollection { get; set; }

        /// <summary>
        /// 评论条数
        /// </summary>
        public int CommentCount { get; set; }

        /// <summary>
        /// 上一篇
        /// </summary>
        public ArticleLink BeforeArticle { get; set; }

        /// <summary>
        /// 下一篇
        /// </summary>
        public ArticleLink AfterArticle { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        public List<blog_tb_comment> CommentCollection { get; set; }


    }
}

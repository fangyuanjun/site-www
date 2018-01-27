using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
    public class IndexModel
    {
 
        /// <summary>
        /// 最新文章
        /// </summary>
        public IEnumerable<ArticleLink> NewArticles { get; set; }

        /// <summary>
        /// 最多浏览文章
        /// </summary>
        public IEnumerable<ArticleLink> MostViewArticles { get; set; }

        /// <summary>
        /// 最多评论文章
        /// </summary>
        public IEnumerable<ArticleLink> MostCommentArticles { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public IEnumerable<blog_tb_category> categorys { get; set; }

        /// <summary>
        /// 文章归档
        /// </summary>
        public IEnumerable<Month> Months { get; set; }


        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<blog_tb_tag> Tags { get; set; }


        /// <summary>
        /// 随机10条文章
        /// </summary>
        public IEnumerable<ArticleLink> RandomArticles { get; set; }

        /// <summary>
        /// 置顶文章
        /// </summary>
        public IEnumerable<ArticleIndexLink> TopArticles { get; set; }

        /// <summary>
        /// 首页文章
        /// </summary>
        public IEnumerable<ArticleIndexLink> IndexArticles { get; set; }

        /// <summary>
        /// 文章数
        /// </summary>
        public int RecordCount { get; set; }
    }
}

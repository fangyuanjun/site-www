using Blogs.Entity;
using Blogs.UI.Main.Models;
using FYJ.Web.NetPager;
using System.Collections.Generic;

namespace Blogs.UI.Main.ViewModel
{
    public class IndexViewModel : SharedViewModel
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
        /// 标签
        /// </summary>
        public IEnumerable<blog_tb_tag> Tags { get; set; }

        /// <summary>
        /// 文章归档
        /// </summary>
        public IEnumerable<Month> Months { get; set; }

        /// <summary>
        /// 随机10条文章
        /// </summary>
        public IEnumerable<ArticleLink> RandomArticles { get; set; }

        /// <summary>
        /// 置顶文章
        /// </summary>
        public IEnumerable<ArticleIndexLink> TopArticles { get; set; }

        /// <summary>
        /// 首页大图
        /// </summary>
        public IEnumerable<blog_tb_slider> SliderCollection { get; set; }

        /// <summary>
        /// 首页文章
        /// </summary>
        public IEnumerable<ArticleIndexLink> IndexArticles { get; set; }

        private IEnumerable<PagerModel> _pagerCollection = new List<PagerModel>();
        /// <summary>
        /// 分页
        /// </summary>
        public IEnumerable<PagerModel> PagerCollection
        {
            get { return _pagerCollection; }
            set { _pagerCollection = value; }
        }

        /// <summary>
        /// 分页html
        /// </summary>
        public string PagerHtml
        {
            get;
            set;
        }

        /// <summary>
        /// 文章的轮播图片
        /// </summary>
        public IEnumerable<ArticlePic> MainArticlePics { get; set; }


        /// <summary>
        /// 总访问量
        /// </summary>
        public int TotalPV { get; set; }
    }
}
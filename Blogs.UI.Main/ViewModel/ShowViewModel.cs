using Blogs.UI.Main.Models;
using System.Collections.Generic;
using Blogs.Entity;

namespace Blogs.UI.Main.ViewModel
{
    public class ShowViewModel : SharedViewModel
    {

        public ArticleShow CurrentArticle { get; set; }

        public IEnumerable<CommentState> CommentStateCollection { get; set; }

        public IEnumerable<CommentTag> CommentTagCollection { get; set; }

        public IEnumerable<blog_tb_comment> CommentCollection { get; set; }

        public string CommentPagerHtml { get; set; }

        /// <summary>
        /// 申明
        /// </summary>
        public string Shenming { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public IEnumerable<blog_attachment> AttachmentCollection { get; set; }
      
        /// <summary>
        /// 上一篇
        /// </summary>
        public ArticleLink BeforeArticle { get; set; }

        /// <summary>
        /// 下一篇
        /// </summary>
        public ArticleLink AfterArticle { get; set; }

        /// <summary>
        /// 评论是否关闭
        /// </summary>
        public bool IsCloseComment { get; set; }
    }
}
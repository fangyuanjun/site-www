using Blogs.Entity;
using Blogs.UI.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class CommentViewModel : BaseViewModel
    {
       
        /// <summary>
        /// 评论列表
        /// </summary>
        public IEnumerable<blog_tb_comment> Collection { get; set; }

        /// <summary>
        /// 分页html
        /// </summary>
        public string CommentPagerHtml { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public int ArticleID { get; set; }

        public IEnumerable<CommentState> CommentStateCollection { get; set; }

        public IEnumerable<CommentTag> CommentTagCollection { get; set; }
    }
}
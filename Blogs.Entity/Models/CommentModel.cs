using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.Entity
{
    public class CommentModel
    {

        public IEnumerable<CommentState> CommentStateCollection { get; set; } 

        public IEnumerable<CommentTag> CommentTagCollection { get; set; } 

        /// <summary>
        /// 总评论数
        /// </summary>
        public int RecordCount { get; set; }

        /// <summary>
        /// 评论列表
        /// </summary>
        public IEnumerable<blog_tb_comment> CommentCollection { get; set; }

    }
}
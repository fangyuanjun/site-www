using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.Entity
{
    public class CommentTag
    {
        public string commentid { get; set; }

        public int supportCount { get; set; }

        public string commentText { get; set; }

        /// <summary>
        /// 背景色
        /// </summary>
        public string backgroundColor { get; set; }
    }
}
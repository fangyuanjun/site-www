using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{

    public partial class blog_tb_comment 
    {
        public string commentID { get; set; }
        public string parentID { get; set; }
        public int articleID { get; set; }
        public string userID { get; set; }
        public string userName { get; set; }
        public string commentIP { get; set; }
        public string commentCountry { get; set; }
        public string commentCity { get; set; }
        public string commentContent { get; set; }
        public string commentText { get; set; }
        public int commentState { get; set; }
        public int floor { get; set; }
        public int supportCount { get; set; }
        public int againstCount { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }

        [Ignore]
        public string AuthorUserID { get; set; }

        [Ignore]
        public string Author { get; set; }

        [Ignore]
        public int ChildCount { get; set; }

        [Ignore]
        public string Mark { get; set; }
    }
}

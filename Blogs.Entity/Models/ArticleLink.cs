using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
    public class ArticleLink
    {
        public int articleID { get; set; }

        public string articleTitle { get; set; }

        public System.DateTime articleDatetime { get; set; }

        public int articleClickTimes { get; set; }

        public int articleCommentTimes { get; set; }

        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetArticleUrl(articleID + "");
            }
        }

        public string ShowTitle
        {
            get
            {
                return articleTitle;
            }
        }
    }
}

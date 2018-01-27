using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{
    public partial class blog_tb_article
    {
        public int articleID { get; set; }
        public int blogID { get; set; }
        public int siteCategoryID { get; set; }
        public int categoryID { get; set; }
        public int topicID { get; set; }
        public string articleTitle { get; set; }
        public string articleTitleColor { get; set; }
        public string articleKeywords { get; set; }
        public string articleDescription { get; set; }

        public string articleAuthor { get; set; }

        public string articlePic { get; set; }
        public string articleThumbPic { get; set; }
        public System.DateTime articleDatetime { get; set; }
        public string articleRedirectUrl { get; set; }
        public string articleAddUserID { get; set; }
        public string articleModifyUserID { get; set; }
        public int articleOrder { get; set; }
        public int articleReplyCount { get; set; }
        public bool articleIsTop { get; set; }
        public bool articleIsDisabled { get; set; }
        public int articleCommentLimit { get; set; }
    
        public bool articleIsOriginal { get; set; }
        public bool articleIsSystem { get; set; }
        public bool articleIsPic { get; set; }

        public bool articleIsHidden { get; set; }

        public bool articleIsDelete { get; set; }
        public int articleAttachmentLimit { get; set; }
        public string articlePassword { get; set; }
        public string articlePostBy { get; set; }
        public string themeID { get; set; }

        public bool IsShowSource { get; set; }

        public string articleSource { get; set; }

        public string articleSourceUrl { get; set; }

        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }

        [Ignore]
        public int articleClickTimes { get; set; }

        [Ignore]
        public int articleCommentTimes { get; set; }

        [Ignore]
        public string articleSubContentText { get; set; }

        [Ignore]
        public string articleSubContentHtml { get; set; }

        [Ignore]
        public string articleContent { get; set; }

        /// <summary>
        /// 隐藏内容
        /// </summary>
        [Ignore]
        public string articleHideContent { get; set; }

        [Ignore]
        public string categoryDomain { get; set; }


        [Ignore]
        public string categoryUrl { get; set; }

        [Ignore]
        public string categoryDisplay { get; set; }


        [Ignore]
        public List<blog_tb_tag> TagCollection { get; set; } = new List<blog_tb_tag>();

        [Ignore]
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetArticleUrl(articleID + "");
            }
        }

        private string _showTitle;
        [Ignore]
        public string ShowTitle
        {
            get
            {
                if(String.IsNullOrEmpty(_showTitle))
                {
                    return articleTitle;
                }

                return _showTitle;
            }
            set
            {
                _showTitle = value;
            }
        }
    }
}

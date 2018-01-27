using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class ArticleIndexLink : ArticleLink
    {
        public int categoryID { get; set; }
      
        public string categoryDisplay { get; set; }

        public string categoryDomain { get; set; }

        public string articlePassword { get; set; }

        public string articlePic { get; set; }
        public string articleThumbPic { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string articleAuthor { get; set; }

        public bool articleIsTop { get; set; }

        public string articleTitleColor { get; set; }


        public List<blog_tb_tag> TagCollection { get; set; } = new List<blog_tb_tag>();


        private string _articleSubContentText;
        public string articleSubContentText
        {
            get
            {
                if (!String.IsNullOrEmpty(articlePassword))
                {
                    return "";
                }

                return _articleSubContentText;
            }
            set { _articleSubContentText = value; }
        }


        private string _articleSubContentHtml;
        public string articleSubContentHtml
        {
            get
            {
                if (!String.IsNullOrEmpty(articlePassword))
                {
                    return "";
                }

                return _articleSubContentHtml;
            }
            set { _articleSubContentHtml = value; }
        }


        private string _showTitle;
        public new string ShowTitle
        {
            get
            {
                if (String.IsNullOrEmpty(_showTitle))
                {
                    string title = "";
                    if (articleIsTop)
                    {
                        title = "[顶]" + articleTitle;
                    }

                    if (!String.IsNullOrEmpty(articleTitleColor))
                    {
                        title = "<font color='" + articleTitleColor + "'>" + title + "</font>";
                    }

                    return title;
                }
                else
                {
                    return _showTitle;
                }
            }
            set
            {
                _showTitle = value;
            }
        }

        public string categoryUrl
        {
            get
            {
              return FYJ.IocFactory<IBlogFix>.Instance.GetCategoryUrl(categoryID + "", categoryDomain);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class ArticleShow :ArticleIndexLink
    {
        public bool articleIsOriginal { get; set; }

        public bool IsShowSource { get; set; }

        public string articleSource { get; set; }

        public string articleSourceUrl { get; set; }

        public string articleHideContent { get; set; }

        public int articleAttachmentLimit { get; set; }

        public string articleContent { get; set; }

        //    if (item.articleIsOriginal)
        //    {
        //        item.articleSource = "本站原创";
        //    }


        //    if (!String.IsNullOrEmpty(item.articleSourceUrl))
        //    {
        //        //return "本文转载自:<a href=\"" + _articleSourceUrl + "\" target=\"_blank\">" + _articleSourceUrl + "</a>";
        //        item.articleSourceUrl = "本文转载自:" + item.articleSourceUrl;
        //    }

        //    if (!item.IsShowSource)
        //    {
        //        item.articleSource = "";
        //        item.articleSourceUrl = "";
        //    }
    }
}

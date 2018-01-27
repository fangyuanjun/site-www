using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
    public interface IBlogFix
    {
      
        string GetMenuUrl( string menuUrl);

        string GetCategoryUrl(string categoryID,string categoryDomain);

        string GetMonthUrl(string yyyy, string mm);

        string GetArticleUrl(string articleID);

        string GetTagUrl(string tagID);


        IEnumerable<blog_tb_comment> GetComments(int articleCommentLimit,IEnumerable<blog_tb_comment> comments);
    }
}

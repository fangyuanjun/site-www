using Blogs.Entity;
using Blogs.UI.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class NoteViewModel :SharedViewModel
    {
        public IEnumerable<blog_tb_article> ListArticleCollection { get; set; }

    }
}
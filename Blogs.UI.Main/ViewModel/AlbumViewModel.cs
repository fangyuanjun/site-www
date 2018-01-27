using Blogs.Entity;
using Blogs.UI.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class AlbumViewModel :BaseViewModel
    {
        /// <summary>
        /// 相册集合
        /// </summary>
        public List<blog_tb_Album> AlbumCollection{get; set;}
    }
}
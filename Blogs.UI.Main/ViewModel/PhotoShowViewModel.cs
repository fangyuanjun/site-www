using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class PhotoShowViewModel : BaseViewModel
    {
     
        /// <summary>
        /// 当前大图
        /// </summary>
        public string CurrentUrl { get; set; }

        /// <summary>
        /// 当前缩略图
        /// </summary>
        public string CurrentThumbUrl { get; set; }

        /// <summary>
        ///图片列表
        /// </summary>
        public List<blog_tb_Photo> PhotoCollection {get;set; }

    }
}
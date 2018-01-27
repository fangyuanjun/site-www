using Blogs.Entity;
using System.Collections.Generic;

namespace Blogs.UI.Main.ViewModel
{
    public class PhotoListViewModel : BaseViewModel
    {
      
        /// <summary>
        /// 图片列表
        /// </summary>
        public List<blog_tb_Photo> PhotoCollection
        {
            get;
            set;
        }

    }
}
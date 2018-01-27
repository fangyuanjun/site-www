using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class BoardViewModel : BaseViewModel
    {
        /// <summary>
        /// 留言内容
        /// </summary>
        public IEnumerable<Entity.blog_tb_Board> Collection { get; set; }

        /// <summary>
        /// 留言是否关闭
        /// </summary>
        public bool IsCloseBoard { get; set; }

    }
}
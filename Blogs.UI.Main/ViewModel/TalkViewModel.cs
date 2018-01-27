using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.UI.Main.ViewModel
{
    public class TalkViewModel :SharedViewModel
    {
        /// <summary>
        /// 碎言碎语
        /// </summary>
        public IEnumerable<Entity.blog_tb_Talk> TalkCollection { get; set; }
    }
}
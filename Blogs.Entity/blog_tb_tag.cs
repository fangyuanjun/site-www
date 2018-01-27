using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{

    public partial class blog_tb_tag 
    {
        public int tagID { get; set; }
        public int blogID { get; set; }
        public int categoryID { get; set; }
        public int tagOrder { get; set; }
        public string tagName { get; set; }
        public string tagDisplay { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }

        /// <summary>
        /// 拥有的文章总数
        /// </summary>
        [Ignore]
        public int ArticleCount { get; set; }

 
        /// <summary>
        /// 标签Url   不受设置Url属性的影响
        /// </summary>
        [Ignore]
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetTagUrl(tagID+"");
            }
        }

        /// <summary>
        /// 标签样式
        /// </summary>
        [Ignore]
        public string style { get; set; }
    }
}

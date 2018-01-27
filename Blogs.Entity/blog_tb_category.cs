using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{
    public partial class blog_tb_category 
    {
        public int categoryID { get; set; }
        public int parentID { get; set; }
        public int blogID { get; set; }
        public string categoryName { get; set; }
        public string categoryDisplay { get; set; }
        public int categoryOrderWeight { get; set; }
        public string categoryPic { get; set; }
        public string categoryKeywords { get; set; }
        public string categoryDescription { get; set; }
        public string categoryDomain { get; set; }
        public bool categoryIsDisabled { get; set; }
        public bool categoryIsSystem { get; set; }
        public string themeID { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }


        /// <summary>
        /// 拥有的文章总数
        /// </summary>
        [Ignore]
        public int ArticleCount { get; set; }

        /// <summary>
        /// 分类Url  
        /// </summary>
        [Ignore]
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetCategoryUrl(categoryID + "",categoryDomain);
            }
        }

    }
}

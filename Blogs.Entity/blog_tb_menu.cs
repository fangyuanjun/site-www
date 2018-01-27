using FYJ;
using System;
using System.Collections.Generic;

namespace Blogs.Entity
{
  
    public partial class blog_tb_menu 
    {
        public string menuID { get; set; }
        public int blogID { get; set; }
        public string parentID { get; set; }
        public string menuClass { get; set; }
        public string menuName { get; set; }
        public string menuDisplay { get; set; }

        public string menuUrl { get; set; }
        
        public string menuPic { get; set; }
        public int menuOrder { get; set; }
        public bool menuIsDisabled { get; set; }
        public string menuTarget { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }

        [Ignore]
        public bool IsActive { get; set; }

        [Ignore]
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetMenuUrl(menuUrl);
            }
        }
    }
}

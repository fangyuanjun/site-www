//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Blogs.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class blog_tb_Visit
    {
        public string visitID { get; set; }

        public string Domain { get; set; }

        public string siteID { get; set; }
        public string objectID { get; set; }

        public string Reffer { get; set; }

        public string visitUrl { get; set; }
        public string visitTitle { get; set; }
        public string visitIP { get; set; }
        public string userAgent { get; set; }
        public System.DateTime ADD_DATE { get; set; }
        public System.DateTime UPDATE_DATE { get; set; }
        public string SessionID { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }
}

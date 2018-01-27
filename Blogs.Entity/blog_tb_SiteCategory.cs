using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_SiteCategory
    {

        ///<summary>
        /// 
        ///</summary>
        public Int32 ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 parentID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Name { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Display { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 OrderWeight { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Pic { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean IsDisabled { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime ADD_DATE { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime UPDATE_DATE { get; set; }



    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_blog_count 
    {
        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 BlogID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 PV { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 AddPV { get; set; }

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
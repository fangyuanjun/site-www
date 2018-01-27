using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_slider 
    {

        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String BlogID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Pic { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Url { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 OrderWeight { get; set; }

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
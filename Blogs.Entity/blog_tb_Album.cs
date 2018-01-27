using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using FYJ;

namespace Blogs.Entity
{

    public class blog_tb_Album 
    {

        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String UserID { get; set; }

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
        public Int32 Permission { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime ADD_DATE { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime UPDATE_DATE { get; set; }

        /// <summary>
        /// 封面
        /// </summary>
        public string CoverUrl{ get;set;}

        /// <summary>
        /// 照片数
        /// </summary>
        [Ignore]
        public int PhotoCount { get; set; }

        [Ignore]
        public List<string> Photos { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_Board 
    {

        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String PID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String UserID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Author { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Email { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String IP { get; set; }

        ///<summary>
        ///留言内容
        ///</summary>
        public String Mark { get; set; }

        ///<summary>
        ///状态 0-未处理 1-已通过 2-已拒绝
        ///</summary>
        public Int32 State { get; set; }

        ///<summary>
        ///支持数
        ///</summary>
        public Int32 SupportCount { get; set; }

        ///<summary>
        ///反对数
        ///</summary>
        public Int32 AgainstCount { get; set; }

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
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_Talk 
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
        public String TalkContent { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String TalkText { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Pic { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 是否临时
        /// </summary>
        public bool IsTemp { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public DateTime TalkDatetime { get; set; }

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
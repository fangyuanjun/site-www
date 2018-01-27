using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_Exif 
    {

        ///<summary>
        /// 
        ///</summary>
        public String ID { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String PhotoID { get; set; }

        ///<summary>
        ///相机
        ///</summary>
        public String Camera { get; set; }

        ///<summary>
        ///镜头
        ///</summary>
        public String Lens { get; set; }

        ///<summary>
        ///焦距
        ///</summary>
        public Decimal Focus { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 ISO { get; set; }

        ///<summary>
        ///快门
        ///</summary>
        public String Shutter { get; set; }

        ///<summary>
        ///光圈
        ///</summary>
        public Decimal Aperture { get; set; }

        ///<summary>
        ///曝光补偿
        ///</summary>
        public String Exposure { get; set; }

        ///<summary>
        ///测光模式
        ///</summary>
        public String Metering { get; set; }

        ///<summary>
        ///曝光模式
        ///</summary>
        public String Mode { get; set; }

        ///<summary>
        ///白平衡
        ///</summary>
        public String Balance { get; set; }

        ///<summary>
        ///闪光灯
        ///</summary>
        public String Flashlight { get; set; }

        ///<summary>
        /// 拍摄时间
        ///</summary>
        public DateTime ShotDate { get; set; }

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
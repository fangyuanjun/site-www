using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Blogs.Entity
{

    public class blog_tb_Photo 
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
        ///相册ID
        ///</summary>
        public String AlbumID { get; set; }

        ///<summary>
        ///文件名
        ///</summary>
        public String FileName { get; set; }

        ///<summary>
        ///显示名
        ///</summary>
        public String Display { get; set; }

        ///<summary>
        ///大小
        ///</summary>
        public Int32 Size { get; set; }

        ///<summary>
        ///
        ///</summary>
        public String Url { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 Width { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Int32 Height { get; set; }

        ///<summary>
        ///缩略图地址
        ///</summary>
        public String ThumbUrl { get; set; }

        ///<summary>
        ///权限
        ///</summary>
        public Int32 Permission { get; set; }

        ///<summary>
        ///作者
        ///</summary>
        public String Author { get; set; }

        ///<summary>
        ///备注
        ///</summary>
        public String Mark { get; set; }

        ///<summary>
        ///来源
        ///</summary>
        public String FromUrl { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean IsToAliyun { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean IsToLocal { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean IsToRemote { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public Boolean IsDelete { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Exif { get; set; }

        ///<summary>
        /// 
        ///</summary>
        public String Sha1 { get; set; }

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
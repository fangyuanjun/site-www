using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class blog_attachment
    {
        public string fileID { get; set; }

        public string relationID { get; set; }

        public string fileName { get; set; }

        public string objectTag { get; set; }

        private string _fileUrl;
        public string fileUrl
        {
            get
            {   
                if (objectTag== "attachment")
                {
                    return "http://upload.kecq.com/file/download?key=" + DownloadKey;
                }

                return _fileUrl;
            }
            set
            {
                _fileUrl = value;
            }
        }

        public string fileThumbUrl { get; set; }

        public string Exif { get; set; }

        public long fileSize { get; set; }


        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(fileName))
                {
                    return Path.GetFileName(fileUrl);
                }

                return fileName;
            }
        }

       

        public string SizeString { get { return SizeHelper.ToSizeString(fileSize); } }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int DownloadCount { get; set; }

        public string DownloadKey { get; set; }


        public string icon { get; set; }
    }
}

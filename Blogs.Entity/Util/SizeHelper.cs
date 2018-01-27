using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
   public class SizeHelper
    {
        public static string ToSizeString(long size)
        {
            if (size > 0 && size < 1024)
            {
                return size + "B";
            }

            if (size >= 1024 && size < 1048576)
            {
                return Math.Round(size / 1024.0, 2) + "KB";
            }

            if (size >= 1048576 && size < 1048576 * 1024)
            {
                return Math.Round(size / 1048576.0, 2) + "MB";
            }

            if (size >= 1073741824)
            {
                return Math.Round(size / 1073741824.0, 2) + "GB";
            }

            return size + "";
        }
    }
}

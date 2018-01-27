using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALPhoto 
    {
        List<blog_tb_Photo> Query(string albumID);
    }
}

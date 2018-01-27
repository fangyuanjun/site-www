using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLPhoto : IBLLPhoto
    {
        public List<blog_tb_Photo> Query(string albumID)
        {
            return IocFactory<IDALPhoto>.Instance.Query(albumID);
        }
    }
}

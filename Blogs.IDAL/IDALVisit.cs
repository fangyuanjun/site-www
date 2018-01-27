using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALVisit
    {
        int AddVisit(blog_tb_Visit entity);

    }
}

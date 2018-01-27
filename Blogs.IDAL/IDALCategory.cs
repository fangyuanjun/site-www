
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;


namespace Blogs.IDAL
{
    public interface IDALCategory 
    {
        blog_tb_category GetEntity(string id);
    }
}

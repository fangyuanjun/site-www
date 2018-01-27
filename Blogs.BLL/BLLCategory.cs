
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;
using FYJ;
using Blogs.IBLL;
using Blogs.IDAL;

namespace Blogs.BLL
{
    public class BLLCategory : IBLLCategory
    {
        public blog_tb_category GetEntity(string id)
        {
            return IocFactory<IDALCategory>.Instance.GetEntity(id);
        }
    }
}

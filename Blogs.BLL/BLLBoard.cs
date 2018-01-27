using Blogs.IBLL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Blogs.Entity;
using Blogs.IDAL;

namespace Blogs.BLL
{
    public class BLLBoard :  IBLLBoard
    {
        public int Delete(string id)
        {
            return IocFactory<IDALBoard>.Instance.Delete(id);
        }

        public blog_tb_Board GetEntity(string id)
        {
            return IocFactory<IDALBoard>.Instance.GetEntity(id);
        }

        public int Insert(blog_tb_Board entity)
        {
            return IocFactory<IDALBoard>.Instance.Insert(entity);
        }

        public List<blog_tb_Board> Query(int state)
        {
            return IocFactory<IDALBoard>.Instance.Query(state);
        }

        public int Update(blog_tb_Board entity)
        {
            return IocFactory<IDALBoard>.Instance.Update(entity);
        }
    }
}

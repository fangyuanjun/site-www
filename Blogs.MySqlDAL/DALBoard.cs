using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using FYJ.Data.Entity;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class DALBoard : IDALBoard
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public int Delete(string id)
        {
            return SqlCommon.Delete(DbInstance, "blog_tb_Board", "ID", id);
        }

        public blog_tb_Board GetEntity(string id)
        {
            return EntityHelper<blog_tb_Board>.GetEntity("blog_tb_Board", "ID", id, DbInstance);
        }

        public int Insert(blog_tb_Board entity)
        {
            return EntityHelper<blog_tb_Board>.Insert(entity, "blog_tb_Board", "ID", true, DbInstance);
        }

        public List<blog_tb_Board> Query(int state)
        {
            string sql = "select * from blog_tb_Board where 1=1";
            if (state != -1)
            {
                sql += " and state=" + state;
            }

            DataTable dt = DbInstance.GetDataTable(sql);
            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Board>(dt);
        }

        public int Update(blog_tb_Board entity)
        {
            return EntityHelper<blog_tb_Board>.Update(entity, "blog_tb_Board", DbInstance, "ID");
        }
    }
}

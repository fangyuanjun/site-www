using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class DALSlider :  IDALSlider
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public List<Entity.blog_tb_slider> Query(string blogID)
        {
            string sql = "select   *  from blog_tb_slider where BlogID=@BlogID   order by OrderWeight DESC limit 0,5";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@BlogID", blogID));
            return ObjectHelper.DataTableToModel<blog_tb_slider>(dt);
        }
    }
}

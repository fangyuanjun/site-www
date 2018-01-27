using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class DALPhoto : IDALPhoto
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public List<blog_tb_Photo> Query(string albumID)
        {
            string sql = "select * from blog_tb_Photo where AlbumID=@AlbumID and IsDelete=0";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@AlbumID", albumID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Photo>(dt);
        }
    }
}

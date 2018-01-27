using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using FYJ.Data.Entity;

namespace Blogs.DAL
{
    public class DALCategory : IDALCategory
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public blog_tb_category GetEntity(string id)
        {
            return EntityHelper<blog_tb_category>.GetEntity("blog_tb_category", "categoryID", id, DbInstance);
        }
    }
}


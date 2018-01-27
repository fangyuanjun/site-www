using Blogs.Entity;
using Blogs.IDAL;
using FYJ;

namespace Blogs.BLL
{
    public class BLLVisit: IBLL.IBLLVisit
    {

        public int AddVisit(blog_tb_Visit entity)
        {
            return IocFactory<IDALVisit>.Instance.AddVisit(entity);
        }

    }
}

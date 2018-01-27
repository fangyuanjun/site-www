using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using System.Collections.Generic;

namespace Blogs.BLL
{
    public class BLLAlbum : IBLLAlbum
    {
        public List<blog_tb_Album> QueryAlbum(string userID)
        {
            return IocFactory<IDALAlbum>.Instance.QueryAlbum(userID);
        }

        public blog_tb_Album GetEntity(string id)
        {
            return IocFactory<IDALAlbum>.Instance.GetEntity(id);
        }
    }
}

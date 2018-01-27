using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using System.Collections.Generic;

namespace Blogs.BLL
{
    public class BLLTalk :  IBLL.IBLLTalk
    {

        public List<blog_tb_Talk> Query(string userID)
        {
            return IocFactory<IDALTalk>.Instance.Query(userID);
        }
    }
}

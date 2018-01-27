using Blogs.Entity;
using System.Collections.Generic;

namespace Blogs.IBLL
{
    public interface IBLLTalk 
    {
       
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<blog_tb_Talk> Query(string userID);
    }
}

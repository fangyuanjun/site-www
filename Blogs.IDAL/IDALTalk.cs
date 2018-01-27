using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALTalk 
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        List<blog_tb_Talk> Query(string userID);
    }
}

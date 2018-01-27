using Blogs.Entity;
using System.Collections.Generic;
using System.Data;

namespace Blogs.IBLL
{
    public interface IBLLPhoto 
    {
        List<blog_tb_Photo> Query(string albumID);
    }
}

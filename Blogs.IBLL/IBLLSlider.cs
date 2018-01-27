using Blogs.Entity;
using System.Collections.Generic;

namespace Blogs.IBLL
{
    public interface IBLLSlider 
    {
        List<blog_tb_slider> Query(string blogID);

    }
}

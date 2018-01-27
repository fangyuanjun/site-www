using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALSlider 
    {
        List<blog_tb_slider> Query(string blogID);
    }
}

using Blogs.Entity;
using Blogs.IDAL;
using System;
using FYJ;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.BLL
{
    public class BLLSlider :  IBLL.IBLLSlider
    {

        public List<blog_tb_slider> Query(string blogID)
        {
            return IocFactory<IDALSlider>.Instance.Query(blogID);
        }
    }
}

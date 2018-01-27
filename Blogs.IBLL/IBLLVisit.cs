using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.IBLL
{
    public interface IBLLVisit 
    {
        int AddVisit(Entity.blog_tb_Visit entity);
    }
}

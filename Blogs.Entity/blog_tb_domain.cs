using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blogs.Entity
{
    public class blog_tb_domain 
    {
        public string ID { get; set; }

        public int blogID { get; set; }

        public string domain { get; set; }

        public int port { get; set; }

        public DateTime ADD_DATE { get; set; }

        public DateTime UPDATE_DATE { get; set; }
    }
}

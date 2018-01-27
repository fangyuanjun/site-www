using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.Entity
{
    public class CommentState 
    {
        public string typeID { get; set; }

        public string typeName { get; set; }

        public string typePicUrl { get; set; }

        public int stateCount { get; set; }
    }
}
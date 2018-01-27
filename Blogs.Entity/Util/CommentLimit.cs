using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Entity
{
    [Flags]
    public enum CommentLimit : int
    {
        未设置 = 0x00,
        禁止匿名用户回复 = 0x01,
        需要审核 = 0x02,
        禁止回复 = 0x04
    }
}

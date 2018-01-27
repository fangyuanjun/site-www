using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blogs.Entity
{
    [Flags]
    public enum AttachmentLimit
    {
        [Description("未设置")]
        未设置 = 1 << 0,//1

        [Description("禁止未登录用户下载")]
        禁止未登录用户下载 = 1 << 1,//2

        [Description("禁止未回复用户下载")]
        禁止未回复用户下载 = 1 << 2,//4

        [Description("禁止非管理组下载")]
        禁止非管理组下载 = 1 << 3,//8

        [Description("禁止下载")]
        禁止下载 = 1 << 4,//16
    }
}

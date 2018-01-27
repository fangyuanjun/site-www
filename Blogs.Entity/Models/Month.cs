using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogs.Entity
{
    /// <summary>
    /// 文章档案模型
    /// </summary>
    public class Month
    {

        /// <summary>
        /// 拥有的文章总数
        /// </summary>
        public int ArticleCount
        {
            get;
            set;
        }


        /// <summary>
        /// 归档Url  不受设置Url属性的影响
        /// </summary>
        /// RIGHT('0'+CAST(@mm AS nvarchar(2)),2)
        public string Url
        {
            get
            {
                return FYJ.IocFactory<IBlogFix>.Instance.GetMonthUrl(yyyy, mm);
            }
        }

        public string yyyy { get; set; }
        public string mm { get; set; }

        public string Title
        {
            get
            {
                return yyyy + "年" + mm + "月 (" + ArticleCount + ")";
            }
        }
    }
}
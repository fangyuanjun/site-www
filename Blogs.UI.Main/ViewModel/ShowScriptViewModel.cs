using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Blogs.UI.Main.ViewModel
{
   public class ShowScriptViewModel
    {
        public string articleID { get; set; }

        public string PassportRootUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["PassportRootUrl"];
            }
        }
    }
}

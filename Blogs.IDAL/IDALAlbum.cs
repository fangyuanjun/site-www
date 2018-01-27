using Blogs.Entity;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALAlbum 
    {
        blog_tb_Album GetEntity(string id);

        List<blog_tb_Album> QueryAlbum(string userID);

    }
}

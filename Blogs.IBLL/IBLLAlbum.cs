using Blogs.Entity;
using System.Collections.Generic;
using System.Data;

namespace Blogs.IBLL
{
    public interface IBLLAlbum 
    {
        blog_tb_Album GetEntity(string id);

        List<blog_tb_Album> QueryAlbum(string userID);

    }
}

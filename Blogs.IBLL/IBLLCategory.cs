using Blogs.Entity;


namespace Blogs.IBLL
{
    public interface IBLLCategory 
    {
        blog_tb_category GetEntity(string id);
    }
}

using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;

namespace Blogs.BLL
{
    public class BLLBlog : IBLLBlog
    {
        private IDAL.IDALBlog Dal
        {
            get { return IocFactory<IDALBlog>.Instance; }
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public blog_tb_blog GetEntity(string id)
        {
            return Dal.GetEntity(id);
        }

        /// <summary>
        /// 获取数据库第一条博客
        /// </summary>
        /// <returns></returns>
        /// fangyj 2015-6-22
        public blog_tb_blog GetFirstEntity()
        {
            return Dal.GetFirstEntity();
        }


        /// <summary>
        /// 获取用户的一个博客  (第一个)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>2015-07-25</returns>
        public blog_tb_blog GetSingleBlogByUserID(string userID)
        {
            return Dal.GetSingleBlogByUserID(userID);
        }


        /// <summary>
        /// 获取博客母板页数据
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public SharedModel GetBlogSharedData(string blogID)
        {
            return Dal.GetBlogSharedData(blogID);
        }

        /// <summary>
        /// 获取博客主页数据
        /// </summary>
        /// <param name="blogID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryID"></param>
        /// <param name="tagID"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public IndexModel GetBlogIndexData(string blogID, int page = 1, int pageSize = 10, string categoryID = null, string tagID = null, int year = 0, int month = 0)
        {
            return Dal.GetBlogIndexData(blogID,page,pageSize,categoryID,tagID,year,month);
        }

        /// <summary>
        /// 获取博客Rss源
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public List<blog_tb_article> GetBlogRss(string blogID)
        {
            return Dal.GetBlogRss(blogID);
        }

        /// <summary>
        /// 获取主页轮播图片
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public List<ArticlePic> GetMainArticlePic(string blogID)
        {
            return Dal.GetMainArticlePic(blogID);
        }

        public int Save(blog_tb_blog entity)
        {
            return Dal.Save(entity);
        }


        public List<sitemap> GetSitemap(string blogID)
        {
            return Dal.GetSitemap(blogID,false);
        }

        public List<sitemap> GetSitemapWithHide(string blogID)
        {
            return Dal.GetSitemap(blogID,true);
        }

        public bool CheckID(string id, string userID)
        {
            List<string> list = Dal.CheckID(userID);
            string[] ids = id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string t in ids)
            {
                if (!list.Contains(t))
                {
                    return false;
                }
            }

            return true;
        }


        public blog_tb_blog GetSingleBlogByDomain(string domain,int port)
        {
            return Dal.GetSingleBlogByDomain(domain,port);
        }


        public List<blog_tb_SiteCategory> QuerySiteCategory()
        {
            return Dal.QuerySiteCategory();
        }

        /// <summary>
        /// 更新统计信息
        /// </summary>
        /// <param name="blogID"></param>
        /// <param name="ip"></param>
        /// <param name="sessionID"></param>
        /// <returns></returns>
        /// 2015-8-13
        public int UpdateBlogCount(string blogID, string ip, string sessionID)
        {
            //本地不统计
            if (ip == "127.0.0.1")
            {
                return 0;
            }

            Dal.SaveBlogCount(blogID);

            return 1;
        }

        public int GetTotalPV(string blogID)
        {
           return Dal.GetTotalPV(blogID);
        }

        public bool IsExistsBlogName(string name)
        {
            return Dal.IsExistsBlogName(name);
        }

        public bool IsBlogDisabled(string blogID)
        {
            return Dal.IsBlogDisabled(blogID);
        }

        public bool AddDomain(string blogID, string domain, int port)
        {
            blog_tb_domain entity = Dal.QueryDomain(blogID,domain,port);
            if(entity!=null)
            {
                throw new CustomException("该绑定已存在");
            }

            entity  = new blog_tb_domain();
            entity.ID = Guid.NewGuid().ToString("N");
            entity.blogID =Convert.ToInt32(blogID);
            entity.ADD_DATE = DateTime.Now;
            entity.UPDATE_DATE = DateTime.Now;
            entity.domain = domain;
            entity.port = port;
            return Dal.AddDomain(entity)>0;
        }

        public bool UpdateDomain(string ID, string domain, int port)
        {
            if (domain == null)
            {
                domain = "";
            }
            else
            {
                domain = domain.Trim();
            }

            blog_tb_domain entity = Dal.GetDomainEntity(ID);
            if (entity == null)
            {
                throw new CustomException("绑定不存在");
            }

            blog_tb_domain entity2 = Dal.QueryDomain(entity.blogID+"", domain, port);
            if (entity2 != null && entity2.ID!=entity.ID)
            {
                throw new CustomException("绑定已存在");
            }

            entity.domain = domain;
            entity.port = port;
            entity.UPDATE_DATE = DateTime.Now;

            return Dal.UpdateDomain(entity) > 0;
        }

        public bool DeleteDomain(string ID)
        {
            return Dal.DeleteSingleDomain(ID)>0;
        }

        public List<blog_tb_domain> GetDomainList(string blogID)
        {
            return Dal.GetDomainList(blogID);
        }
    }
}

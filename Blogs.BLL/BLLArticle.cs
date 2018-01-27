using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using System;
using System.Collections.Generic;
using System.Data;


namespace Blogs.BLL
{
    public class BLLArticle : IBLLArticle
    {

        private IDALArticle Dal
        {
            get { return IocFactory<IDALArticle>.Instance; }
        }

       
        /// <summary>
        /// 获取个人博客文章列表
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public List<blog_tb_article> GetArticleList(string blogID)
        {
            return Dal.GetArticleList(blogID);
        }

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <param name="ip">访问IP</param>
        /// <param name="userID">访问用户ID</param>
        /// <returns></returns>
        public ArticleModel GetArticleByID(string articleID, string ip, string userID = null)
        {
            return Dal.GetArticleByID(articleID, ip, userID);
        }

  

        /// <summary>
        /// 更新Extend表  主要是访问次数
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="lastOpenIP"></param>
        /// <param name="lastOpenUserID"></param>
        /// <returns></returns>
        public int UpdateExtend(string articleID, string lastOpenIP, string lastOpenUserID)
        {
            return Dal.UpdateExtend(articleID, lastOpenIP, lastOpenUserID);
        }

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        public blog_tb_article GetEntity(string articleID)
        {
            return Dal.GetEntity(articleID);
        }

     

        public string GetarticleUserID(string articleID)
        {
            return Dal.GetarticleUserID(articleID);
        }



        public blog_tb_article_content GetArticleContent(string articleID)
        {
            return Dal.GetArticleContent(articleID);
        }

        public IEnumerable<blog_tb_tag> GetArticleTags(string articleID)
        {
            return Dal.GetArticleTags(articleID);
        }


        public List<blog_attachment> GetFileRelation(string objectID, string objectTag)
        {
            return Dal.GetFileRelation(objectID, objectTag);
        }

        public List<blog_attachment> GetArticlePhotos(string articleID)
        {
            return Dal.GetArticlePhotos(articleID);
        }

        public int GetArticleCommentLimit(string articleID)
        {
            return Dal.GetArticleCommentLimit(articleID);
        }

        public bool IsArticleDisabled(string articleID)
        {
            return Dal.IsArticleDisabled(articleID);
        }

        public string GetArticlePassword(string articleID)
        {
            return Dal.GetArticlePassword(articleID);
        }

        public string GetArticleThemeID(string articleID)
        {
            return Dal.GetArticleThemeID(articleID);
        }
    }
}

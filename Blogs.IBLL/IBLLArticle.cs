using Blogs.Entity;
using System.Collections.Generic;
using System.Data;


namespace Blogs.IBLL
{
    public interface IBLLArticle 
    {

        /// <summary>
        /// 获取个人博客文章列表
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        List<blog_tb_article> GetArticleList(string blogID);

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="articleID">文章ID</param>
        /// <param name="ip">访问IP</param>
        /// <param name="userID">访问用户ID</param>
        /// <returns></returns>
        ArticleModel GetArticleByID(string articleID, string ip, string userID = null);


        /// <summary>
        /// 更新Extend表  主要是访问次数
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="lastOpenIP"></param>
        /// <param name="lastOpenUserID"></param>
        /// <returns></returns>
        int UpdateExtend(string articleID, string lastOpenIP, string lastOpenUserID);

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        blog_tb_article GetEntity(string articleID);


        /// <summary>
        /// 获取文章用户ID
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        string GetarticleUserID(string articleID);

       
        /// <summary>
        /// 获取文章正文
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
        blog_tb_article_content GetArticleContent(string articleID);

        /// <summary>
        /// 获取文章标签
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// fangyj 2015-5-8
        IEnumerable<blog_tb_tag> GetArticleTags(string articleID);

        /// <summary>
        /// 获取文件关系
        /// </summary>
        /// <param name="objectID"></param>
        /// <param name="objectTag"></param>
        /// <returns></returns>
        List<blog_attachment> GetFileRelation(string objectID, string objectTag);


        /// <summary>
        /// 获取文章的图片
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        /// 2015-6-16
        List<blog_attachment> GetArticlePhotos(string articleID);

        /// <summary>
        /// 获取评论限制
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        int GetArticleCommentLimit(string articleID);

        /// <summary>
        /// 获取文章是否被禁用
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        bool IsArticleDisabled(string articleID);

        /// <summary>
        /// 获取文章密码
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        string GetArticlePassword(string articleID);

        /// <summary>
        /// 获取文章的主题ID
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        string GetArticleThemeID(string articleID);
    }
}

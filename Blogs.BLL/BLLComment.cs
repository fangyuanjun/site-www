using Blogs.Entity;
using Blogs.IBLL;
using Blogs.IDAL;
using FYJ;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Blogs.BLL
{
    public class BLLComment : IBLLComment
    {
        private IDAL.IDALComment Dal
        {
            get { return IocFactory<IDALComment>.Instance; }
        }

        public  int Insert(Blogs.Entity.blog_tb_comment entity)
        {
            IocFactory<IDALArticle>.Instance.UpdateComment(entity.articleID + "", 1);
            return Dal.Insert(entity);
        }
        /// <summary>
        /// 获取未读取的回复条数
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public int GetNoReadCommentCount(string blogID)
        {
            return Dal.GetNoReadCommentCount(blogID);
        }

        /// <summary>
        /// 状态投票
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="typeID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int Vote(string articleID, string typeID, string userID, string ip)
        {
            return Dal.Vote(articleID, typeID, userID, ip);
        }

        /// <summary>
        /// 支持
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int Support(string commentID, string userID, string ip)
        {
            return Dal.Support(commentID, userID, ip);
        }

        /// <summary>
        /// 反对
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public int Against(string commentID, string userID, string ip)
        {
            return Dal.Against(commentID, userID, ip);
        }

        /// <summary>
        /// 获取分页的评论
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public CommentModel GetCommentPage(string articleID, int page, int pageSize)
        {
            return Dal.GetCommentPage(articleID, page, pageSize);
        }

        /// <summary>
        /// 验证评论限制  如果验证不通过抛出自定义异常
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="userID"></param>
        public void ValidateCommentLimit(string articleID, string userID)
        {
            Dal.ValidateCommentLimit(articleID, userID);
        }

        /// <summary>
        /// 是否禁止评论
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        public bool isDisableComment(int articleCommentLimit)
        {
            return Dal.isDisableComment(articleCommentLimit);
        }

        /// <summary>
        /// 评论是否需审核
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        public bool isVerifyComment(int articleCommentLimit)
        {
            return Dal.isVerifyComment(articleCommentLimit);
        }

        /// <summary>
        /// 是否允许匿名评论
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        public bool isDisabledAnonymousComment(int articleCommentLimit)
        {
            return Dal.isDisabledAnonymousComment(articleCommentLimit);
        }

        public int GetMaxFloor(string articleID)
        {
            return Dal.GetMaxFloor(articleID);
        }



        public bool IsReplyed(string articleID, string userID)
        {
            return Dal.IsReplyed(articleID, userID);
        }

        public bool IsAllowCommentContent(string commentContent)
        {
            //<img onerror=alert(1)

            List<string> htmltaglist = new List<string>() { "html", "script", "link", "img" };
            foreach (string s in htmltaglist)
            {
                if (Regex.IsMatch(commentContent, "<\\s*?" + s, RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            List<string> sqlwordlist = new List<string>() { "select", "update", "delete", "drop", "html", "script", "create", "table" };
            foreach (string s in sqlwordlist)
            {
                if (Regex.IsMatch(commentContent, s, RegexOptions.IgnoreCase))
                {
                    return false;
                }
            }

            return true;
        }

        public blog_tb_comment GetEntity(string id)
        {
            return Dal.GetEntity(id);
        }

        public int Update(blog_tb_comment entity)
        {
            return Dal.Update(entity);
        }

        public int Delete(string id)
        {
            return Dal.Delete(id);
        }
    }
}

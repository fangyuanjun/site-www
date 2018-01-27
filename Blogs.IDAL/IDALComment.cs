
using System;
using System.Collections.Generic;
using System.Data;
using Blogs.Entity;

namespace Blogs.IDAL
{
    public interface IDALComment 
    {
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        blog_tb_comment GetEntity(string id);

        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(blog_tb_comment entity);

        /// <summary>
        /// 更新Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(blog_tb_comment entity);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(string id);


        /// <summary>
        /// 获取未读取的回复条数
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        int GetNoReadCommentCount(string blogID);

        /// <summary>
        /// 状态投票
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="typeID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        int Vote(string articleID, string typeID, string userID, string ip);

        /// <summary>
        /// 支持
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        int Support(string commentID, string userID, string ip);

        /// <summary>
        /// 反对
        /// </summary>
        /// <param name="commentID"></param>
        /// <param name="userID"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        int Against(string commentID, string userID, string ip);

        /// <summary>
        /// 获取分页的评论
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        CommentModel GetCommentPage(string articleID, int page, int pageSize);

        /// <summary>
        /// 验证评论限制  如果验证不通过抛出自定义异常
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="userID"></param>
        void ValidateCommentLimit(string articleID, string userID);

        /// <summary>
        /// 是否禁止评论
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        bool isDisableComment(int articleCommentLimit);

        /// <summary>
        /// 评论是否需审核
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        bool isVerifyComment(int articleCommentLimit);

        /// <summary>
        /// 是否允许匿名评论
        /// </summary>
        /// <param name="articleCommentLimit"></param>
        /// <returns></returns>
        bool isDisabledAnonymousComment(int articleCommentLimit);

        /// <summary>
        /// 获取最大楼层
        /// </summary>
        /// <param name="articleID"></param>
        /// <returns></returns>
        int GetMaxFloor(string articleID);

        /// <summary>
        /// 是否回复
        /// </summary>
        /// <param name="articleID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        bool IsReplyed(string articleID, string userID);
    }
}

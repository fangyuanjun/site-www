using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using FYJ.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class DALComment : IDALComment
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public  int Insert(blog_tb_comment entity)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Comment"); 
            entity.commentID = Guid.NewGuid().ToString("N");
            entity.ADD_DATE = DateTime.Now;
            entity.UPDATE_DATE = DateTime.Now;

           return FYJ.Data.Entity.EntityHelper<blog_tb_comment>.Insert(entity, "blog_tb_comment", "commentID", true, db);
        }

        public  int Delete(string commentID)
        {
            try
            {
                DbInstance.BeginTran();
                string sql = "select articleID,articleCommentTimes from blog_view_article where articleID=(select articleID from blog_tb_comment where commentID=@commentID)";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@commentID", commentID));
                if (dt.Rows.Count > 0)
                {
                    int articleCommentTimes = Convert.ToInt32(dt.Rows[0]["articleCommentTimes"]);
                    articleCommentTimes = Math.Max(articleCommentTimes - 1, 0);
                    sql = "update blog_tb_article_extend  set articleCommentTimes=" + articleCommentTimes + " where articleID=@articleID";
                    DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@articleID", dt.Rows[0]["articleID"]));
                }

                DbInstance.ExecuteSql("delete from blog_tb_comment where commentID=@commentID",DbInstance.CreateParameter("@commentID", commentID));
                DbInstance.Commit();

                return 1;
            }
            catch
            {
                DbInstance.Rollback();
                throw;
            }
        }

        public int GetNoReadCommentCount(string blogID)
        {
            string sql = @"select COUNT(*) from blog_tb_comment 
                            inner join blog_view_article on blog_view_article.articleID=blog_tb_comment.articleID 
                            left join blog_tb_article_extend on blog_tb_article_extend.articleID=blog_view_article.articleID
                            where  blogID=@blogID
                            and blog_tb_comment.ADD_DATE>=blog_tb_article_extend.lastOpenDatetime";
            int count = DbInstance.GetInt(sql, DbInstance.CreateParameter("@blogID", blogID));

            return count;
        }

        public int Vote(string articleID, string typeID, string userID, string ip)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Comment");
            string stateID = String.Empty;
            //如果评论状态表中已存在该文章的该状态
            string sql = "select * from blog_tb_commentState where articleID=@articleID and typeID=typeID";
            DataTable dt = db.GetDataTable(sql
                , db.CreateParameter("@articleID", articleID)
                , db.CreateParameter("@typeID", typeID));
            if (dt.Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(userID))
                {
                    if (db.Exists("select 1 from blog_tb_commentStateSupport where stateID=@stateID and userID=@userID"
                     , db.CreateParameter("@stateID", dt.Rows[0]["stateID"])
                     , db.CreateParameter("@userID", userID)))
                    {
                        throw new CustomException("你已经支持过了");
                    }
                }

                if (db.Exists("select 1 from blog_tb_commentStateSupport where stateID=@stateID and supportIP=@supportIP"
                    , db.CreateParameter("@stateID", dt.Rows[0]["stateID"])
                    , db.CreateParameter("@supportIP", ip)))
                {
                    throw new CustomException("你已经支持过了");
                }

                stateID = dt.Rows[0]["stateID"].ToString();
                db.ExecuteSql("update blog_tb_commentState set stateCount=IFNULL(stateCount,0)+1,UPDATE_DATE=getdate() where stateID=@stateID"
                    , db.CreateParameter("@stateID", dt.Rows[0]["stateID"]));
            }
            else
            {
                stateID = Guid.NewGuid().ToString("N");
                blog_tb_commentState state = new blog_tb_commentState { stateID = stateID, articleID = Convert.ToInt32(articleID), typeID = typeID, stateCount = 1, ADD_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now };
                FYJ.Data.Entity.EntityHelper<blog_tb_commentState>.Insert(state, "blog_tb_commentState", "stateID", true, db);
            }

            blog_tb_commentStateSupport stateSupport = new blog_tb_commentStateSupport { supportID = Guid.NewGuid().ToString("N"), stateID = stateID, supportDatetime = DateTime.Now, supportIP = ip, userID = userID, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentStateSupport>.Insert(stateSupport, "blog_tb_commentStateSupport", "supportID", true, db);

            return 1;
        }


        public int Support(string commentID, string userID, string ip)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Comment"); 
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(userID))
            {
                sql = "select * from blog_tb_commentSupport where commentID=@commentID and userID=@UserID and isSupport=@isSupport";
                if (db.Exists(sql
                    , db.CreateParameter("@commentID", commentID)
                    , db.CreateParameter("@userID", userID)
                    , db.CreateParameter("@isSupport", true)
                    ))
                {
                    throw new CustomException("你已经支持过了");
                }
            }

            sql = "select * from blog_tb_commentSupport where commentID=@commentID and supportIP=@supportIP and isSupport=@isSupport";
            if (db.Exists(sql
                , db.CreateParameter("@commentID", commentID)
                , db.CreateParameter("@supportIP", ip)
                , db.CreateParameter("@isSupport", true)
                ))
            {
                throw new CustomException("你已经支持过了");
            }

            blog_tb_commentSupport support = new blog_tb_commentSupport { supportID = Guid.NewGuid().ToString("N"), supportIP = ip, commentID = commentID, supportDatetime = DateTime.Now, isSupport = true, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentSupport>.Insert(support, "blog_tb_commentSupport", "supportID", true, db);
            sql = "update blog_tb_comment set supportCount=IFNULL(supportCount,0)+1 where commentID=@commentID";
            return db.ExecuteSql(sql, this.DbInstance.CreateParameter("@commentID", commentID));
        }

        public int Against(string commentID, string userID, string ip)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Comment");
            string sql = string.Empty;
            if (!String.IsNullOrEmpty(userID))
            {
                sql = "select * from blog_tb_commentSupport where commentID=@commentID and userID=@UserID and isSupport=@isSupport";
                if (db.Exists(sql
                    , db.CreateParameter("@commentID", commentID)
                    , db.CreateParameter("@userID", userID)
                    , db.CreateParameter("@isSupport", false)
                    ))
                {
                    throw new CustomException("你已经反对过了");
                }
            }

            sql = "select * from blog_tb_commentSupport where commentID=@commentID and supportIP=@supportIP and isSupport=@isSupport";
            if (db.Exists(sql
                , db.CreateParameter("@commentID", commentID)
                , db.CreateParameter("@supportIP", ip)
                , db.CreateParameter("@isSupport", false)
                ))
            {
                throw new CustomException("你已经反对过了");
            }

            blog_tb_commentSupport support = new blog_tb_commentSupport { supportID = Guid.NewGuid().ToString("N"), supportIP = ip, commentID = commentID, supportDatetime = DateTime.Now, isSupport = false, UPDATE_DATE = DateTime.Now };
            FYJ.Data.Entity.EntityHelper<blog_tb_commentSupport>.Insert(support, "blog_tb_commentSupport", "supportID", true, db);
            sql = "update blog_tb_comment set againstCount=IFNULL(againstCount,0)+1 where commentID=@commentID";

            return db.ExecuteSql(sql, db.CreateParameter("@commentID", commentID));
        }

        public CommentModel GetCommentPage(string articleID, int page, int pageSize)
        {
            if(page==0)
            {
                page = 1;
            }

            string limit = ((page - 1) * pageSize) + "," + pageSize;

            string sql = @"
select blog_tb_commentType.typeID, typeName,typePicUrl,stateCount from (select stateCount,typeID from blog_tb_commentState where articleID=@articleID) table1
right join blog_tb_commentType on blog_tb_commentType.typeID=table1.typeID order by typeOrder 
;

select  commentID,commentText,supportCount from blog_tb_comment where articleID=@articleID 
and commentState=1  
and LENGTH(commentText)>5 and LENGTH(commentText)<30
order by supportCount desc,ADD_DATE DESC
limit 0,100
;

/*获取评论总数*/
select COUNT(*) from blog_tb_comment where articleID=@articleID
;


 select * from 
 (select blog_tb_comment.*,blog_tb_blog.userID as AuthorUserID,
 (case when blog_tb_blog.userID=blog_tb_comment.userID then '[楼主]' else '' end) as Author,ifnull(ChildCount,0) ChildCount
from blog_tb_comment 
inner join blog_view_article on blog_view_article.articleID=blog_tb_comment.articleID 
inner join blog_tb_blog on blog_tb_blog.blogID=blog_view_article.blogID  
left join(select parentID,COUNT(*) as ChildCount from blog_tb_comment group by parentID) as table1 on table1.parentID=blog_tb_comment.commentID
order by ADD_DATE asc
) table1 where articleID=@articleID    
limit  {0}
";
            sql = sql.Replace("{0}", limit);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@articleID", articleID);

            DataSet ds = this.DbInstance.GetDataSet(sql, dic);

            CommentModel model = new CommentModel();
            model.CommentStateCollection= FYJ.ObjectHelper.DataTableToModel<CommentState>(ds.Tables[0]);
            model.CommentTagCollection= FYJ.ObjectHelper.DataTableToModel<CommentTag>(ds.Tables[1]);
            model.RecordCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
            model.CommentCollection= FYJ.ObjectHelper.DataTableToModel<blog_tb_comment>(ds.Tables[3]);

            return model;
        }


        public void ValidateCommentLimit(string articleID, string userID)
        {
            return;
        }

        public bool isDisableComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            //bool test = (limit & CommentLimit.禁止回复) == CommentLimit.禁止回复;
            bool hasFlag = ((limit & CommentLimit.禁止回复) != 0);

            return hasFlag;
        }

        public bool isVerifyComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            bool hasFlag = ((limit & CommentLimit.需要审核) != 0);

            return hasFlag;
        }


        public bool isDisabledAnonymousComment(int articleCommentLimit)
        {
            CommentLimit limit = (CommentLimit)Enum.Parse(typeof(CommentLimit), articleCommentLimit + "");
            bool hasFlag = ((limit & CommentLimit.禁止匿名用户回复) != 0);

            return hasFlag;
        }

        public int GetMaxFloor(string articleID)
        {
            string sql = "select MAX(floor) from blog_tb_comment c left join blog_tb_article a on a.articleID=c.articleID where c.articleID=@articleID";
            return DbInstance.GetInt(sql, DbInstance.CreateParameter("@articleID", articleID));
        }


        public bool IsReplyed(string articleID, string userID)
        {
            string sql = "select 1 from blog_tb_comment where articleID=@articleID and userID=@userID";
            return DbInstance.Exists(sql, DbInstance.CreateParameter("@articleID", articleID)
                                        , DbInstance.CreateParameter("@userID", userID));
        }

        public blog_tb_comment GetEntity(string id)
        {
            return EntityHelper<blog_tb_comment>.GetEntity("blog_tb_comment", "commentID", id,DbInstance);
        }

        public int Update(blog_tb_comment entity)
        {
            return EntityHelper<blog_tb_comment>.Update(entity, "blog_tb_comment",DbInstance, "commentID");
        }
    }
}


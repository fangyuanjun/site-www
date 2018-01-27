
using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.DAL
{
    public class DALArticle : IDALArticle
    {

        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        /// <summary>
        /// 获取个人博客文章列表
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public List<blog_tb_article> GetArticleList(string blogID)
        {
            string sql = @"
            select  blog_tb_article.articleID,articleTitle,articleDatetime,articleClickTimes,articleCommentTimes 
            from blog_tb_article
            left join blog_tb_article_extend e on e.articleID=blog_tb_article.articleID
            LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
             where  blog_tb_article.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0 order by articleDatetime desc, articleIsTop DESC,articleOrder DESC,articleDatetime DESC
                ";
            DataTable dt = this.DbInstance.GetDataTable(sql, this.DbInstance.CreateParameter("@blogID", blogID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_article>(dt);
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
            string sql = "call blog_proc_singleArticle(@articleID,@ip,@userID)";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@articleID", articleID);
            dic.Add("@ip", ip);
            dic.Add("@userID", userID);
            DataSet ds = this.DbInstance.GetDataSet(sql, dic);

            ArticleModel model = new ArticleModel();

            model.CurrentArticle = FYJ.ObjectHelper.DataTableToSingleModel<ArticleShow>(ds.Tables[0]);
            model.CommentCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
            model.CommentStateCollection = ObjectHelper.DataTableToModel<CommentState>(ds.Tables[2]);
            model.CommentTagCollection = ObjectHelper.DataTableToModel<CommentTag>(ds.Tables[3]);

            model.BeforeArticle = new ArticleLink();
            model.AfterArticle = new ArticleLink();
            if (ds.Tables[4].Rows.Count > 0)
            {
                model.BeforeArticle = FYJ.ObjectHelper.DataTableToSingleModel<ArticleLink>(ds.Tables[4]); 
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                model.AfterArticle = FYJ.ObjectHelper.DataTableToSingleModel<ArticleLink>(ds.Tables[5]);
            }

            model.CurrentArticle.TagCollection = FYJ.ObjectHelper.DataTableToModel<blog_tb_tag>(ds.Tables[6]);
            model.CommentCollection = FYJ.ObjectHelper.DataTableToModel<blog_tb_comment>(ds.Tables[7]);

            return model;
        }


        public int UpdateExtend(string articleID, string lastOpenIP, string lastOpenUserID)
        {
            string sql = "select * from blog_tb_article_extend where articleID=@articleID";
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            if (db.Exists(sql, db.CreateParameter("@articleID", articleID)))
            {
                sql = " update blog_tb_article_extend set lastOpenDatetime=@UPDATE_DATE,lastOpenIP=@ip,lastOpenUserID=@userID,UPDATE_DATE=@UPDATE_DATE,articleClickTimes=ifnull(articleClickTimes,0)+1 where articleID=@articleID";
                db.ExecuteSql(sql
                   , db.CreateParameter("@articleID", articleID)
                   , db.CreateParameter("@ip", lastOpenIP)
                   , db.CreateParameter("@userID", lastOpenUserID)
                   , db.CreateParameter("@UPDATE_DATE", DateTime.Now));
            }
            else
            {
                blog_tb_article_extend entity = new blog_tb_article_extend();
                entity.articleClickTimes = 1;
                entity.articleCommentTimes = 0;
                entity.articleID = Convert.ToInt32(articleID);
                entity.lastOpenDatetime = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                entity.lastOpenIP = lastOpenIP;
                entity.lastOpenUserID = lastOpenUserID;
                FYJ.Data.Entity.EntityHelper<blog_tb_article_extend>.Insert(entity, "blog_tb_article_extend", "articleID", true, db);
            }

            return 1;
        }


        public blog_tb_article GetEntity(string articleID)
        {
            DataTable dt = DbInstance.GetDataTable("select * from blog_tb_article where articleID=@articleID"
               , DbInstance.CreateParameter("@articleID", articleID));

            blog_tb_article model = ObjectHelper.DataTableToModel<blog_tb_article>(dt).FirstOrDefault();

            return model;
        }


        public string GetarticleUserID(string articleID)
        {
            string sql = "select userID from blog_tb_article a left join blog_tb_blog b on a.blogID=b.blogID where a.articleID=@articleID";
            return DbInstance.GetString(sql, DbInstance.CreateParameter("@articleID", articleID));
        }


      
        public blog_tb_article_content GetArticleContent(string articleID)
        {
            string sql = "select * from blog_tb_article_content where articleID=@articleID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            return ObjectHelper.DataTableToSingleModel<blog_tb_article_content>(dt);
        }

        public IEnumerable<blog_tb_tag> GetArticleTags(string articleID)
        {
            string sql = "select * from blog_tb_tag where tagID in ( select tagID from blog_tb_tagArticle where articleID=@articleID)";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            return ObjectHelper.DataTableToModel<blog_tb_tag>(dt);
        }


        public List<blog_attachment> GetFileRelation(string objectID, string objectTag)
        {
            // select fileUrl,[fileName],fileSize from db_common.dbo.blog_tb_fileRelation inner join  db_common.dbo.blog_tb_file on db_common.dbo.blog_tb_file.fileID=blog_tb_fileRelation.fileID
            //where objectID = @articleID and objectType = 'attachment'

            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Files");
            string sql = "select f.*,r.relationID,c.DownloadCount from blog_tb_fileRelation r left join blog_tb_file f on r.fileID=f.fileID  LEFT JOIN blog_tb_filecount c on c.FileID=f.fileID where objectID=@objectID and objectTag=@objectTag";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", objectID), db.CreateParameter("@objectTag", objectTag));
            return FYJ.ObjectHelper.DataTableToModel<blog_attachment>(dt);
        }

      
        public List<blog_attachment> GetArticlePhotos(string articleID)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Files");
            string sql = "select f.fileID,f.fileThumbUrl,f.fileUrl,f.fileName,f.Exif from blog_tb_fileRelation r inner join blog_tb_file  f on r.fileID=f.fileID where f.fileIsImage=1 and r.objectID=@objectID";
            DataTable dt = db.GetDataTable(sql, db.CreateParameter("@objectID", articleID));
            return FYJ.ObjectHelper.DataTableToModel<blog_attachment>(dt);
        }




        public int UpdateComment(string articleID, int add)
        {
            string addstr = (add >= 0 ? "+" + add : add.ToString());
            string sql = "update blog_tb_article_extend set articleCommentTimes=articleCommentTimes" + addstr + " where articleID=@articleID";
            return DbInstance.ExecuteSql(sql, DbInstance.CreateParameter("@articleID", articleID));
        }

        public int GetArticleCommentLimit(string articleID)
        {
            string sql = "select articleCommentLimit from blog_tb_article  where articleID=@articleID";
            return DbInstance.GetInt(sql, DbInstance.CreateParameter("@articleID", articleID));
        }

        public bool IsArticleDisabled(string articleID)
        {
            string sql = @"
                select articleIsDisabled,c.categoryIsDisabled,blogIsDisabled from blog_tb_article a 
                left join blog_tb_category c on a.categoryID=c.categoryID
                inner join blog_tb_blog b on b.blogID=a.blogID
                where a.articleID=@articleID
                ";

            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToBoolean(dt.Rows[0]["articleIsDisabled"]))
                {
                    return true;
                }

                if (Convert.ToBoolean(dt.Rows[0]["categoryIsDisabled"]))
                {
                    return true;
                }

                if (Convert.ToBoolean(dt.Rows[0]["blogIsDisabled"]))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetArticlePassword(string articleID)
        {
            return DbInstance.GetString("select articlePassword from blog_tb_article where articleID=@articleID", DbInstance.CreateParameter("@articleID", articleID));
        }

        public string GetArticleThemeID(string articleID)
        {
            string sql = @"
                   select a.themeID as articleThemeID,c.themeID as categoryIDThemeID,b.themeID as blogThemeID from blog_tb_article a 
                left join blog_tb_category c on a.categoryID=c.categoryID
                inner join blog_tb_blog b on b.blogID=a.blogID
                where a.articleID=@articleID
                ";

            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@articleID", articleID));
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["articleThemeID"].ToString() != "")
                {
                    return dt.Rows[0]["articleThemeID"].ToString();
                }

                if (dt.Rows[0]["categoryIDThemeID"].ToString() != "")
                {
                    return dt.Rows[0]["categoryIDThemeID"].ToString();
                }

                if (dt.Rows[0]["blogThemeID"].ToString() != "")
                {
                    return dt.Rows[0]["blogThemeID"].ToString();
                }
            }

            return null;
        }
    }
}


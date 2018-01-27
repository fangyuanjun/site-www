using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using FYJ.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Blogs.DAL
{
    public class DALBlog : IDALBlog
    {
        public IDbHelper DbInstance
        {
            get
            {
                return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs");
            }
        }

        public blog_tb_blog GetEntity(string id)
        {
            EntityHelper<blog_tb_blog> eq = new EntityHelper<blog_tb_blog>("blog_tb_blog", "blogID", null);
            return eq.GetEntity(id, DbInstance);
        }

        /// <summary>
        /// 获取数据库第一条博客
        /// </summary>
        /// <returns></returns>
        /// fangyj 2015-6-22
        public blog_tb_blog GetFirstEntity()
        {
            string sql = "select  * from blog_tb_blog where blogIsDisabled=0 limit 0,1";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
        }

        /// <summary>
        /// 获取用户的一个博客  (第一个)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>2015-07-25</returns>
        public blog_tb_blog GetSingleBlogByUserID(string userID)
        {
            string sql = "select  * from blog_tb_blog where userID=@userID limit 0,1";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@userID", userID));
            return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
        }


        public List<blog_tb_article> GetBlogRss(string blogID)
        {
            string sql = @"
	  /*最新文章 前10条 */
	select blog_tb_article.articleID,articleTitle,articleDatetime,articleSource,articleAuthor,c.articleSubContentText 
	from blog_tb_article 
	left join blog_tb_article_content c on c.articleID=blog_tb_article.articleID
	where blog_tb_article.blogID=@blogID
	 and articleIsDisabled=0
	 and articleIsHidden=0
    order by articleDatetime desc
    limit 0,10
";

            DataTable dt = this.DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_article>(dt);
        }

        /// <summary>
        /// 获取博客母板页数据
        /// </summary>
        /// <param name="blogID"></param>
        /// <returns></returns>
        public SharedModel GetBlogSharedData(string blogID)
        {
            string sql = @"
	/*获取浏览次数和回复次数*/
     select COUNT(blog_tb_article.articleID) as TotalArticleCount,  IFNULL( sum(articleClickTimes),0) as TotalArticleViewCount,IFNULL(sum(articleCommentTimes),0) as TotalArticleCommentCount from blog_tb_article
	 left join blog_tb_article_extend on blog_tb_article_extend.articleID=blog_tb_article.articleID
	  where blogID=@blogID   ;

	  /*获取菜单*/
	select menuUrl, menuDisplay,menuTarget,menuOrder from blog_tb_menu where blogID=@blogID and menuIsDisabled=0 	order by menuOrder DESC  ;

	/*获取前10条友情链接列表 */
	select  linkID,blogID,linkName,linkUrl,linkPic,linkOrder,linkIsDisabled,ADD_DATE,UPDATE_DATE from blog_tb_link where blogID=@blogID and linkIsDisabled=0 order by linkOrder desc limit 0,10
";
            DataSet ds = this.DbInstance.GetDataSet(sql, DbInstance.CreateParameter("@blogID", blogID));

            SharedModel model = new SharedModel();
            model.TotalArticleCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalArticleCount"]);
            model.TotalArticleViewCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalArticleViewCount"]);
            model.TotalArticleCommentCount = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalArticleCommentCount"]);
            model.Menus = FYJ.ObjectHelper.DataTableToModel<blog_tb_menu>(ds.Tables[1]);
            model.Links = ObjectHelper.DataTableToModel<blog_tb_link>(ds.Tables[2]);

            return model;
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
        /// 获取个人博客首页的置顶文章和文章预览列表 tag categoryID 可以传''不能传null  因为存储过程Null判断有点问题
        public IndexModel GetBlogIndexData(string blogID, int page = 1, int pageSize = 10, string categoryID = null, string tagID = null, int year = 0, int month = 0)
        {
            string sql = @"
   /*最新文章 前10条*/
	select articleID,articleTitle  from blog_tb_article  
  LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
  where blog_tb_article.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0 order by  articleDatetime desc limit 0,10 ;
	
	/*阅读排行榜*/
	select blog_tb_article.articleID,articleTitle,articleClickTimes   
	from blog_tb_article 
	left join blog_tb_article_extend e on e.articleID=blog_tb_article.articleID
    LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
	where blog_tb_article.blogID=@blogID  and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0
	order by articleClickTimes desc limit 0,10 ;

	/*评论排行榜*/
	select blog_tb_article.articleID,articleTitle,articleCommentTimes   
	from blog_tb_article 
	left join blog_tb_article_extend e on e.articleID=blog_tb_article.articleID
    LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
	where blog_tb_article.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0  and c.categoryIsDisabled=0
   order by articleCommentTimes desc limit 0,10 ;

	/*按月份的文章档案*/

select yyyy,mm,(select COUNT(*)  from blog_tb_article where articleIsDisabled=0 and articleIsHidden=0 and  LEFT(concat('0',MONTH(articleDatetime)),2)=mm and YEAR(articleDatetime)=yyyy  and blogID=@blogID) as ArticleCount
from (
 SELECT DISTINCT YEAR(articleDatetime) as yyyy,LEFT(concat('0',MONTH(articleDatetime)),2) as mm FROM blog_tb_article
where blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0
) c  order by CONCAT(yyyy,mm) desc;
	

    /*获取博客分类*/
	select ArticleCount,blog_tb_category.categoryID,categoryDisplay ,categoryDomain from blog_tb_category 
inner join blog_tb_blog on blog_tb_category.blogID=blog_tb_blog.blogID 
left join (select COUNT(*) as ArticleCount,categoryID 
from blog_tb_article where blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 group by categoryID) as table1
on blog_tb_category.categoryID=table1.categoryID
where blog_tb_category.blogID=@blogID 
and ArticleCount>0
and categoryIsDisabled=0
order by categoryOrderWeight asc   ;

 /*获取标签 */
  select  (select COUNT(1) from blog_tb_tagArticle where tagID=blog_tb_tag.tagID) as ArticleCount,blog_tb_tag.tagID,tagDisplay 
	from blog_tb_tag 
	inner join blog_tb_blog on blog_tb_tag.blogID=blog_tb_blog.blogID 
	left join blog_tb_category on blog_tb_tag.categoryID=blog_tb_category.categoryID 
	left join (select COUNT(1) as ArticleCount,tagID from blog_tb_tagArticle group by tagID) table1
	on table1.tagID=blog_tb_tag.tagID
	where blog_tb_tag.blogID=@blogID
	and ArticleCount>0 
    order by tagOrder asc   ;


	/*获取随机10条*/
	select  articleID,articleTitle   from  blog_tb_article 
    LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
    where blog_tb_article.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0 order by rand() limit 0,10
";

            DataSet ds = this.DbInstance.GetDataSet(sql, DbInstance.CreateParameter("@blogID", blogID));

            IndexModel model = new IndexModel();
            model.NewArticles = FYJ.ObjectHelper.DataTableToModel<ArticleLink>(ds.Tables[0]);
            model.MostViewArticles = FYJ.ObjectHelper.DataTableToModel<ArticleLink>(ds.Tables[1]);
            model.MostCommentArticles = FYJ.ObjectHelper.DataTableToModel<ArticleLink>(ds.Tables[2]);
            model.Months = FYJ.ObjectHelper.DataTableToModel<Month>(ds.Tables[3]);
            model.categorys = FYJ.ObjectHelper.DataTableToModel<blog_tb_category>(ds.Tables[4]);
            model.Tags = FYJ.ObjectHelper.DataTableToModel<blog_tb_tag>(ds.Tables[5]);
            model.RandomArticles = FYJ.ObjectHelper.DataTableToModel<ArticleLink>(ds.Tables[6]);


            string s = @"
             select a.*,
            b.userID,
            b.blogName,
            b.blogTitle,
            b.blogDomain,
            c.categoryDomain,
            c.categoryName,
            c.categoryDisplay,
            e.articleClickTimes,
            e.articleCommentTimes,
            topic.topicDisplay,
            ct.articleSubContentHtml,
            ct.articleSubContentText,
            '' as tagIDs,
            '' as tagDisplays,
            '' as tagUrls
            from  blog_tb_article a
            left JOIN blog_tb_category c on a.categoryID=c.categoryID
            left join blog_tb_article_extend e on e.articleID=a.articleID
            left JOIN blog_tb_topic topic on topic.topicID=a.topicID
            inner join blog_tb_article_content ct on ct.articleID=a.articleID
            inner join blog_tb_blog b on b.blogID=a.blogID
            where a.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 and articleIsDelete=0 
            and c.categoryIsDisabled=0
            ";

            StringBuilder sb = new StringBuilder();
            //置顶文章的sql
            string topsql = s + "   AND articleIsTop = 1  ORDER BY articleDatetime DESC  ;";

            sb.AppendLine(topsql);
            string limit = ((page - 1) * pageSize) + "," + pageSize;
            if (!String.IsNullOrEmpty(tagID))
            {
                sb.AppendLine(s + @"
					 and a.articleID in (select articleID from  blog_tb_tagArticle  where tagID=@tagID)
                     order by articleDatetime desc ,articleIsTop DESC,articleOrder DESC
                     limit {0}    ;

                     select count(*) from blog_tb_article 
                    left JOIN blog_tb_category c on blog_tb_article.categoryID=c.categoryID  
                     where  articleID in (select articleID from  blog_tb_tagArticle  where tagID=@tagID) and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0;

                     select tagDisplay+'-'+blogTitle as title from blog_tb_tag inner join blog_tb_blog on blog_tb_blog.blogID=blog_tb_tag.blogID where tagID=@tagID;
                     ");
            }

            else if (year > 0 && month > 0)
            {
                sb.AppendLine(s + @"
					 and YEAR(articleDatetime)=@year and MONTH(articleDatetime)=@month 
                     order by articleDatetime desc ,articleIsTop DESC,articleOrder DESC
                     limit {0}    ;
                      
                    select COUNT(*) from blog_tb_article  
                    left JOIN blog_tb_category c on blog_tb_article.categoryID=c.categoryID  
                    where YEAR(articleDatetime)=@year and MONTH(articleDatetime)=@month  and blog_tb_article.blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0;
                     
                    select CONCAT(blogTitle+'-',@year,@month) as title from blog_tb_blog where blogID=@blogID ;
                    ");
            }

            else if (!String.IsNullOrEmpty(categoryID))
            {
                sb.AppendLine(s + @"
					 and a.categoryID=@categoryID
                     order by articleDatetime desc ,articleIsTop DESC,articleOrder DESC
                     limit {0}    ;
                    
                     select COUNT(*) from blog_tb_article  
                       left JOIN blog_tb_category c on blog_tb_article.categoryID=c.categoryID  
                    where blog_tb_article.categoryID=@categoryID and articleIsDisabled=0 and articleIsHidden=0  and c.categoryIsDisabled=0;

                     select categoryDisplay+'-'+blogTitle as title from blog_tb_category  inner join blog_tb_blog on blog_tb_blog.blogID=blog_tb_category.blogID  where categoryID=@categoryID;
                     ");
            }
            else
            {
                sb.AppendLine(s + @"
                     order by articleIsTop DESC,articleOrder DESC,articleDatetime desc 
                     limit {0}   ;

                     select COUNT(*) from blog_tb_article 
                    left JOIN blog_tb_category c on blog_tb_article.categoryID=c.categoryID  
                    where blog_tb_article.blogID=@blogID  and articleIsDisabled=0 and articleIsHidden=0 and c.categoryIsDisabled=0;

                       select blogTitle as title from blog_tb_blog  where blogID=@blogID;
                     ");
            }

            List<IDataParameter> list = new List<IDataParameter>();
            list.Add(DbInstance.CreateParameter("@blogID", blogID));
            list.Add(DbInstance.CreateParameter("@page", page));
            list.Add(DbInstance.CreateParameter("@pageSize", pageSize));
            list.Add(DbInstance.CreateParameter("@categoryID", categoryID));
            list.Add(DbInstance.CreateParameter("@tagID", tagID));
            list.Add(DbInstance.CreateParameter("@year", year));
            list.Add(DbInstance.CreateParameter("@month", month));

            ds = DbInstance.GetDataSet(sb.ToString().Replace("{0}", limit), list.ToArray());

            //string sql = "DECLARE	@return_value int EXEC	@return_value = [dbo].[blog_proc_article] @blogID = N'" + blogID + "'";
            //sql += ",@page=" + page;
            //sql += ",@pageSize=" + pageSize;
            //sql += ",@categoryID='" + categoryID + "'";
            //sql += ",@tagID='" + tagID + "'";
            //sql += ",@year=" + year;
            //sql += ",@month=" + month;
            //sql += "  SELECT	'Return Value' = @return_value";
            //DataSet ds = this.DbInstance.GetDataSet(sql);
            model.TopArticles = FYJ.ObjectHelper.DataTableToModel<ArticleIndexLink>(ds.Tables[0]);
            model.IndexArticles = FYJ.ObjectHelper.DataTableToModel<ArticleIndexLink>(ds.Tables[1]);
            model.RecordCount = Convert.ToInt32(ds.Tables[2].Rows[0][0]);
            return model;
        }

        public List<ArticlePic> GetMainArticlePic(string blogID)
        {
            string sql =
                @"
                select 
                articleID,
                articlePic,
                articleTitle
                from blog_tb_article
                LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
                where blog_tb_article.blogID=@blogID
                and blog_tb_article.articleIsDisabled=0
                and blog_tb_article.articleIsHidden=0
                and blog_tb_article.articleIsDelete=0
                and c.categoryIsDisabled=0
                and articlePic is not null and articlePic<>'' order by articleDatetime desc
                limit 0,10
                ";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));

            return FYJ.ObjectHelper.DataTableToModel<ArticlePic>(dt);
        }



        public int Save(blog_tb_blog entity)
        {
            EntityHelper<blog_tb_blog> eq = new EntityHelper<blog_tb_blog>("blog_tb_blog", "blogID", null);
            string sql = "select 1 from blog_tb_blog where blogID=@blogID";
            if (DbInstance.Exists(sql, DbInstance.CreateParameter("blogID", entity.blogID)))
            {
                entity.UPDATE_DATE = DateTime.Now;
                return eq.Update(entity, DbInstance);
            }
            else
            {
                entity.blogID = new Random().Next(10000000, 99999999);
                entity.blogAddDatetime = DateTime.Now;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;
                return eq.Insert(entity, DbInstance);
            }
        }


        public List<sitemap> GetSitemap(string blogID, bool isShowHide)
        {
            string showHide = "";
            if(isShowHide==false)
            {
                showHide = " and blog_tb_article.articleIsHidden=0";
            }

            string sql = @"  
            select menuUrl, menuDisplay from blog_tb_menu where blogID=@blogID and menuIsDisabled=0 	order by menuOrder DESC;

            select categoryID,categoryDisplay,categoryDomain from blog_tb_category where categoryIsDisabled='0' and blogID=@blogID  order by categoryOrderWeight desc;

            select yyyy,mm  from (
             SELECT DISTINCT YEAR(articleDatetime) as yyyy,LEFT(concat('0',MONTH(articleDatetime)),2) as mm FROM blog_tb_article
            where blogID=@blogID and articleIsDisabled=0 and articleIsHidden=0
            ) c  order by CONCAT(yyyy,mm) desc;

            select 
            articleID,
            articleTitle,
            articleDatetime,
            blog_tb_article.UPDATE_DATE 
            from blog_tb_article 
            LEFT JOIN blog_tb_category c on c.categoryID=blog_tb_article.categoryID
            where blog_tb_article.blogID=@blogID
            and blog_tb_article.articleIsDisabled=0
            {0}
            and blog_tb_article.articleIsDelete=0
            and c.categoryIsDisabled=0
            order by articleDatetime desc;

            SELECT linkName,linkUrl,UPDATE_DATE from blog_tb_link where linkIsDisabled=FALSE
                ";
            sql = String.Format(sql,showHide);

            DataSet ds = DbInstance.GetDataSet(sql, blogID);

            string domain = "";
            if(ds.Tables[0].Rows.Count>0)
            {
                domain = ds.Tables[0].Rows[0]["blogDomain"].ToString();
            }
            else
            {
                throw new CustomException("博客不存在");
            }

            List<sitemap> list = new List<sitemap>();
            foreach(DataRow dr in ds.Tables[0].Rows)
            {
                sitemap m = new sitemap();
                m.Mtype = "menu";
                m.Text = dr["menuDisplay"].ToString();
                m.changefreq = "daily";
                m.priority = "0.9";
                m.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                m.Url = IocFactory<IBlogFix>.Instance.GetMenuUrl(dr["menuUrl"].ToString());
                list.Add(m);
            }

            foreach (DataRow dr in ds.Tables[1].Rows)
            {
                sitemap m = new sitemap();
                m.Mtype = "category";
                m.Text = dr["categoryDisplay"].ToString();
                m.changefreq = "daily";
                m.priority = "0.8";
                m.lastmod = DateTime.Now.ToString("yyyy-MM-dd");
                m.Url = IocFactory<IBlogFix>.Instance.GetCategoryUrl(dr["categoryID"].ToString(), dr["categoryDomain"].ToString());
                list.Add(m);
            }

            foreach (DataRow dr in ds.Tables[2].Rows)
            {
                sitemap m = new sitemap();
                m.Mtype = "month";
                m.Text = dr["yyyy"]+"年"+dr["mm"]+"月";
                m.changefreq = "daily";
                m.priority = "0.7";
                m.lastmod = dr["yyyy"] + "-" + dr["mm"] + "-28";
                m.Url = IocFactory<IBlogFix>.Instance.GetMonthUrl(dr["yyyy"].ToString(), dr["mm"].ToString());
                list.Add(m);
            }

            foreach (DataRow dr in ds.Tables[3].Rows)
            {
                sitemap m = new sitemap();
                m.Mtype = "article";
                m.Text = dr["articleTitle"].ToString();
                m.changefreq = "never";
                m.priority = "0.5";
                m.lastmod = Convert.ToDateTime(dr["UPDATE_DATE"]).ToString("yyyy-MM-dd");
                m.Url = IocFactory<IBlogFix>.Instance.GetArticleUrl( dr["articleID"].ToString());
                list.Add(m);
            }

            foreach (DataRow dr in ds.Tables[4].Rows)
            {
                sitemap m = new sitemap();
                m.Mtype = "link";
                m.Text = dr["linkName"].ToString();
                m.changefreq = "never";
                m.priority = "0.5";
                m.lastmod = Convert.ToDateTime(dr["UPDATE_DATE"]).ToString("yyyy-MM-dd");
                m.Url = dr["linkUrl"].ToString();
                list.Add(m);
            }

            return list;
        }

        public List<string> CheckID(string userID)
        {
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("@userID", userID);
            //DataSet ds = this.DbInstance.RunProcedure("blog_proc_AllID", dic);
            //DataTable dt = ds.Tables[0];
            //List<string> list = new List<string>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    list.Add(dr["id"].ToString());
            //}

            //return list;
            throw new NotImplementedException();
        }


        public blog_tb_blog GetSingleBlogByDomain(string domain,int port)
        {
            string sql = "select  * from blog_tb_blog where blogID =(select blogID from blog_tb_domain where replace(blogDomain,'www.','')=@blogDomain and port=@port limit 0,1) ";
            DataTable dt = DbInstance.GetDataTable(sql
                , DbInstance.CreateParameter("@blogDomain", domain.Replace("www.", ""))
                , DbInstance.CreateParameter("@port", port));
            if (dt.Rows.Count > 0)
            {
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            sql = "select blogID from blog_tb_category where categoryDomain=@categoryDomain";
            dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@categoryDomain", domain));
            if (dt.Rows.Count > 0)
            {
                return GetEntity(dt.Rows[0]["blogID"].ToString());
            }

            sql = "select * from blog_tb_blog where blogName=@blogName";
            dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogName", domain.Replace(".devblog.cn", "")));
            if (dt.Rows.Count > 0)
            {
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            //如果都没查找到取第一条
            sql = "select * from blog_tb_blog limit 0,1";
            dt = DbInstance.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                return ObjectHelper.DataTableToSingleModel<blog_tb_blog>(dt);
            }

            return null;
        }


        public List<blog_tb_SiteCategory> QuerySiteCategory()
        {
            string sql = "select * from blog_tb_SiteCategory where  IsDisabled=0 order by OrderWeight desc";
            DataTable dt = DbInstance.GetDataTable(sql);
            return ObjectHelper.DataTableToModel<blog_tb_SiteCategory>(dt);
        }


       
        public int SaveBlogCount(string blogID)
        {
            string sql = "select  * from blog_tb_blog_count where blogID=@blogID";
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            if (db.Exists(sql, db.CreateParameter("@blogID", blogID)))
            {
                sql = "update blog_tb_blog_count set PV=IFNULL(PV,0)+1,UPDATE_DATE=@UPDATE_DATE where blogID=@blogID";
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("blogID", blogID);
                dic.Add("UPDATE_DATE", DateTime.Now);
                db.ExecuteSql(sql, dic);
            }
            else
            {
                blog_tb_blog_count entity = new blog_tb_blog_count();
                entity.ID = Guid.NewGuid().ToString("N");
                entity.BlogID = Convert.ToInt32(blogID);
                entity.PV = 1;
                entity.AddPV = 0;
                entity.ADD_DATE = DateTime.Now;
                entity.UPDATE_DATE = DateTime.Now;

                FYJ.Data.Entity.EntityHelper<blog_tb_blog_count>.Insert(entity, "blog_tb_blog_count", "ID", true, db);
            }

            return 1;
        }

        public int GetTotalPV(string blogID)
        {
            string sql = "select  * from blog_tb_blog_count where blogID=@blogID";
            if (DbInstance.Exists(sql, DbInstance.CreateParameter("@blogID", blogID)))
            {
                sql = "select PV+AddPV as TotalPV from blog_tb_blog_count where blogID=@blogID";
                return DbInstance.GetInt(sql, DbInstance.CreateParameter("@blogID", blogID));
            }

            return 0;
        }

        public bool IsExistsBlogName(string name)
        {
            string sql = "select  * from blog_tb_blog where blogName=@blogName  limit 0,1";
            bool b = DbInstance.Exists(sql, DbInstance.CreateParameter("@blogName", name));
            return b;
        }

        public bool IsBlogDisabled(string blogID)
        {
            string sql = "select  blogIsDisabled from blog_tb_blog where blogID=@blogID";
            return DbInstance.GetBoolean(sql, DbInstance.CreateParameter("@blogID", blogID));
        }

        public int AddDomain(blog_tb_domain entity)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return EntityHelper<blog_tb_domain>.Insert(entity, "blog_tb_domain", "ID", true, db);
        }

        public int UpdateDomain(blog_tb_domain entity)
        {
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return EntityHelper<blog_tb_domain>.Update(entity, "blog_tb_domain",db, "ID");
        }

        public blog_tb_domain QueryDomain(string blogID, string domain, int port)
        {
            if(domain==null)
            {
                domain = "";
            }
            else
            {
                domain = domain.Trim();
            }

            string sql = "select * from blog_tb_domain where blogID=@blogID and ifnull(domain,'')=@domain and port=@port";
            DataTable dt = DbInstance.GetDataTable(sql,DbInstance.CreateParameter("@blogID",blogID)
                , DbInstance.CreateParameter("@domain", domain)
                , DbInstance.CreateParameter("@port", port));

            return ObjectHelper.DataTableToSingleModel<blog_tb_domain>(dt);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
       public blog_tb_domain GetDomainEntity(string ID)
        {
            string sql = "select * from blog_tb_domain where ID=@ID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@ID", ID));

            return ObjectHelper.DataTableToSingleModel<blog_tb_domain>(dt);
        }

        public int DeleteSingleDomain(string ID)
        {
            string sql = "delete from blog_tb_domain where ID=@ID";
            IDbHelper db = IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs-Write");
            return db.ExecuteSql(sql,db.CreateParameter("@ID",ID));
        }

        public List<blog_tb_domain> GetDomainList(string blogID)
        {
            string sql = "select * from blog_tb_domain where blogID=@blogID";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@blogID", blogID));

            return ObjectHelper.DataTableToModel<blog_tb_domain>(dt);
        }
    }
}


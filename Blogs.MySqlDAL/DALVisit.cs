using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using System;
using System.Data;

namespace Blogs.DAL
{
    public class DALVisit : IDALVisit
    {

        protected IDbHelper DbInstance
        {
            get
            {
                return IocFactory<IDbFactory>.Instance.GetDbInstance("Visit");
            }
        }


        public int AddCount(Entity.blog_tb_VisitCount entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_VisitCount>.Insert(entity, "blog_tb_VisitCount", "ID", true, DbInstance);
        }

        public int UpdateCount(blog_tb_VisitCount entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_VisitCount>.Update(entity, "blog_tb_VisitCount", DbInstance, "ID");
        }

        public int AddVisit(Entity.blog_tb_Visit entity)
        {
            Uri u = new Uri(entity.visitUrl);
            string domain = u.Host.TrimEnd('.');
            if (domain.StartsWith("www."))
            {
                domain = domain.Substring(4);
            }

            entity.Domain = domain;

            try
            {
                string sql = "";

                if (!DbInstance.Exists("select * from blog_tb_IpAddress where IP=@IP", DbInstance.CreateParameter("@IP", entity.visitIP)))
                {
                    blog_tb_IpAddress address = new blog_tb_IpAddress();
                    address.id = Guid.NewGuid().ToString("N");
                    address.IP = entity.visitIP;
                    address.City = entity.City;
                    address.Contry = entity.County;
                    address.ADD_DATE = DateTime.Now;
                    address.UPDATE_DATE = DateTime.Now;

                    AddIPAddress(address);
                }

                blog_tb_VisitCount countEntity = null;

                sql = "select * from blog_tb_VisitCount where Domain=@Domain and CountDate=@CountDate";
                DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@Domain", domain), DbInstance.CreateParameter("@CountDate", DateTime.Now.Date));
                countEntity = ObjectHelper.DataTableToSingleModel<blog_tb_VisitCount>(dt);


                bool isAddCount = (countEntity == null);
                if (countEntity == null)
                {
                    countEntity = new blog_tb_VisitCount();
                    countEntity.Domain = domain;
                    countEntity.SiteID = entity.siteID;
                    countEntity.ID = Guid.NewGuid().ToString("N");
                    countEntity.ADD_DATE = DateTime.Now;
                    countEntity.CountDate = DateTime.Now;
                }

                countEntity.PV += 1;


                if (entity.SessionID != null)
                {
                    sql = "select 1 from blog_tb_Visit where Domain=@Domain and SessionID=@SessionID and datediff(ADD_DATE,CURRENT_DATE)=0";
                    if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@Domain", domain), DbInstance.CreateParameter("@SessionID", entity.SessionID)))
                    {
                        countEntity.UV += 1;
                    }
                }

                sql = "select * from blog_tb_Visit where Domain=@Domain and visitIP=@visitIP and datediff(ADD_DATE,CURRENT_DATE)=0";
                if (!DbInstance.Exists(sql, DbInstance.CreateParameter("@Domain", domain), DbInstance.CreateParameter("@visitIP", entity.visitIP)))
                {
                    countEntity.IP += 1;
                }

                countEntity.UPDATE_DATE = DateTime.Now;

                if (isAddCount)
                {
                    AddCount(countEntity);
                }
                else
                {
                    UpdateCount(countEntity);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                FYJ.Data.Entity.EntityHelper<blog_tb_Visit>.Insert(entity, "blog_tb_Visit", "visitID", true, DbInstance);
            }

            return 1;
        }

        public int AddIPAddress(blog_tb_IpAddress entity)
        {
            return FYJ.Data.Entity.EntityHelper<blog_tb_IpAddress>.Insert(entity, "blog_tb_IpAddress", "ID", true, DbInstance);
        }


    }
}

using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using System.Collections.Generic;
using System.Data;


namespace Blogs.DAL
{
    public class DALTalk : IDALTalk
    {
        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public List<blog_tb_Talk> Query(string userID)
        {
            string sql = "select   *  from blog_tb_Talk where UserID=@UserID and IsTemp=0 and IsDisabled=0 order by TalkDatetime DESC limit 0,100";
            DataTable dt = DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@UserID", userID));
            return ObjectHelper.DataTableToModel<blog_tb_Talk>(dt);
        }
    }
}

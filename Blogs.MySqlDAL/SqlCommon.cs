using FYJ;
using FYJ.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class SqlCommon
    {

        private static IDataParameter[] GetParameters(IDbHelper db, IDictionary<string, object> dic)
        {
            List<IDataParameter> list = new List<IDataParameter>();
            if (dic != null)
            {
                foreach (var item in dic)
                {
                    IDataParameter d = db.CreateParameter(item.Key, item.Value);
                    list.Add(d);
                }
            }

            return list.ToArray();
        }

        public static DataTable GetTable(IDbHelper db, GridPager pager, string select, string tableName)
        {

            string sql = String.Empty;
            if (pager.Offset == -1)
            {
                sql = String.Format("select {0} from {1} {2} {3} limit {4},{5}  ", select, tableName, pager.QueryString, pager.OrderString, pager.PageSize * (pager.CurrentPage - 1), pager.PageSize);
            }
            else
            {
                sql = String.Format("select {0} from {1} {2} {3} limit {4},{5}  ", select, tableName, pager.QueryString, pager.OrderString, pager.Offset, pager.Limit);
            }
            String countSql = String.Format("select count(*) from  {0} {1}", tableName, pager.QueryString);

            DataTable countdt = db.GetDataTable(countSql, GetParameters(db, pager.Parameters));
            pager.TotalRows = Convert.ToInt32(countdt.Rows[0][0]);

            DataTable dt = db.GetDataTable(sql, GetParameters(db, pager.Parameters));

            return dt;
        }

        public static int Delete(IDbHelper db, string tableName, string idName, string id)
        {
            string tmp = "";
            foreach (string s in id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string t = s.Trim('\'');
                tmp += "'" + t + "',";
            }
            tmp = tmp.TrimEnd(',');
            string sql = "delete from " + tableName + " where " + idName + " in (" + tmp + ")";


            return db.ExecuteSql(sql);
        }
    }
}

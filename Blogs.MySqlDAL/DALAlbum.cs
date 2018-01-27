using Blogs.Entity;
using Blogs.IDAL;
using FYJ;
using FYJ.Data;
using FYJ.Data.Entity;
using System.Collections.Generic;
using System.Data;

namespace Blogs.DAL
{
    public class DALAlbum : IDALAlbum
    {

        private IDbHelper DbInstance
        {
            get { return IocFactory<IDbFactory>.Instance.GetDbInstance("Blogs"); }
        }

        public List<blog_tb_Album> QueryAlbum(string userID)
        {
            string sql = @"
SELECT
	a.ID,
	a.UserID,
  a. NAME,
	a.Display,
	a.ADD_DATE,
	a.Permission,
	a.UPDATE_DATE,
	IFNULL(c.cc, 0) AS PhotoCount,
	CASE WHEN IFNULL(CoverUrl, '') = '' THEN p.ThumbUrl ELSE CoverUrl END AS CoverUrl
FROM
	blog_tb_Album AS a
LEFT  JOIN (
	SELECT COUNT(*) AS cc,AlbumID
	FROM
		blog_tb_Photo
	GROUP BY
		AlbumID
) AS c ON c.AlbumID = a.ID
LEFT OUTER JOIN (
	SELECT
		AlbumID,	MIN(ThumbUrl) AS ThumbUrl
	FROM
		blog_tb_Photo AS blog_tb_Photo_1
	GROUP BY
		AlbumID
) AS p ON p.AlbumID = a.ID 
where a.UserID=@UserID
";
            DataTable dt= DbInstance.GetDataTable(sql, DbInstance.CreateParameter("@UserID", userID));

            return FYJ.ObjectHelper.DataTableToModel<blog_tb_Album>(dt);
        }


        public blog_tb_Album GetEntity(string id)
        {
            return EntityHelper<blog_tb_Album>.GetEntity("blog_tb_Album", "ID", id, DbInstance);
        }
    }
}

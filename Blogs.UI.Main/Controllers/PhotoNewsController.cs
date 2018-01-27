using Blogs.UI.Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class PhotoNewsController : Controller
    {
        // GET: PhotoNews
        public ActionResult Index()
        {
            FYJ.Data.DbHelper db = new FYJ.Data.DbHelper("System.Data.SqlClient", "server=.;Uid=sa;Pwd=533533;Database=db_163;");
            string sql = "select ZFAVORID as ID,ZTITLE as Title,newCover,imgsum from ZFAVORITE where ZFAVORTYPE='1'  order by datatime desc";
            DataTable dt = db.GetDataTable(sql);
            return View(dt);
        }


        public ActionResult Show(string id)
        {
            FYJ.Data.DbHelper db = new FYJ.Data.DbHelper("System.Data.SqlClient", "server=.;Uid=sa;Pwd=533533;Database=db_163;");
            DataTable albumDt = db.GetDataTable("select * from ZFAVORITE where ZFAVORID=@ID",db.CreateParameter("@ID",id));
            if(albumDt.Rows.Count==0)
            {
                return Content("没有查询到该ID的图片");
            }

            PhotoShowViewModel model = new PhotoShowViewModel();
            model.PhotoCollection = new List<Entity.blog_tb_Photo>();
        
            model.Title = albumDt.Rows[0]["ZTITLE"].ToString();
            DataTable dt = db.GetDataTable("select * from tb_newspic where NewsID=@NewsID", db.CreateParameter("@NewsID", id));

            //是否远程IP地址  2015-6-16
            bool isRemote = Utility.IsRemote;

            foreach (DataRow dr in dt.Rows)
            {
                Entity.blog_tb_Photo entity = new Entity.blog_tb_Photo();
                entity.ID = dr["ID"].ToString();
                entity.Display = "";
       
                entity.ThumbUrl = dr["NewUrl"].ToString();
                entity.Url = dr["NewUrl"].ToString();
                entity.Exif = dr["note"].ToString().Replace("\n", "<br/>");
                model.PhotoCollection.Add(entity);
            }
            if (dt.Rows.Count > 0)
            {
                model.CurrentThumbUrl = dt.Rows[0]["NewUrl"].ToString();
                model.CurrentUrl = dt.Rows[0]["NewUrl"].ToString();
            }

            return View("~/Views/PhotoNews/PhotoShow.cshtml", model);
        }
    }
}
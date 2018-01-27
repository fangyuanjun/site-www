using Blogs.Entity;
using Blogs.UI.Main.Models;
using Blogs.UI.Main.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main.Controllers
{
    public class AlbumController : BaseController
    {
        private string GetVersion()
        {
            string ver = Utility.AlbumVersion;
            if (!String.IsNullOrEmpty(Request["ver"]))
            {
                ver = Request["ver"];
            }

            return ver;
        }
        //
        // GET: /Album/
        [CountFilter]
        public ActionResult Index()
        {
            AlbumViewModel model = new AlbumViewModel();
            model.SiteID = BlogID;
            model.AlbumCollection = Utility.AlbumBll.QueryAlbum(base.Info.userID);
            model.Title = "相册 - " + base.Info.blogTitle;

            foreach (var v in model.AlbumCollection)
            {
                v.Photos = new List<string> { v.CoverUrl };
            }

            return View("~/Views/Album/" + GetVersion() + "/Index.cshtml", model);
        }

        public ActionResult PhotoList()
        {
            string albumID = Request["albumID"];
            PhotoListViewModel model = new PhotoListViewModel();
            model.SiteID = BlogID;
            model.PhotoCollection = new List<Entity.blog_tb_Photo>();
            Entity.blog_tb_Album album = Utility.AlbumBll.GetEntity(albumID);
            model.Title = album.Display + "-" + base.Info.blogTitle;
            List<blog_tb_Photo> list = Utility.PhotoBll.Query(albumID);

            //是否远程IP地址  2015-6-16
            bool isRemote = Utility.IsRemote;

            foreach (var v in list)
            {
                Entity.blog_tb_Photo entity = new Entity.blog_tb_Photo();
                entity.ID = v.ID;
                entity.Display = v.Display;
                if (String.IsNullOrEmpty(entity.Display))
                {
                    entity.Display = v.FileName;
                }

                string thumbUrl = v.ThumbUrl;
                if (isRemote)
                {
                    thumbUrl = Utility.ReplaceImgOrFileSrc(thumbUrl);
                }

                string url = v.Url;
                if (isRemote)
                {
                    url = Utility.ReplaceImgOrFileSrc(url);
                }

                entity.ThumbUrl = thumbUrl;
                if (String.IsNullOrEmpty(thumbUrl))
                {
                    entity.ThumbUrl = url;
                }
                model.PhotoCollection.Add(entity);
            }

            return View("~/Views/Album/" + GetVersion() + "/PhotoList.cshtml", model);
        }

        [CountFilter]
        public ActionResult PhotoShow(string id)
        {
            string albumID = id;
            PhotoShowViewModel model = new PhotoShowViewModel();
            model.SiteID = BlogID;
            model.PhotoCollection = new List<Entity.blog_tb_Photo>();
            Entity.blog_tb_Album album = Utility.AlbumBll.GetEntity(albumID);
            model.Title = album.Display + "-" + base.Info.blogTitle;
            List<blog_tb_Photo> list = Utility.PhotoBll.Query(albumID);

            //是否远程IP地址  2015-6-16
            bool isRemote = Utility.IsRemote;

            foreach (var v in list)
            {
                Entity.blog_tb_Photo entity = new Entity.blog_tb_Photo();
                entity.ID = v.ID;
                entity.Display = v.Display;
                if (String.IsNullOrEmpty(entity.Display))
                {
                    entity.Display = v.FileName;
                }

                string thumbUrl = v.ThumbUrl;
                if (isRemote)
                {
                    thumbUrl = Utility.ReplaceImgOrFileSrc(thumbUrl);
                }

                string url = v.Url;
                if (isRemote)
                {
                    url = Utility.ReplaceImgOrFileSrc(url);
                }

                entity.ThumbUrl = thumbUrl;
                entity.Url = url;
                entity.Exif = "文件名:" + entity.Display + "<br/>" + v.Exif.Replace("\n", "<br/>");
                model.PhotoCollection.Add(entity);
            }
            if (list.Count > 0)
            {
                string thumbUrl = list.First().ThumbUrl;
                if (isRemote)
                {
                    thumbUrl = Utility.ReplaceImgOrFileSrc(thumbUrl);
                }

                string url = list.First().Url;
                if (isRemote)
                {
                    url = Utility.ReplaceImgOrFileSrc(url);
                }

                model.CurrentThumbUrl = thumbUrl;
                model.CurrentUrl = url;
            }

            return View("~/Views/Album/" + GetVersion() + "/PhotoShow.cshtml", model);
        }

        public ActionResult ArtilePhotoShow()
        {
            string articleID = Request["articleID"];
            PhotoShowViewModel model = new PhotoShowViewModel();
            model.SiteID = BlogID;
            model.PhotoCollection = new List<Entity.blog_tb_Photo>();
            Entity.blog_tb_article article = Utility.ArticleBll.GetEntity(articleID);
            model.Title = article.articleTitle + "- 查看图片 -" + base.Info.blogTitle;
            List<blog_attachment> list = Utility.ArticleBll.GetArticlePhotos(articleID);
            if (list.Count == 0)
            {
                return Content("没有查询到该文章的图片");
            }
            //是否远程IP地址  2015-6-16
            bool isRemote = Utility.IsRemote;

            foreach (var v in list)
            {
                Entity.blog_tb_Photo entity = new Entity.blog_tb_Photo();
                entity.ID = v.fileID;
                entity.Display = v.fileName;

                string thumbUrl = v.fileThumbUrl;
                if (isRemote)
                {
                    thumbUrl = Utility.ReplaceImgOrFileSrc(thumbUrl);
                }

                string url = v.fileUrl;
                if (isRemote)
                {
                    url = Utility.ReplaceImgOrFileSrc(url);
                }

                if (String.IsNullOrEmpty(thumbUrl))
                {
                    thumbUrl = url;
                }

                entity.ThumbUrl = thumbUrl;
                entity.Url = url;

                if (String.IsNullOrEmpty(entity.Display))
                {
                    entity.Display = System.IO.Path.GetFileName(entity.Url);
                }

                if (!String.IsNullOrEmpty(v.Exif))
                {
                    entity.Exif = "文件名:" + entity.Display + "<br/>" + v.Exif.Replace("\n", "<br/>");
                }

                model.PhotoCollection.Add(entity);
                string currentUri = Server.UrlDecode(Request["uri"]);
                if (entity.ThumbUrl == currentUri || entity.Url == currentUri)
                {
                    model.CurrentThumbUrl = thumbUrl;
                    model.CurrentUrl = url;
                }
            }

            if (String.IsNullOrEmpty(model.CurrentThumbUrl))
            {
                model.CurrentThumbUrl = model.PhotoCollection.First().ThumbUrl;
            }

            if (String.IsNullOrEmpty(model.CurrentUrl))
            {
                model.CurrentUrl = model.PhotoCollection.First().Url;
            }

            return View("~/Views/Album/" + GetVersion() + "/PhotoShow.cshtml", model);
        }

        public ActionResult Exif()
        {
            string uri = Request["uri"];
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<li>文件名:</li>");
            sb.AppendLine("<li>描述:</li>");
            sb.AppendLine("<li>相机:Canon EOS 6D</li>");
            //<li>文件名:</li>
            //                <li>描述:</li>
            //                <li>相机:Canon EOS 6D</li>
            //                <li>镜头:EF17-40mm f/4L USM</li>
            //                <li>焦距:17</li>
            //                <li>ISO:100</li>
            //                <li>快门:1/333</li>
            //                <li>光圈值:F/9.1</li>
            //                <li>曝光补偿:0/1</li>
            //                <li>曝光模式:手动曝光</li>
            //                <li>测光模式:评价测光</li>
            //                <li>白平衡:自动</li>
            //                <li>闪光灯:关</li>
            //                <li>拍摄时间:2015:06:01 16:56:38</li>
            return Content(sb.ToString());
        }
    }
}

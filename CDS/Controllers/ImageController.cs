using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using LEAF_Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace CDS.Controllers
{
    public class ImageController : Controller
    {
        // GET: Images
        public ActionResult Index()
        {
            if (SessionManager.Current.UserID != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult CheckSession()
        {
            if (SessionManager.Current.UserID != 0)
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetImages(int bkid, int lid, bool AandD = false, bool isUpdate = false, bool IsIndex = false, int BooktypeID = 1)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.BookID = bkid;
                ViewBag.LessonID = lid;
                if (AandD)
                {
                    ViewBag.AandD = 1;
                }
                else
                {
                    ViewBag.AandD = 0;
                }
                if (isUpdate)
                {
                    ViewBag.isUpdate = 1;
                }
                else
                {
                    ViewBag.isUpdate = 0;
                }
                if (IsIndex == true)
                {
                    ViewBag.isIndex = 1;

                }
                else
                {
                    ViewBag.IsIndex = 0;
                }
                ViewBag.Bktype = BooktypeID;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }


        public ActionResult GoogleImage(string id, int dropdown)
        {
            if (SessionManager.Current.UserID != 0)
            {
                List<ImagesModel> imagesList = new List<ImagesModel>();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    imagesList = new ImagesModel().Search(id, dropdown);
                }
                return Json(imagesList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult LessonImages(string searchKeyWord, int bkid = 0, int pageNumber = 0,string imageType = "Lesson",int lid = 0)
        {

            if (SessionManager.Current.UserID != 0)
            {
                List<ImagesModel> imageList = new List<ImagesModel>();
                int userID = SessionManager.Current.UserID;
                imageList = new ImagesManager().GetImages(bkid, pageNumber, searchKeyWord, userID, imageType,lid);
                return Json(imageList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult LessonAudios(string searchKeyWord, int bkid = 0, int pageNumber = 0, string imageType = "Lesson", int lid = 0)
        {

            if (SessionManager.Current.UserID != 0)
            {
                List<ImagesModel> audioList = new List<ImagesModel>();
                int userID = SessionManager.Current.UserID;
                audioList = new ImagesManager().GetAudios(bkid, pageNumber, searchKeyWord, userID, imageType,lid);
                return Json(audioList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public void DeleteOldImages(string dirName)
        {
            DirectoryInfo taskDirectory = new DirectoryInfo(dirName);
            string[] files = Directory.GetFiles(dirName);

            foreach (string file in files)
            {
                if (file.Contains("temp_"))
                {
                    FileInfo fi = new FileInfo(file);
                    if (fi.LastAccessTime < DateTime.Now.AddDays(-1))
                    {
                        fi.Delete();
                    }
                }
            }
        }

        public ActionResult DeleteImage()
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    if (Session["AandDImageName"] != null)
                    {
                        string oldpath = Convert.ToString(Session["AandDImagePath"]);
                        //string tempname = Convert.ToString(Session["AandDImageName"]);
                        //string oldpath = foldername + "\\" + tempname + ".jpg";
                        FileInfo file = new FileInfo(oldpath);
                        if (file.Exists == true)
                        {
                            file.Delete();
                        }
                        Session["AandDImagePath"] = null;
                        Session["AandDImageName"] = null;
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult SaveImage(string image, int bkid, int lesid, int isGoogleImage, string description, int AandD = 0, int isUpdate = 0, int IsIndexPage = 0, int BooktypeID = 0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (!string.IsNullOrEmpty(image))
                {
                    try
                    {
                        int UserId = SessionManager.Current.UserID;
                        var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
                        var ext = 2;

                        ImagesManager objbll = new ImagesManager();
                        var fol = "";
                        DirectoryInfo folder = null;
                        if (IsIndexPage == 1)
                        {
                            fol = mainpath + "B_" + bkid + "\\BKType_" + BooktypeID + "\\PIndex_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\BKType_" + BooktypeID + "\\PIndex_" + lesid);
                        }
                        else
                        {
                            fol = mainpath + "B_" + bkid + "\\L_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
                        }


                        var folderfulname = @"" + folder.FullName;

                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }
                        int ImageID = 0;
                        if (IsIndexPage == 1)
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId,4, BooktypeID, IsIndexPage);
                        }
                        else
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId);
                        }

                        if (ImageID != 0)
                        {
                            string localPath = "";
                            string remoteImgPath = "";
                            if (isGoogleImage == 1)
                            {
                                localPath = folderfulname + "\\" + ImageID + ".jpg";
                                remoteImgPath = image;
                            }
                            else
                            {
                                localPath = folderfulname + "\\" + ImageID + ".jpg";
                                remoteImgPath = mainpath + image;

                            }


                            try
                            {
                                WebClient webClient = new WebClient();
                                webClient.UseDefaultCredentials = true;
                                webClient.DownloadFile(remoteImgPath, localPath);
                                    
                                if (isGoogleImage == 1)
                                {
                                    return Json("B_" + bkid + "\\L_" + lesid + "\\" + ImageID + ".jpg", JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    int[] arr = new int[2];
                                    arr[0] = 1;
                                    arr[1] = ImageID;
                                    return Json(arr, JsonRequestBehavior.AllowGet);
                                }
                            }
                            catch (Exception ex)
                            {
                                return Json(0, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(0, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult SaveAudio(string audio, int bkid, int lesid, string description)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (!string.IsNullOrEmpty(audio))
                {
                    try
                    {
                        int UserId = SessionManager.Current.UserID;
                        var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
                        var ext = 5;

                        ImagesManager objbll = new ImagesManager();

                        var fol = mainpath + "B_" + bkid + "\\L_" + lesid;

                        var folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);

                        var folderfulname = @"" + folder.FullName;

                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }

                        int ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description,UserId);

                        if (ImageID != 0)
                        {
                            string localPath = "";
                            string remoteImgPath = "";
                            localPath = folderfulname + "\\" + ImageID + ".wav";
                            remoteImgPath = mainpath + audio;


                            try
                            {
                                WebClient webClient = new WebClient();
                                webClient.UseDefaultCredentials = true;
                                webClient.DownloadFile(remoteImgPath, localPath);
                                return Json(1, JsonRequestBehavior.AllowGet);
                            }
                            catch (Exception ex)
                            {
                                return Json(0, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(0, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                        return Json(0, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpGet]
        public ActionResult SaveImageName(int imageId, string description)
        {
            ImagesManager objbll = new ImagesManager();
            if (objbll.SaveImageName(imageId, description))
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Upload()
        {
            int bkid = 0;
            int lesid = 0;
            int isUpdate = 0;
            string description = null;
            int BookTypeID = 1;
            int IsIndexPage = 0;
            var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            var ext = 0;
            string imagePath = null;
            int UserId = SessionManager.Current.UserID;
            ImagesManager objbll = new ImagesManager();
            if (Request.Files != null)
            {
                bkid = Int32.Parse(Request["bkid"].ToString());
                lesid = Int32.Parse(Request["lesid"].ToString());
                description = Request["description"];
                isUpdate = Int32.Parse(Request["isUpdate"].ToString());
                IsIndexPage = Int32.Parse(Request["IsIndexPage"].ToString());
                BookTypeID = Int32.Parse(Request["BookTypeID"].ToString());
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var filesize = file.ContentLength;
                    if (filesize < 12582912)
                    {
                        var fol = "";
                        DirectoryInfo folder = null;
                        if (IsIndexPage == 1)
                        {
                            fol = mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid);
                        }
                        else
                        {
                            fol = mainpath + "B_" + bkid + "\\L_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
                        }

                        var folderfulname = @"" + folder.FullName;
                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }

                        string vr = file.FileName.Substring(file.FileName.LastIndexOf("."));

                        if (vr == ".jpg" || vr == ".JPG")
                        {
                            ext = 2;
                        }

                        else if (vr == ".png" || vr == ".PNG")
                        {
                            ext = 1;
                        }
                        else if (vr == ".jpeg" || vr == ".JPEG")
                        {
                            ext = 3;
                        }

                        else if (vr == ".gif" || vr == ".GIF")
                        {
                            ext = 4;
                        }

                        else
                        {
                            return Json("Format Not Supported", JsonRequestBehavior.AllowGet);
                        }
                       
                        int ImageID = 0;
                        if (IsIndexPage == 1)
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId, 4, BookTypeID);
                        }
                        else
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId);
                        }

                        if (ImageID != 0)
                        {

                            string url = folderfulname + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));

                            file.SaveAs(url);
                            imagePath = "B_" + bkid + "\\L_" + lesid + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        }
                        return Json(imagePath, JsonRequestBehavior.AllowGet);
                    }
                    //var fileName = Path.GetFileName(file.FileName);

                    //var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    //file.SaveAs(path);
                }
                return Json(imagePath, JsonRequestBehavior.AllowGet);
            }
            return Json("Upload Fail! Try Again Later", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadAudio()
        {
            int bkid = 0;
            int lesid = 0;
            string description = null;
            int BookTypeID = 1;
            int IsIndexPage = 0;
            var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            var ext = 0;
            string imagePath = null;
            int UserId = SessionManager.Current.UserID;
            ImagesManager objbll = new ImagesManager();
            if (Request.Files != null)
            {
                bkid = Int32.Parse(Request["bkid"].ToString());
                lesid = Int32.Parse(Request["lesid"].ToString());
                description = Request["value"];
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var filesize = file.ContentLength;
                    if (filesize < 12582912)
                    {
                        var fol = "";
                        DirectoryInfo folder = null;
                        if (IsIndexPage == 1)
                        {
                            fol = mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid);
                        }
                        else
                        {
                            fol = mainpath + "B_" + bkid + "\\L_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
                        }

                        var folderfulname = @"" + folder.FullName;
                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }

                        string vr = file.FileName.Substring(file.FileName.LastIndexOf("."));

                        if (vr == ".wav" || vr == ".WAV")
                        {
                            ext = 5;
                        }

                        else if (vr == ".ogg" || vr == ".OGG")
                        {
                            ext = 6;
                        }
                        else if (vr == ".mp3" || vr == ".MP3")
                        {
                            ext = 7;
                        }

                        else if (vr == ".m4a" || vr == ".M4A")
                        {
                            ext = 11;
                        }

                        else
                        {
                            return Json("Format Not Supported", JsonRequestBehavior.AllowGet);
                        }

                        int ImageID = 0;
                        if (IsIndexPage == 1)
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId, 4, BookTypeID);
                        }
                        else
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId);
                        }

                        if (ImageID != 0)
                        {

                            string url = folderfulname + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));

                            file.SaveAs(url);
                            imagePath = "B_" + bkid + "\\L_" + lesid + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        }
                        return Json(imagePath, JsonRequestBehavior.AllowGet);
                    }
                    //var fileName = Path.GetFileName(file.FileName);

                    //var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    //file.SaveAs(path);
                }
                return Json(imagePath, JsonRequestBehavior.AllowGet);
            }
            return Json("Upload Fail! Try Again Later", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadVideos()
        {
            int bkid = 0;
            int lesid = 0;
            string description = null;
            int BookTypeID = 1;
            int IsIndexPage = 0;
            var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            var ext = 0;
            string imagePath = null;
            int UserId = SessionManager.Current.UserID;
            ImagesManager objbll = new ImagesManager();
            if (Request.Files != null)
            {
                bkid = Int32.Parse(Request["bkid"].ToString());
                lesid = Int32.Parse(Request["lesid"].ToString());
                description = Request["value"];
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var filesize = file.ContentLength;
                    if (filesize < 12582912)
                    {
                        var fol = "";
                        DirectoryInfo folder = null;
                        if (IsIndexPage == 1)
                        {
                            fol = mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\BKType_" + BookTypeID + "\\PIndex_" + lesid);
                        }
                        else
                        {
                            fol = mainpath + "B_" + bkid + "\\L_" + lesid;
                            folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
                        }

                        var folderfulname = @"" + folder.FullName;
                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }

                        string vr = file.FileName.Substring(file.FileName.LastIndexOf("."));

                        if (vr == ".mp4" || vr == ".MP4")
                        {
                            ext = 8;
                        }

                        else if (vr == ".mov" || vr == ".MOV")
                        {
                            ext = 9;
                        }

                        else
                        {
                            return Json("Format Not Supported", JsonRequestBehavior.AllowGet);
                        }

                        int ImageID = 0;
                        if (IsIndexPage == 1)
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId, 4, BookTypeID);
                        }
                        else
                        {
                            ImageID = objbll.ImagesInsert(bkid, lesid, ext, 0, description, UserId);
                        }

                        if (ImageID != 0)
                        {

                            string url = folderfulname + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));

                            file.SaveAs(url);
                            imagePath = "B_" + bkid + "\\L_" + lesid + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        }
                        return Json(imagePath, JsonRequestBehavior.AllowGet);
                    }
                    //var fileName = Path.GetFileName(file.FileName);

                    //var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    //file.SaveAs(path);
                }
                return Json(imagePath, JsonRequestBehavior.AllowGet);
            }
            return Json("Upload Fail! Try Again Later", JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public ActionResult UploadVideos(string value, int bkid, int lesid, int chpid)
        //{
        //    try
        //    {
        //        int UserId = SessionManager.Current.UserID;
        //        var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
        //        var ext = 0;

        //        ImagesManager objbll = new ImagesManager();


        //        if (Request.Files != null)
        //        {


        //            foreach (string img in Request.Files)
        //            {

        //                HttpPostedFileBase file = Request.Files[img];

        //                var filesize = file.ContentLength;

        //                if (filesize < 17825792)
        //                {
        //                    var fol = mainpath + "B_" + bkid + "\\L_" + lesid;

        //                    var folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
        //                    //var folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\" + "\\L_" + lesid);
        //                    //var fol = mainpath + "B_" + bkid + "\\C_" + chpid + "\\L_" + lesid;

        //                    //var folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\C_" + chpid + "\\" + "\\L_" + lesid);

        //                    var folderfulname = @"" + folder.FullName;

        //                    if (folder.Exists == false)
        //                    {
        //                        System.IO.Directory.CreateDirectory(fol);
        //                    }

        //                    string vr = file.FileName.Substring(file.FileName.LastIndexOf("."));


        //                    if (vr == ".mp4" || vr == ".MP4")
        //                    {
        //                        ext = 8;
        //                    }

        //                    else if (vr == ".mov" || vr == ".MOV")
        //                    {
        //                        ext = 9;
        //                    }

        //                    else if (vr == ".avi" || vr == ".AVI")
        //                    {
        //                        ext = 10;
        //                    }



        //                    else
        //                    {
        //                        return Json("Format Not Supported", JsonRequestBehavior.AllowGet);
        //                    }

        //                    int ImageID = objbll.ImagesInsert(bkid, lesid, ext, chpid, value,UserId);

        //                    if (ImageID != 0)
        //                    {

        //                        string url = folderfulname + "\\" + ImageID + file.FileName.Substring(file.FileName.LastIndexOf("."));

        //                        file.SaveAs(url);
        //                    }
        //                }

        //                else
        //                {
        //                    return Json("File Size Greater than 5MB", JsonRequestBehavior.AllowGet);
        //                }


        //            }

        //        }
        //        return Json("images", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }

        //}


        public ActionResult GetAudio(int bkid, int lid)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.BookID = bkid;
                ViewBag.LessonID = lid;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult GetScriptAudio(string LessonPlanID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.LessonPlanID = LessonPlanID;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult GetVideo(int bkid, int lid)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.BookID = bkid;
                ViewBag.LessonID = lid;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult ShowImages(string ImageSrc,int mediaType)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.ImageSrc = ImageSrc;
                ViewBag.MediaType = mediaType;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult GetDeleteImages(int ImageID,int LessonId = 0, string EncID = null)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.ImageID = ImageID;
                bool canDelete = true;
                canDelete = new ImagesManager().CanDeleteImage(LessonId, EncID);
                ViewBag.LessonId = LessonId;
                ViewBag.CanDelete = canDelete;
                return PartialView();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpPost]
        public JsonResult DeleteImages(int Id)
        {
            try
            {
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                        SessionManager.Current.UserID, 1, "Delete Images with" + Id + " on" + DateTime.Now + ".", this.Request.UserHostAddress
                       );
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "ImageController");
            }
            int i = 0;
            i = new ImagesManager().DeleteImages(Id);

            return Json(i);
        }

        [HttpPost]
        public ActionResult SelectImages(int bkid, bool All = false)
        {

            try
            {
                ImagesManager objbll = new ImagesManager();
                var ctx = objbll.ImagesSelect(bkid, 1, All);

                return Json(ctx, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SelectSounds(int LessonID = 0, bool All = false)
        {

            try
            {
                ImagesManager objbll = new ImagesManager();
                var ctx = objbll.ImagesSelect(LessonID, 2, All);

                return Json(ctx, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SelectVideos(int LessonID = 0, bool All = false)
        {
            try
            {
                ImagesManager objbll = new ImagesManager();
                var ctx = objbll.ImagesSelect(LessonID, 3, All);

                return Json(ctx, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetExtensionSize(int MediaTypeID = 0)
        {
            var ctx = new ImagesManager().MediaExtionSize(MediaTypeID);

            if (ctx != null)
            {
                return Json(ctx, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}
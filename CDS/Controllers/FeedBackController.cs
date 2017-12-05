using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using LEAF_Logic;

namespace CDS.Controllers
{
    public class FeedBackController : Controller
    {
        // GET: FeedBack
        public ActionResult UserFeedBack()
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (SessionManager.Current.UserID != 0)
                {
                    List<string> imgList = new List<string>();
                    imgList = (List<string>)Session["FeedBackImages"];
                    foreach (string img in imgList)
                    {
                        FileInfo file = new FileInfo(img);
                        if (file.Exists == true)
                        {
                            file.Delete();
                        }
                    }
                    Session["FeedBackImages"] = null;
                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult SchoolFeedBack()
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (Session["FeedBackImages"] != null)
                {
                    List<string> imgList = new List<string>();
                    imgList = (List<string>)Session["FeedBackImages"];
                    foreach (string img in imgList)
                    {
                        FileInfo file = new FileInfo(img);
                        if (file.Exists == true)
                        {
                            file.Delete();
                        }
                    }
                    Session["FeedBackImages"] = null;
                }
                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult FeedBack()
        {
            if (SessionManager.Current.UserID != 0)
            {
                    ViewBag.ViewName = "Feed Back";
                    return View(new Mngr_FeedBack().GetFeedBackList(SessionManager.Current.UserID.ToString()));
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult LoadFeedBack(bool isSchoolFeedBack = false, string FeedBackGroupID = null)
        {
            if (SessionManager.Current.UserID != 0)
            {
                mdl_FeedBack objbll = new mdl_FeedBack();

                if (FeedBackGroupID != null)
                {
                    FeedBackGroupID = EncyptionDcryption.GetDecryptedText(FeedBackGroupID);
                }
                objbll.feedBackType = new Mngr_FeedBack().DropDownType(FeedBackGroupID);

                objbll.feedBackFrequency = new Mngr_FeedBack().DropDownFrequency();
                if (isSchoolFeedBack)
                {
                    objbll.isSchoolFeedBack = true;
                }
                return PartialView("_FeedBackPartial", objbll);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult SaveFeedBack(string title, string remarks)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    int userID = SessionManager.Current.UserID;
                    List<mdl_FeedBack> Select = new List<mdl_FeedBack>();
                    Select = new Mngr_FeedBack().AddFeedBack(title, remarks, userID);

                    if (Select != null)
                    {
                        if (Select.ToList()[0].ScopeIdentity != 0)
                        {
                            int id = Select.ToList()[0].ScopeIdentity;
                            if (Session["FeedBackImages"] != null)
                            {
                                List<string> imgList = new List<string>();
                                List<string> newimgList = new List<string>();
                                imgList = (List<string>)Session["FeedBackImages"];
                                int count = 1;
                                foreach (string img in imgList)
                                {
                                    string all = string.Concat(id, "_", count);
                                    newimgList.Add(all);
                                    count++;
                                }
                                string images = string.Join(",", newimgList.ToArray());
                                bool isImageSave = new Mngr_FeedBack().AddFeedBackImages(images, id);
                                var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
                                var fol = "";
                                fol = mainpath + "Picture\\FeedBack";
                                string newpath = null;
                                int newcount = 0;
                                foreach (string img in imgList)
                                {
                                    newpath = fol + "\\" + newimgList.ToList()[newcount] + ".jpg";
                                    System.IO.File.Move(img, newpath);
                                    newcount++;
                                }

                                Session["FeedBackImages"] = null;
                            }
                        }
                            return Json(Select, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(Select, JsonRequestBehavior.AllowGet);
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

        public ActionResult FeedBackList(string userID = null)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewFeedbackManagement, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    try
                    {
                        ViewBag.userID = userID;
                        ViewBag.ViewName = "Feedback Management";

                        List<mdl_FeedBack> FeedBackList = new List<mdl_FeedBack>();
                        mdl_FeedBack objbll = new mdl_FeedBack();
                        FeedBackList = new Mngr_FeedBack().GetFeedBackList(userID);
                        return View(FeedBackList);
                    }
                    catch (Exception ex)
                    {
                        new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                        return View();
                    }

                }
                else {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }


        public ActionResult FeedBackProcess(string EncFeedBackId, string returnPage = "", string isView = "w1KSdRHV15E=", string isRouteTo = "w1KSdRHV15E=")
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    ViewBag.ViewName = "Feedback Process";
                    int DecFeedBackId = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(EncFeedBackId));
                    bool DecIsView = Convert.ToBoolean(EncyptionDcryption.GetDecryptedText(isView));
                    bool DecIsRouteTo = Convert.ToBoolean(EncyptionDcryption.GetDecryptedText(isRouteTo));

                    mdl_FeedBack objbll = new Mngr_FeedBack().GetFeedBackProcess(DecFeedBackId);

                    var status = new Mngr_FeedBack().DropDownStatus();
                    ViewBag.statusdropdown = status;

                    var dept = new Mngr_FeedBack().DropDownDepartment();
                    ViewBag.departmentdropdown = dept;

                    if (DecIsView)
                        ViewBag.isView = 1;
                    else
                        ViewBag.isView = 0;
                    if (DecIsRouteTo)
                        ViewBag.isRouteTo = 1;
                    else
                        ViewBag.isRouteTo = 0;

                    ViewBag.returnPage = returnPage;
                    ViewBag.CommentUserID = SessionManager.Current.UserID;
                    ViewBag.CommentUserName = SessionManager.Current.UserName;
                    ViewBag.Date = DateTime.Now;
                    return View(objbll);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult FeedBackReAllocate(string EncFeedBackId)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    Mngr_FeedBack objbll = new Mngr_FeedBack();
                    int DecFeedBackId = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(EncFeedBackId));
                    var dept = new Mngr_FeedBack().DropDownDepartment();
                    ViewBag.departmentdropdown = dept;

                    return View(objbll.GetFeedBackProcess(DecFeedBackId));
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonController");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult UpdateFeedBack(int status, string comment, int feebbackId)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    Mngr_FeedBack objbll = new Mngr_FeedBack();
                    int userID = SessionManager.Current.UserID;
                    bool isUpdate = objbll.UpdateFeedBack(status, comment, userID, feebbackId);
                    if (isUpdate)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        public ActionResult AddComment(string comment, int feebbackId)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    int userID = SessionManager.Current.UserID;
                    bool isUpdate = new Mngr_FeedBack().AddFeedBackComment(comment, userID, feebbackId);
                    if (isUpdate)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
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



        public ActionResult ReAllocateFeedBack(int departmentID, string comment, int feebbackId, int prevDeptID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    int userID = SessionManager.Current.UserID;
                    bool isUpdate = new Mngr_FeedBack().ReAllocateFeedBack(departmentID, comment, feebbackId, prevDeptID, userID);
                    if (isUpdate)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
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

        public ActionResult DeleteFeedBackComment(int commentID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    Mngr_FeedBack objbll = new Mngr_FeedBack();
                    int userID = SessionManager.Current.UserID;
                    bool isUpdate = objbll.DeleteFeedBackComment(commentID);
                    if (isUpdate)
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public ActionResult Upload()
        {
            var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
            var ext = 0;
            List<string> imagePath = new List<string>();
            List<string> imagePathsession = new List<string>();
            mdl_FeedBackImages objbll = new mdl_FeedBackImages();
            if (Request.Files != null)
            {
                if (Session["FeedBackImages"] != null)
                {
                    List<string> imgList = new List<string>();
                    imgList = (List<string>)Session["FeedBackImages"];
                    foreach (string img in imgList)
                    {
                        FileInfo file = new FileInfo(img);
                        if (file.Exists == true)
                        {
                            file.Delete();
                        }
                    }
                    Session["FeedBackImages"] = null;
                }
                string pathtoImage = null;
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    var filesize = file.ContentLength;
                    if (filesize < 12582912)
                    {
                        var fol = "";
                        DirectoryInfo folder = null;
                        fol = mainpath + "Picture\\FeedBack";
                        folder = new DirectoryInfo(mainpath + "Picture\\FeedBack");

                        var folderfulname = @"" + folder.FullName;
                        if (folder.Exists == false)
                        {
                            System.IO.Directory.CreateDirectory(fol);
                        }
                        pathtoImage = folderfulname;
                        string vr = file.FileName.Substring(file.FileName.LastIndexOf("."));

                        if (vr == ".jpg" || vr == ".JPG")
                        {
                        }
                        else
                        {
                            return Json("Format Not Supported", JsonRequestBehavior.AllowGet);
                        }

                        long ticks = DateTime.Now.Ticks;
                        byte[] bytes = BitConverter.GetBytes(ticks);
                        string datediff = Convert.ToBase64String(bytes)
                                                .Replace('+', '_')
                                                .Replace('/', '-')
                                                .TrimEnd('=');


                        string url = folderfulname + "\\" + "temp_" + datediff + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        string imgPth = "Picture\\FeedBack" + "\\" + "temp_" + datediff + file.FileName.Substring(file.FileName.LastIndexOf("."));
                        imagePath.Add(imgPth);
                        imagePathsession.Add(url);
                        file.SaveAs(url);
                    }

                }
                DeleteOldImages(pathtoImage);
                Session["FeedBackImages"] = imagePathsession;
                return Json(imagePath, JsonRequestBehavior.AllowGet);
            }
            return Json("Upload Fail! Try Again Later", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteImage()
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    if (Session["FeedBackImages"] != null)
                    {
                        List<string> imgList = new List<string>();
                        imgList = (List<string>)Session["FeedBackImages"];
                        foreach (string img in imgList)
                        {
                            FileInfo file = new FileInfo(img);
                            if (file.Exists == true)
                            {
                                file.Delete();
                            }
                        }
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
    }
}
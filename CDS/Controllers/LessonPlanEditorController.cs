using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using LEAF_Logic;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class LessonPlanEditorController : Controller
    {
        [HttpGet]
        public ActionResult LessonPlanEnglish()
        {
            return RedirectToAction("LoginUser", "User");
        }
        [HttpGet]
        public ActionResult LessonPlanUrdu()
        {
            return RedirectToAction("LoginUser", "User");
        }
        [HttpGet]
        public ActionResult ReadModeEnglish()
        {
            return RedirectToAction("LoginUser", "User");
        }
        [HttpGet]
        public ActionResult LessonPlan(string LessonId = null, string mode = null, string InvitaionID = null,string InvitationUserID=null,string VerifiedUserID=null)
        {
                Session["LessonId"] = LessonId;
                Session["LessonMode"] = mode;
                Session["LessonInvitaionID"] = InvitaionID;
                Session["LessonInvitationUserID"] = InvitationUserID;
                Session["LessonVerifiedUserID"] = VerifiedUserID;
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.Mode = mode;
                int isinvitationid = 0;
                if (InvitaionID != null)
                {
                    ViewBag.CommentID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(InvitaionID));
                }
                else
                {
                    ViewBag.CommentID = 0;
                }
                try
                {
                    if (LessonId != null && LessonId != "0" && LessonId != "")
                    {
                        if (LessonId.StartsWith("'") && LessonId.EndsWith("'"))
                        {
                            LessonId = LessonId.Replace("'", "");
                        }
                        bool isnumber = CommonMng.IsNumeric(LessonId);
                        int ID = 0;
                        if (isnumber)
                        {
                            ID = Convert.ToInt32(LessonId);
                        }
                        else
                        {
                            ID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonId));
                        }
                        int uid = 0;
                        if (InvitationUserID == null && VerifiedUserID == null)
                        {
                            uid = SessionManager.Current.UserID;
                        }
                        else if (InvitationUserID != null)
                        {
                            isinvitationid = 1;
                            uid = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(InvitationUserID));
                        }
                        else if (VerifiedUserID != null)
                        {
                            isinvitationid = 1;
                            uid = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(VerifiedUserID));
                        }
                        if (ID != 0)
                        {
                            try
                            {
                                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                                        SessionManager.Current.UserID, 1, "Final Lesson with book type " + 1 + " with Id:" + ID + " is being open on " + DateTime.Now + ".", this.Request.UserHostAddress
                                       );
                            }
                            catch (Exception ex)
                            {
                                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                            }
                            DataSet ds = null;
                            ViewBag.ViewName = "Lesson Plan Editor";
                            ds = new LessonInfoManager().GetLessonDataForEdit(ID, uid,isinvitationid);
                            LessonInfo ltp = GetLessonData(ds, ID, uid);
                            int CheckLessonUser = new LessonInfoManager().CheckLessonUser(ID,Convert.ToInt32(ltp.LessonPlanIDDecr), SessionManager.Current.UserID);
                                ViewBag.Mode = mode;
                                Session["SelectedLessonPlanId"] = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(ltp.LessonPlanID));
                                Session["CurrentBookTypeID"] = 1;
                            var obj = new LessonInfoManager().ResetLessonPlanLock(Convert.ToInt32(ltp.LessonPlanIDDecr), SessionManager.Current.UserID, 1);
                            ltp.LockedBy = obj.LockedBy;
                            ltp.IsLockByVerifier = obj.IsLockByVerifier;
                            ltp.VerifiedLockTrackId = obj.VerifiedLockTrackId;
                            if (ltp.LanguageID == 1)
                                {
                                    ViewBag.OwnerID = ltp.OwnerId;

                                if (ltp.LessonPlanStatus == 3 || CheckLessonUser == 1 || mode == "Verify")
                                {
                                    return View("ReadModeEnglish", ltp);
                                }
                                else
                                {
                                    if (CheckLessonUser > 0)
                                    {
                                        return View("LessonPlanEnglish", ltp);
                                    }
                                    else
                                    {
                                        ViewBag.Name = "Lesson";
                                        return View("~/Views/Shared/Authurise.cshtml");
                                    }
                                }
                            }
                                else
                                {
                                    return View("LessonPlanUrdu", ltp);
                                }

                        }
                        else
                        {
                            return RedirectToAction("LoginUser", "User");
                        }
                    }
                    else
                    {
                        return RedirectToAction("LoginUser", "User");
                    }
                }
                catch (Exception ex)
                {
                    new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditor Controller");
                    return View();
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        private LessonInfo GetLessonData(DataSet ds, int ID, int userID)
        {
            LessonInfo ltp = new LessonInfo();
            ltp._booksTypes = new BookManager().GetBookTypeByBook(ltp.BookID);
            ltp.selectedbookoption = "jOMmt2GbRBY=";
            ltp.TotalLessonofBook = new LessonInfoManager().GetTotalBookLesson(ltp.BookID, "Lesson", userID, 1);
            ltp.BookTypeID = 1;
            ltp = new LessonInfoManager().GetLessonHeader(ds.Tables[0]);
            int LessonPlanID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(ltp.LessonPlanID));
            ltp.sections = new LessonInfoManager().GetLessonSections(ds.Tables[1]);
            ltp.subsections = new LessonInfoManager().GetLessonSubSections(ds.Tables[2]);
            ltp.VocabloryCount = CommonMng.Count_KeyVocabulary(ltp.AllScript, ltp.subsections.Where(x => x.SubSectionId == 2).FirstOrDefault().LessonPlanScript, ',');
            ltp.lstComments = new LessonInfoManager().CommentsSectionByLessonID(ID, LessonPlanID, SessionManager.Current.UserID);
            ltp.Rating = new LessonInfoManager().GetLessonRating(LessonPlanID, SessionManager.Current.UserID);
            ltp.lstLessonUsers = new LessonInfoManager().GetUserWorkingOnLesson(ID, LessonPlanID, SessionManager.Current.UserID);
            ltp.LessonUserAccess = new LessonInfoManager().GetLessonRights(SessionManager.Current.UserID, LessonPlanID, ID);
            ltp.LessSourinfo= new LessonInfoManager().GetLessonSourceInfo(LessonPlanID);
            var ctximg = ltp.ListImages = new LessonInfoManager().ImagesSelect(ID, 1, userID);
            var ctxsound = ltp.ListSounds = new LessonInfoManager().ImagesSelect(ID, 2, userID);
            var ctxvideos = ltp.ListVideos = new LessonInfoManager().ImagesSelect(ID, 3, userID);
            ltp.lstTemplates = new TemplateManager().GetLessonTemplates(ID, SessionManager.Current.UserID);
            ltp.LessonPlanIDDecr = Convert.ToString(EncyptionDcryption.GetDecryptedText(ltp.LessonPlanID));
            if (ltp.subsections != null)
            {
                int secid = ltp.sections[0].Id;
                ltp.SelectedLesspnPlanSection = ltp.subsections.Where(x => x.SectionId == secid).FirstOrDefault().LessonPlanScriptId;
                ltp.LogId = new LessonInfoManager().ManageLessonTime(0, userID, Convert.ToInt32(ltp.LessonPlanIDDecr), ltp.SelectedLesspnPlanSection);
            }
            ltp.ScriptAudioPath = new LessonInfoManager().CheckScriptAudio(ltp.LessonPlanIDDecr);
            if (!string.IsNullOrEmpty(ltp.ScriptAudioPath))
            {
                string dummypath = "B_" + ltp.BookID + "\\L_" + ID + "\\" + ltp.ScriptAudioPath;
                byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(dummypath);
                ltp.ScriptAudioPath = Convert.ToBase64String(imgEncrypt);

                ltp.isScriptAudio = true;
            }
            if (ctximg != null)
            {
                foreach (var itemimg in ctximg)
                {
                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(itemimg.ImageName);
                    var xy = itemimg.ImageName = Convert.ToBase64String(imgEncrypt);
                }
            }
            if (ctxsound != null)
            {
                foreach (var itemsound in ctxsound)
                {
                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(itemsound.ImageName);
                    var xy = itemsound.ImageName = Convert.ToBase64String(imgEncrypt);
                }
            }

            if (ctxvideos != null)
            {
                foreach (var itemvideos in ctxvideos)
                {
                    byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes(itemvideos.ImageName);
                    var xy = itemvideos.ImageName = Convert.ToBase64String(imgEncrypt);
                }
            }
            return ltp;
        }
        public ActionResult GetLessonRights(int userID,int lessonPlanID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                List<LessonUser> LessonUserObj = null;
                if (userID != 0)
                {
                    LessonUserObj = new LessonInfoManager().GetLessonRights(userID, lessonPlanID);
                    ViewBag.UserID = userID;
                    ViewBag.LessonPlanID = lessonPlanID;
                }
                return PartialView(LessonUserObj);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpPost]
        public ActionResult Get_Msg(int LessonID, string BookTypeId, string ShowArchive, int isIndexPage = 0, bool isSorted = false, int LessonPlanID = 0)
        {
            if (SessionManager.Current.UserID!=0)
            {
                try
                {
                    return Json(new LessonInfoManager().CommentsSectionByLessonID(LessonID,LessonPlanID, SessionManager.Current.UserID), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult GetImages(int ID, int ImageType, bool userID)
        {
            List<LessonImages> LessonImagesList = new List<LessonImages>();
            int UID = 0;
            if (userID == false)
            {
                UID = SessionManager.Current.UserID;
            }
            LessonImagesList = new LessonInfoManager().ImagesSelect(ID, ImageType, UID,false,1);
            if (LessonImagesList == null)
            {
                LessonImagesList = new List<LessonImages>();
            }
            return Json(LessonImagesList, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult LessonList(int SelectTab=1)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewLessonScript, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    ViewBag.ViewName = "My Lessons";
                    ViewBag.SelectedTab = SelectTab;
                    int ID = SessionManager.Current.UserID;
                    ListLessonInfo info = new LessonInfoManager().GetLessonPlanList(ID, SelectTab);
                    return View(info);
                }
                else {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpPost]
        public ActionResult UpdateUserLessonRights(int userID,int lessonPlanID,string[] lst = null,bool isRevoke = false)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (isRevoke)
                {
                    new LessonInfoManager().UpdateUserLessonRights(0,userID, 0, lessonPlanID, "",isRevoke);
                    return Json("Done", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (lst != null)
                    {
                        int a = 1;
                        foreach (string item in lst)
                        {
                            new LessonInfoManager().UpdateUserLessonRights(Convert.ToInt32(item), userID, a, lessonPlanID, "selection");
                            a = 2;
                        }
                        return Json("Done", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        new LessonInfoManager().UpdateUserLessonRights(0, userID, 0, lessonPlanID, "no selection");
                        return Json("Done", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveData(int LessonPlanScriptId = 0, string Data = null, int trackid = 0, string LessonPlanID = null)
        {
            if (SessionManager.Current.UserID != 0)
            {
                int status = 0;
                int Lpid = 0;
                if (LessonPlanID != null)
                {
                    Lpid = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                int userid = SessionManager.Current.UserID;
                try
                {
                    string divchk = "<div id=";
                    while (Data.Contains(divchk))
                    { Data = Data.Replace(Data.Substring(Data.IndexOf(divchk), Data.Substring(Data.IndexOf(divchk)).IndexOf('>') + 1), "><div>"); }
                    Data = Data.Replace("img-responsive", " ");
                    Data = Data.Replace("UWR_UrduFont", " ");
                    Data.Replace("scriptid_", " ");
                    if ((Data.Replace("<br />", "").Replace(" ", "")).Length == 0)
                    {
                        Data = null;
                    }
                    status = new LessonInfoManager().UpdateLessonPalnScript(LessonPlanScriptId, Data, userid, Lpid, trackid);
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                                  userid, 1, "Data Script with Id:" + LessonPlanScriptId + " is being updated on " + DateTime.Now + ".", this.Request.UserHostAddress);
                    if (status != -1)
                    {
                        SaveData obj = new SaveData();
                        obj.SData = Data;
                        obj.Trackingid = status;
                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json(status, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Login", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult ManageLessonTime(int LogId, Int64 LessonPalnID, int LesPlansecID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                int ID = new LessonInfoManager().ManageLessonTime(LogId, SessionManager.Current.UserID, LessonPalnID, LesPlansecID);
                return Json(ID, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Login", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RestoreSectionHistory(Int64 LogId = 0)
        {
            try
            {
                if (SessionManager.Current.UserID != 0)
                {
                    if (LogId != 0)
                    {
                        int userid = SessionManager.Current.UserID;
                        string data = new LessonInfoManager().RestoreSectionData(LogId, userid);
                        if (data != null)
                        {
                            return Json(data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(null, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return RedirectToAction("LoginUser", "User");
                }
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult VerifyLessonPlan(int LessonId, int BooktypeID, int isindexPage = 0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                string result = null;
                int userid = SessionManager.Current.UserID;
                string logtext = null;
                string txt = null;
                try
                {
                    result = new LessonInfoManager().CompleteLessonPlanMaster(LessonId, userid, BooktypeID, isindexPage);
                    if (isindexPage == 1)
                    {
                        txt = "Book Extra Page with id=";
                    }
                    else
                    {
                        txt = "Lesson with id=";
                    }
                    logtext = txt + LessonId + " with booktype id=" + BooktypeID + " Verified by by user " + userid + " on " + DateTime.Now + ".";
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                              SessionManager.Current.UserID, 1, logtext, this.Request.UserHostAddress
                             );
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json(result);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public JsonResult DeleteLesson(string Val)
        {
            string result = new LessonInfoManager().DeleteLesson(Val);
            if (result != null)
                return Json(result, JsonRequestBehavior.AllowGet);
            else
                return Json(0, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetTemplateDataByID(int ID,int LessonID)
        {
            if (SessionManager.Current.UserID!=0)
            {
                if (ID!=0)
                {
                    TemplateModel obj = new TemplateModel();
                    DataSet ds = null;
                    ds = new TemplateManager().GetTemplateDetails(ID, LessonID);
                    if (ds != null)
                    {
                        obj.obj = new TemplateDetail();
                        obj.obj.lstSection = new LessonInfoManager().GetLessonSections(ds.Tables[1]);
                        obj.obj.lstSubsection = new LessonInfoManager().GetLessonSubSections(ds.Tables[2]);
                        return Json(obj, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnderVerificationLessons()
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanVerifiyLesson, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    ViewBag.ViewName = "Submitted Lessons";
                    ListLessonInfo info = new LessonInfoManager().UnderVerificationLessons(SessionManager.Current.UserID);
                    return View(info);
                }
                else
                {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult GetLessonHistory(int LessonPlanID) {
            ListLessonInfo obj = new ListLessonInfo();
            obj= new LessonInfoManager().GetLessonHistory(LessonPlanID);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RejectLesson(string LessonPlanID,int LessonID,int OwnerID,string Masg)
        {
            if (SessionManager.Current.UserID != 0 && LessonPlanID!=null && LessonID!=0 && OwnerID!=0 && Masg!=null)
            {
                try
                {
                    int ID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                    new CommentController().AddComment(LessonID, SessionManager.Current.UserID, Masg, null, null, ID, OwnerID,0,1);
                    new LessonInfoManager().ModifyLesson(ID, Convert.ToInt32(SessionManager.Current.UserID), Masg);
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                                SessionManager.Current.UserID, 1, "Lesson Send back to Editor where LessonPlanId=" + ID, this.Request.UserHostAddress
                               );
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditor Controller");
                    return Json(null, JsonRequestBehavior.AllowGet);
                }

            }
            else {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            }
        public ActionResult VerifyLesson(string LessonPlanID,int LessonID, int OwnerID)
        {
            if (SessionManager.Current.UserID != 0 && LessonPlanID != null && LessonID != 0 && OwnerID != 0)
            {
                int ID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                string Masg = "Congratulations Your Lesson Approved by " + SessionManager.Current.UserName;
                new CommentController().AddComment(LessonID, SessionManager.Current.UserID, Masg, null, null, ID, OwnerID,1,0);
                int VerifiedID=new LessonInfoManager().VerifyLesson(ID,Convert.ToInt32(SessionManager.Current.UserID));
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                             SessionManager.Current.UserID, 1, "Lesson Verify where LessonPlanId="+ID, this.Request.UserHostAddress
                            );
                return Json(VerifiedID, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VerifiedLessonList(int SelectedTab=1) {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewLessonVerifiedList, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    ViewBag.SelectedTab = SelectedTab;
                    ViewBag.ViewName = "Approved Lessons";
                    ListLessonInfo info = new LessonInfoManager().VerifiedLessonList(SelectedTab, SessionManager.Current.UserID);
                    return View(info);
                }
                else
                {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        public ActionResult RateLesson(string LessonPlanID,string Rating,int VerifiedID)
        {
            if (SessionManager.Current.UserID != 0)
            {

                int ID = 0;
                if (CommonMng.IsNumeric(LessonPlanID))
                {
                    ID = Convert.ToInt32(LessonPlanID);
                }
                else
                {
                    ID= Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                }

                new LessonInfoManager().RateLesson(ID,Convert.ToInt32(Rating),Convert.ToInt32(SessionManager.Current.UserID),VerifiedID);
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                             SessionManager.Current.UserID, 1, "Lesson Rated where LessonPlanId=" + ID, this.Request.UserHostAddress
                            );
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        public ActionResult VerifiedLesson() {

            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewVerifiedLessonList, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    ViewBag.ViewName = "All Approved Lesson";
                    ListLessonInfo info = new LessonInfoManager().VerifiedLesson(SessionManager.Current.UserID);
                    return View(info);
                }
                else
                {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        public ActionResult ModifyApprovedLesson(int LessonPlanID, string LessonID) {

            int lsnid= Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonID));

            int message = new LessonInfoManager().ModifyApprovedLesson(LessonPlanID, lsnid,Convert.ToInt32(SessionManager.Current.UserID));

            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ResetLock(int ID)
        {
            LessonLock obj=null;
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    obj = new LessonInfoManager().ResetLessonPlanLock(ID, SessionManager.Current.UserID, 0);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");

                }
                return Json(obj, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult UpdateVerifiedTracking(int ID)
        {
            if (ID != 0)
            {
                try
                {
                   new LessonInfoManager().UpdateVerifiedTracID(ID);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");

                }
                return Json(1, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult UpdateLock(int ID=0,int VerifiedLockTrackid=0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                   new LessonInfoManager().updateLock(ID, SessionManager.Current.UserID,VerifiedLockTrackid);
                    return Json("Done", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult RealeseLessonLock(int ID=0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                   new LessonInfoManager().RealeseLessonLock(ID);
                    return Json("Done", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpGet]
        public ActionResult GrtTemplateRating(int ID = 0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    var obj = new TemplateManager().GetTemplateRating(ID);
                    if (obj!=null)
                    {
                        obj[0].CanRate = obj.Where(x => x.RatedBy == SessionManager.Current.UserID).FirstOrDefault().RatedBy;
                    }
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditorController");
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveHttpImage(string data1 = null, int lessonplansectionid = 0, int bkid = 0, int lesid = 0, string LessonPlanID = null)
        {
            if (SessionManager.Current.UserID != 0)
            {
                int Lpid = 0;
                if (LessonPlanID != null)
                {
                    Lpid = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonPlanID));
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                int userid = SessionManager.Current.UserID;
                string substr = "<img ", subStr1 = "src=\"";
                string str = "";
                int fileID = 0;
                byte[] ImgByte = null;
                string baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/";
                for (int i = 0; i < data1.Length - (substr.Length - 1); i = i + 1/*substr.Length*/)
                {
                    str = data1.Substring(i, substr.Length);
                    if (substr == str)
                    {
                        string keyValue = data1.Substring(i, data1.Substring(i).IndexOf(">") + 1);
                        string src = keyValue.Substring(keyValue.IndexOf(subStr1), keyValue.Substring(keyValue.IndexOf(subStr1) + subStr1.Length).IndexOf("\"") + subStr1.Length + 1);

                        if (src.ToLower().Contains("http") && !src.ToLower().Contains("cds")  && !src.ToLower().Contains("handler"))
                        {
                            src = src.Replace(subStr1, "");
                            src = src.Substring(0, src.Length - 1);
                            try
                            {
                                WebClient proxy = new WebClient();
                                ImgByte = proxy.DownloadData(src);
                                var mainpath = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"];
                                var ext = 2;
                                ImagesManager objbll = new ImagesManager();
                                var fol = "";
                                DirectoryInfo folder = null;
                                fol = mainpath + "B_" + bkid + "\\L_" + lesid;
                                folder = new DirectoryInfo(mainpath + "B_" + bkid + "\\L_" + lesid);
                                var folderfulname = @"" + folder.FullName;
                                if (folder.Exists == false)
                                {
                                    System.IO.Directory.CreateDirectory(fol);
                                }
                                fileID = objbll.ImagesInsert(bkid, lesid, ext, 0, "",userid);
                                string path = fol + "\\" + fileID + ".JPG";
                                System.IO.File.WriteAllBytes(path, ImgByte);
                                byte[] imgEncrypt = UTF8Encoding.UTF8.GetBytes("B_" + bkid + "\\L_" + lesid + "\\" + fileID + ".JPG");
                                var xy = Convert.ToBase64String(imgEncrypt);
                                string newsrc = "..\\Handler\\ImagesHandler.ashx?from=false&&key=" + xy;
                                string NewkeyValue = keyValue.Replace(src, newsrc).Replace("<img ", "<img id=\"image_" + fileID + "\" onclick=\"ShowImages(" + fileID + ",1)\" ");
                                data1 = data1.Replace(keyValue, NewkeyValue);
                                //data1.Replace(src, newsrc);
                            }
                            catch (Exception ex)
                            {
                                return Json(null, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                }
                int status = new LessonInfoManager().UpdateLessonPalnScript(lessonplansectionid, data1, userid, Lpid, 0);
                if (status == 0)
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                return Json(data1, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using LEAF_Logic;

namespace CDS.Controllers
{
    public class CommentController : Controller
    {

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddComment(int LessonID, int userid, string Comment, string Tagid = null,string TagEmail = null,int LessonPlanID =0,int OwnerID =0,int isverified=0,int IsReject=0, int secId = 0, string selectedtext = null)
        {

            CommentManager objbll = new CommentManager();
            string ID = null;
            if (!IsNumeric(LessonID))
            {
                ID = EncyptionDcryption.GetEncryptedText(LessonID.ToString());
            }
            else
            {
                ID = LessonID.ToString();
            }
            string baseUrl = null;
            //if (BookTypeID > 1)
            //{
            //    if (isindexpage == 0)
            //    {
            //        baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/BookComposing/BookEditor?LessonId=" + ID + "&&BookTypeID=" + EncyptionDcryption.GetEncryptedText(BookTypeID.ToString()) + "&& ViewMode='Standerd View'";
            //    }
            //    else
            //    {
            //        baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/BookPages/BookIndexing?PageID=" + ID + "&&BookID=" + BookID + "&&BookTypeID=" + EncyptionDcryption.GetEncryptedText(BookTypeID.ToString());
            //    }

            //}
            //else
            //{
            //    baseUrl = System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Authority + System.Web.HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/LessonPlanEditor/LessonPlan?LessonId=" + ID + "&&BookTypeID=1";
            //}

            baseUrl = CommonMng.baseUrl + "/LessonPlanEditor/LessonPlan?LessonId=" + ID + "&&BookTypeID=1";

            if (Tagid != null && Tagid.Trim().Length > 0)
            {
                Tagid = Tagid.Substring(0, Tagid.Trim().Length - 1);
            }

            else
            {
                Tagid = "";
            }

            if(TagEmail != null && TagEmail.Trim().Length > 0)
            {
                TagEmail = TagEmail.Substring(0, TagEmail.Trim().Length - 1);
            }

            else
            {
                TagEmail = "";
            }

            //string url = "<a href = '" + baseUrl + "' > Click here to review this comment.</a>";

            if (OwnerID != 0)
            {
                Tagid = OwnerID.ToString();
            }

            int id = objbll.CommentInsert(LessonID, userid, Comment, baseUrl, Tagid, TagEmail,LessonPlanID,isverified,IsReject, secId, selectedtext);
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddScopeComment(string LessonID, string Comment,int ReceiverID)
        {
            CommentManager objbll = new CommentManager();
            int ID = 0;
            if (!string.IsNullOrEmpty(LessonID))
            {
                ID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonID));
            }

            int id = objbll.ScopeCommentInsert(ID, Comment,SessionManager.Current.UserID,ReceiverID);
            return Json(id, JsonRequestBehavior.AllowGet);
        }


        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }


        [HttpGet]
        public ActionResult GetUsers()
        {

            var usid = 0;
            if (SessionManager.Current.UserID != 0)
            {
                usid = SessionManager.Current.UserID;
            }

            var ctx = new CommentManager().Get_AllUserListForTagging(usid, "0");

            return Json(ctx, JsonRequestBehavior.AllowGet);

        }



        public ActionResult CommentInbox() {

            if (SessionManager.Current.UserID != 0)
            {

                    ViewBag.ViewName = "Inbox";
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()), SessionManager.Current.UserID, 1, "Access User Inbox Page" + DateTime.Now + ".", this.Request.UserHostAddress);
                    var List = new CommentManager().CommentInbox(SessionManager.Current.UserID, SessionManager.Current.EntityID);
                    return View(List);

            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult ChangeReadStatus(int LessonPlanID)
        {
            BLL_ChatHistory ctx = null;
            if (SessionManager.Current.UserID != 0)
            {
                ctx = new CommentManager().ChangeReadStatus(LessonPlanID,SessionManager.Current.UserID);
            }

            return Json(ctx, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CommentScopeAndSequence(string LessonID, string MessageID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                ViewBag.LessonId = LessonID;
                List<BLL_ChatHistory> ChatHistoryObj = null;
                if (LessonID != null)
                {
                    int decLessonID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonID));
                    int decMessageID = 0;
                    if (!string.IsNullOrEmpty(MessageID))
                    {
                        decMessageID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(MessageID));
                        ViewBag.MessageId = MessageID;

                    }
                    else
                    {
                        ViewBag.MessageId = 0;
                    }
                    ChatHistoryObj = new CommentManager().GetScopeAndSequenceComments(decLessonID,SessionManager.Current.UserID,decMessageID);

                }
                return PartialView(ChatHistoryObj);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpPost]
        public ActionResult UpdateInbox()
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    List<BLL_ChatHistory> chatHistoryList = new List<BLL_ChatHistory>();
                    chatHistoryList = new UserManagement().GetMessageNotification(SessionManager.Current.UserID, SessionManager.Current.EntityID);
                    if (chatHistoryList == null)
                    {
                        chatHistoryList = new List<BLL_ChatHistory>();
                    }
                    return Json(chatHistoryList , JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "CommentController");
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        [HttpPost]
        public ActionResult Get_Scope_Msg(string LessonID, string MessageID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    int decLessonID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(LessonID));
                    int decMessageID = 0;
                    if (MessageID != "0")
                    {
                        decMessageID = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(MessageID));
                    }
                    return Json(new CommentManager().GetScopeAndSequenceComments(decLessonID,SessionManager.Current.UserID, decMessageID), JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "CommentController");
                    return Json("-1", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }


    }
}
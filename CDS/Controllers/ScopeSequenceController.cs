using CDS.Manager;
using CDS.Models;
using LEAF_Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class ScopeSequenceController : Controller
    {
        // GET: ScopeSequence
        public ActionResult ScopeSequence(int BookID = 0)
        {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewScopeSequence, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    ViewBag.ViewName = "Scope and Sequence";
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()), SessionManager.Current.UserID, 1, "View ScopeSequence " + DateTime.Now + ".", this.Request.UserHostAddress);
                    ScopeSequenceModel obj = new ScopeSequenceModel();
                    obj.lst = new Mngr_ScopeSequence().UserScopeSequence(SessionManager.Current.UserID);

                    return View(obj);
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

        public ActionResult BookConfig(int BookID,string BookName=null)
        {
            if (SessionManager.Current.UserID != 0 && BookID!=0)
            {
                ViewBag.BookName = BookName;
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()), SessionManager.Current.UserID, 1, "Click On a Book" + DateTime.Now + ".", this.Request.UserHostAddress);
                ScopeSequenceModel obj = new ScopeSequenceModel();
                obj.lst = new Mngr_ScopeSequence().GetUnitLessonInformation(BookID,SessionManager.Current.UserID);
                ViewBag.BookID = BookID;
                return View(obj);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public JsonResult IsUserLessonExists(string LesPlanID)
        {
            try
            {
               int result = new Mngr_ScopeSequence().IsUserLessonExists(LesPlanID, SessionManager.Current.UserID);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                new LEAF_Logic.CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "LessonPlanEditor Controller");
                return Json(-1, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
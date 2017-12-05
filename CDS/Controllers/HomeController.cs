using CDS.Logic;
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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (SessionManager.Current.UserID != 0)
            {
                int ID = SessionManager.Current.UserID;
                int SelectedTab = 0;
                ListLessonInfo info = new LessonInfoManager().GetLessonPlanList(ID,SelectedTab);
                if (new Mngr_Con_Class().GetAllUserSubjects(SessionManager.Current.UserID) == null && (SessionManager.Current.PrivigilesID==1 || SessionManager.Current.PrivigilesID==2))
                {
                    return RedirectToAction("UserSubjects", "User");
                }
                else {

                    if (SessionManager.Current.PrivigilesID == 2 && SessionManager.Current.isMaster!=1)
                    {
                        ViewBag.ViewName = "Under Verification Lesson";
                        return RedirectToAction("UnderVerificationLessons", "LessonPlanEditor");
                    }
                    else if (SessionManager.Current.PrivigilesID == 3 && SessionManager.Current.isMaster != 1)
                    {
                        ViewBag.ViewName = "All Users";
                        return RedirectToAction("UserList", "User");
                    }
                    else
                    {
                        ViewBag.ViewName = "Home";
                        return View(info);
                    }
                }

            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ScriptStatistics() {
            int ID = SessionManager.Current.UserID;
            var info = new DashBoardHandler().ScriptStatisticsData(ID);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Charts(int TypeID) {
            int ID = SessionManager.Current.UserID;
            var info = new DashBoardHandler().Charts(ID,TypeID);
            return Json(info, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LessonTabData() {
            int ID = SessionManager.Current.UserID;
            var info = new DashBoardHandler().LessonTabData(ID);
            return Json(info, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaticDashboard() {

            if (SessionManager.Current.UserID != 0)
            {
                int ID = SessionManager.Current.UserID;
                int SelectedTab = 0;
                ListLessonInfo info = new LessonInfoManager().GetLessonPlanList(ID, SelectedTab);
                ViewBag.ViewName = "Dashboard Statistics";
                return View(info);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        public ActionResult LoadLesson(int SelectedTab) {
            if (SessionManager.Current.UserID != 0)
            {
                    int ID = SessionManager.Current.UserID;
                    ListLessonInfo info = new LessonInfoManager().GetLessonPlanList(ID, SelectedTab);
                    return Json(info,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult Error(string Masg=null)
        {
            return View();
        }
    }
}
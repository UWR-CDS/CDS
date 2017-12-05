using CDS.Models;
using LEAF_Logic;
using System;
using CDS.Manager;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDS.Logic;
using System.Data;
using System.Web.Routing;
using static CDS.Controllers.AccountController;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.Owin.Security;
using System.Web.Security;
using System.Text.RegularExpressions;


namespace CDS.Controllers
{
    public class UserManagementController : Controller
    {
        // GET: UserManagement
        public ActionResult GetPriviliges(int UserID)
        {
            if (SessionManager.Current.UserID != 0)
            {
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()), SessionManager.Current.UserID, 1, "Click on User to get Priviliges" + DateTime.Now + ".", this.Request.UserHostAddress);
                UserData obj = new UserData();
                obj = new UserManagement().GetUserPrivilige(UserID);
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }
        
        public ActionResult SavePriviliges(int UserID,string PriviligeID,string UserFirstName,string UserLastName,string ContactNo,int Status,int LoginAs) {
            if (SessionManager.Current.UserID != 0)
            {
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()), SessionManager.Current.UserID, 1, "Save UserData and Priviliges of " + UserID + "" + DateTime.Now + ".", this.Request.UserHostAddress);
                if (UserFirstName.Length > 20)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else if (UserLastName.Length > 20)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else if (Regex.IsMatch(ContactNo, @"^[a-zA-Z]+$"))
                {

                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else if (Status == 0)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    new UserManagement().SavePriviliges(UserID, PriviligeID, UserFirstName, UserLastName, ContactNo, Status, LoginAs);
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult UserLoginAs() {
            return Json(new UserManagement().UserLoginAs(),JsonRequestBehavior.AllowGet);
        }
    }
}
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
using System.IO;

namespace CDS.Controllers
{
    public class UserController : Controller
    {

        public ActionResult UserList() {
            if (SessionManager.Current.UserID != 0)
            {
                if (new CommonLogic().CanAccessPrivilige((int)Priviliges.CanViewUserList, Convert.ToInt32(SessionManager.Current.UserID)))
                {
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                                        SessionManager.Current.UserID, 1, "Access User Manger List" + DateTime.Now + ".", this.Request.UserHostAddress
                                       );
                    ViewBag.ViewName = "User Manager";
                    var List = new ManageUser().UserList(SessionManager.Current.UserID);
                    return View(List);
                }
                else {
                    return View("~/Views/Shared/Authurise.cshtml");
                }
            }
            else {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult LoginUser(string message=null)
        {
            if (CommonMng.baseUrl.Contains("localhost"))
            {
                ViewBag.islocalhost = "yes";
            }
            else
            {
                ViewBag.islocalhost = null;
            }
            if (Session["lomasg"] != null)
            {
                ViewBag.LoginError = Session["lomasg"].ToString();
                Session["lomasg"] = null;
            }

            if (message != null)
            {

                string ip =new ManageUser().LogoutBy(Convert.ToInt64(Session["LoginTrackID"]), SessionManager.Current.UserID);
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
                ViewBag.AlreadyLogout = "Now you are Login from New Location [" + ip + " ]";
                return View();
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult LoginUser(LoginModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserData objuser = new ManageUser().CheckUserCredentionals(obj.Email, EncyptionDcryption.GetEncryptedText(obj.Password));
                    LoginTracking obtrack = new LoginTracking();
                    if (objuser.UserEmail != null && objuser.Password.Equals(EncyptionDcryption.GetEncryptedText(obj.Password)))
                    {
                        DataTable dt = new ActivityLog().CheckLoginTracking(objuser.UserID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //if ((Convert.ToString(dt.Rows[0]["NewLogin"]) == "1") && (Convert.ToString(dt.Rows[0]["Msg"]) == ""))
                            //{
                            Session["louspass"] = obj.Password;
                            AddSessionValues(objuser);
                            obtrack.LoginTrackID = Convert.ToInt32(dt.Rows[0]["LoginTrackID"]);
                            Session["LoginTrackID"] = obtrack.LoginTrackID;
                            new ActivityLog().GenActivitylog(obtrack.LoginTrackID, objuser.UserID, 1, "successfully Loged in.", this.Request.UserHostAddress);
                            ViewBag.LoginError = null;

                            if (Session["LessonId"] != null)
                                return RedirectToAction("LessonPlan", new RouteValueDictionary(new
                                {
                                    controller = "LessonPlanEditor", action = "LessonPlan",
                                    LessonId = Session["LessonId"],
                                    mode = Session["LessonMode"],
                                    InvitaionID = Session["LessonInvitaionID"],
                                    InvitationUserID = Session["LessonInvitationUserID"],
                                    VerifiedUserID = Session["LessonVerifiedUserID"],
                                }));
                            if (Session["LessonId"] == null && objuser.DefaultPage != null && objuser.DefaultPage != "" && !(objuser.DefaultPage.Contains("Home/Index")))
                            {
                                return Redirect(objuser.DefaultPage);
                            }

                            else
                            {
                                if (string.IsNullOrEmpty(SessionManager.Current.DefaultPage))
                                {
                                    if (SessionManager.Current.PrivigilesID == 2 && SessionManager.Current.isMaster != 1)
                                    {
                                        return RedirectToAction("UnderVerificationLessons", "LessonPlanEditor");
                                    }
                                    else if (SessionManager.Current.PrivigilesID == 3 && SessionManager.Current.isMaster != 1)
                                    {
                                        return RedirectToAction("UserList", "User");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }

                            return RedirectToAction("Index", "Home");
                            //}
                            //else if ((Convert.ToString(dt.Rows[0]["NewLogin"]) == "0") && (Convert.ToString(dt.Rows[0]["Msg"]) != ""))
                            //{
                            //    AddSessionValues(objuser);
                            //    Session["LoginTrackID"] = Convert.ToInt64(dt.Rows[0]["LoginTrackID"]);
                            //   // ViewBag.AlreadyLogin = Convert.ToString(dt.Rows[0]["Msg"]);
                            //}
                        }
                    }
                    else if (obj.Email != null && obj.Password != null)
                    {
                        ViewBag.LoginError = "Login Failed Invalid Email/Password";
                        Reset();
                    }

                        return View(obj);
                    }
                else
                {
                    ModelState.AddModelError("Error", " error in model");
                    return View(obj);
                }
            }
            catch (Exception ex)
            {
                ViewBag.LoginError = "Login Failed Invalid Email/Password";
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "User Controller");
                return View(obj);
            }
        }
        [HttpGet]
        public ActionResult LoginSocailUser(string Email=null,string Password=null)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserData objuser = new ManageUser().CheckUserCredentionals(Email,Password);
                    LoginTracking obtrack = new LoginTracking();
                    if (objuser.UserEmail != null && objuser.Password.Equals(Password))
                    {
                        DataTable dt = new ActivityLog().CheckLoginTracking(objuser.UserID);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            //if ((Convert.ToString(dt.Rows[0]["NewLogin"]) == "1") && (Convert.ToString(dt.Rows[0]["Msg"]) == ""))
                            //{
                            Session["louspass"] = Password;
                            AddSessionValues(objuser);
                            obtrack.LoginTrackID = Convert.ToInt32(dt.Rows[0]["LoginTrackID"]);
                            Session["LoginTrackID"] = obtrack.LoginTrackID;
                            new ActivityLog().GenActivitylog(obtrack.LoginTrackID, objuser.UserID, 1, "successfully Loged in.", this.Request.UserHostAddress);
                            ViewBag.LoginError = null;
                            if (Session["LessonId"] != null)
                                return RedirectToAction("LessonPlan", new RouteValueDictionary(new
                                {
                                    controller = "LessonPlanEditor",
                                    action = "LessonPlan",
                                    LessonId = Session["LessonId"],
                                    mode = Session["LessonMode"],
                                    InvitaionID = Session["LessonInvitaionID"],
                                    InvitationUserID = Session["LessonInvitationUserID"],
                                    VerifiedUserID = Session["LessonVerifiedUserID"],
                                }));
                            if (Session["LessonId"] == null && objuser.DefaultPage != null && objuser.DefaultPage != "" && !(objuser.DefaultPage.Contains("Home/Index")))
                            {
                                return Redirect(objuser.DefaultPage);
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(SessionManager.Current.DefaultPage))
                                {
                                    if (SessionManager.Current.PrivigilesID == 2 && SessionManager.Current.isMaster != 1)
                                    {
                                        return RedirectToAction("UnderVerificationLessons", "LessonPlanEditor");
                                    }
                                    else if (SessionManager.Current.PrivigilesID == 3 && SessionManager.Current.isMaster != 1)
                                    {
                                        return RedirectToAction("UserList", "User");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                            return RedirectToAction("Index", "Home");
                            //}
                            //else if ((Convert.ToString(dt.Rows[0]["NewLogin"]) == "0") && (Convert.ToString(dt.Rows[0]["Msg"]) != ""))
                            //{
                            //    AddSessionValues(objuser);
                            //    Session["LoginTrackID"] = Convert.ToInt64(dt.Rows[0]["LoginTrackID"]);
                            //    ViewBag.AlreadyLogin = Convert.ToString(dt.Rows[0]["Msg"]);
                            //}
                        }
                    }
                    else if (Email != null && Password != null)
                    {
                        ViewBag.LoginError = "Login Failed Invalid Email/Password";
                        Reset();
                    }
                    return View();
                }
                else
                {
                    ModelState.AddModelError("Error", " error in model");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.LoginError = "Login Failed Invalid Email/Password";
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "User Controller");
                return RedirectToAction("Error", "Home");
            }
        }
        private void AddSessionValues(UserData objuser)
        {
            if (SessionManager.Current.UserID != 0)
            {
                new ManageUser().RemoveUserConnectionID(SessionManager.Current.UserID);
            }

            SessionManager.Current.UserID =  objuser.UserID;
            SessionManager.Current.EncUserID = EncyptionDcryption.GetEncryptedText(objuser.UserID.ToString());
            SessionManager.Current.Email = objuser.UserEmail;
            SessionManager.Current.UserName = objuser.UserFirstName +" "+ objuser.UserLastName;
            new ManageUser().RealseAllLessonLockBYUser(SessionManager.Current.UserID);
            if (objuser.DefaultPage != null && objuser.DefaultPage.Contains(CommonMng.baseUrl))
            {
                SessionManager.Current.DefaultPage = objuser.DefaultPage;
            }
            else
            {
                SessionManager.Current.DefaultPage = null;
            }
            SessionManager.Current.PrivigilesID = objuser.LoginAs;
            SessionManager.Current.isMaster = objuser.isMaster;
            SessionManager.Current.EntityID = objuser.EntityID;
            SessionManager.Current.EntityName = objuser.EntityName;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();
        }
        [HttpGet]
        public ActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPassword(ForgotPasswordModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserData objuser = new ManageUser().CheckUserEmail(obj.Email);
                    if (objuser.UserEmail != null)
                    {
                        string em = EncyptionDcryption.GetEncryptedText(obj.Email);
                        string date = EncyptionDcryption.GetEncryptedText(DateTime.Now.Date.ToString());
                        string Subject = "Change Password";
                        string url = CommonMng.baseUrl + "/User/ResetPassword?ID=" + em + "," + date;
                        string body = new ManageUser().GetResetPasswordTempalte();
                        body = body.Replace("@URL", url);
                        int res = CommonLogic.SendGnericEmail(obj.Email, body, Subject);
                        if (res == 1)
                        {
                            ViewData["ForgetPasswordError"] = "Email Sent to your Email Address for password Change";
                            return View(obj);
                        }
                        return View(obj);
                    }
                    else
                    {
                        ViewData["ForgetPasswordError"] = "Invalid Email Address";
                        return View(obj);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", " error in model");
                    return View(obj);
                }


            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "User Controller");
                return RedirectToAction("Error", "User");
            }
        }
        [HttpGet]
        public ActionResult ForceLogout()
        {
            LoginTracking objTrack = new ManageUser().LoginTracking(SessionManager.Current.UserID);
            if (objTrack != null)
            {
                Session["LoginTrackID"] = objTrack.LoginTrackID;
            }
            return Json(objTrack.LoginTrackID, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Logout()
        {
            if (SessionManager.Current.UserID != 0)
            {
                var trackid = Session["LoginTrackID"];
                new  ActivityLog().UpdateLoginTracking(Convert.ToInt32(trackid));
                new ManageUser().RemoveUserConnectionID(SessionManager.Current.UserID);
                Reset();
                return RedirectToAction("LoginUser", "User");
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }


        }
        private void Reset()
        {
            if (Session["LoginTrackID"]!=null)
            {
                new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                        SessionManager.Current.UserID, 1, "Logout.", this.Request.UserHostAddress
                       );
            }
            new ManageUser().RealseAllLessonLockBYUser(SessionManager.Current.UserID);
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            Session["LoginTrackID"] = null;
            Session["LessonId"] = null;
            Session["LessonMode"] = null;
            Session["LessonInvitaionID"] = null;
            Session["LessonInvitationUserID"] = null;
            Session["LessonVerifiedUserID"] = null;
        }
        [HttpGet]
        public ActionResult Expire()
        {

            Reset();
            return RedirectToAction("LoginUser", "User");

        }
        [HttpGet]
        public ActionResult ResetPassword(string ID)
        {
            if (ID != null)
            {
                var text = ID.Split(',');
                string email = text[0].Replace(' ', '+');
                string date = text[1].Replace(' ', '+');
                string dt = EncyptionDcryption.GetDecryptedText(date);
                DateTime d = Convert.ToDateTime(dt);
                if (d.Day == DateTime.Now.Day || d.Day + 1 == DateTime.Now.Day - 1 || d.Day + 2 == DateTime.Now.Day)
                {
                    string em = EncyptionDcryption.GetDecryptedText(email);
                    ResetPasswordModel obj = new ResetPasswordModel();
                    obj.Email = em;
                    return View(obj);
                }
                else
                {
                    return RedirectToAction("Error", "User");
                }

            }
            return View();

        }
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (obj.Password.Length < 8)
                    {
                        ViewData["ResetPasswordError"] = "New Password must be 8 characters long";
                        return View(obj);
                    }
                    new ManageUser().UpdateUserPAssword(EncyptionDcryption.GetEncryptedText(obj.Password), obj.Email);
                    Session["lomasg"] = "Password Updated Successfully. Please Login Here";
                    return RedirectToAction("LoginUser", "User");

                }
                else
                {
                    ViewData["ResetPasswordError"] = "Sorry Something Wrong.";
                    ModelState.AddModelError("Error", " error in model");
                    return View(obj);
                }


            }
            catch (Exception ex)
            {
                new CommonLogic().InsertError(ex.Message, System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name.ToString(), "User Controller");
                return RedirectToAction("Error", "User");
            }
        }
        [HttpGet]
        public ActionResult Register(string UserId=null)
        {
            try
            {
                if (CommonMng.IsNumeric(UserId) || UserId==null || UserId=="")
                {
                    ViewBag.LoginError = "Invalid User.";
                    return RedirectToAction("LoginUser", "User");
                }
                Session["rEGuSERiD"] = Convert.ToInt32(EncyptionDcryption.GetDecryptedText(UserId));
                ViewBag.thisUser = UserId;
                UserData obj = new ManageUser().GetUserById(Convert.ToInt32(EncyptionDcryption.GetDecryptedText(UserId)));
                return View(obj);
            }
            catch (Exception EX)
            {
                ViewBag.LoginError = "Invalid User.";
                return RedirectToAction("LoginUser", "User");
            }

        }
        [HttpPost]
        public ActionResult Register(UserData obj)
        {
            if (Session["rEGuSERiD"] != null)
            {
                obj.UserID = Convert.ToInt32(Session["rEGuSERiD"]);
            }
            if (ModelState.IsValid)
            {
                if (obj.UserEmail.Contains("@testemail.com"))
                {
                    ViewBag.RegisterMassage = "Sorry this Email is not valid.";
                    return View(obj);
                }
                int UserID = new ManageUser().ManageUsers(obj);
                if (UserID == -3)
                {

                    ViewBag.RegisterMassage = "Sorry this Email Address is blocked.Please use another.";
                    return View(obj);
                }
                if (UserID == -1)
                {
                    ViewBag.RegisterMassage = "Sorry this Email Id already exists.";
                    return View(obj);
                }

                if (obj.UserID == 0 || UserID == -2)
                {
                    string em = EncyptionDcryption.GetEncryptedText(obj.UserEmail);
                    string date = EncyptionDcryption.GetEncryptedText(DateTime.Now.Date.ToString());
                    string Subject = "Verifiy Your Account";
                    string url = CommonMng.baseUrl + "/User/VerifyAccount?ID=" + em + "," + date;
                    string body = new ManageUser().GetVerifyTempalte();
                    body = body.Replace("@URL", url);
                    int res = CommonLogic.SendGnericEmail(obj.UserEmail, body, Subject);
                    if (res == 1)
                    {
                        return RedirectToAction("RegisterMasg", "User");
                    }
                }

            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult UpdateUserSubjects(string[] lst = null)
        {
            if (SessionManager.Current.UserID !=0)
            {
                if (lst != null)
                {
                    int a = 1;
                    foreach (var item in lst)
                    {
                        string abc = item;
                        string clsid = abc.Split('-').First();
                        string subid = abc.Split('-').Last();
                        new ManageUser().UpdateUserSubject(a,SessionManager.Current.UserID, Convert.ToInt32(clsid), Convert.ToInt32(subid));
                        a = 2;
                    }
                    return Json("Done", JsonRequestBehavior.AllowGet);
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
        [HttpGet]
        public ActionResult VerifyAccount(string ID)
        {
            if (ID != null)
            {
                var text = ID.Split(',');
                string email = text[0].Replace(' ', '+');
                string date = text[1].Replace(' ', '+');
                string dt = EncyptionDcryption.GetDecryptedText(date);
                DateTime d = Convert.ToDateTime(dt);
                string em = EncyptionDcryption.GetDecryptedText(email);
                var numberOfDays = (DateTime.Now-d).TotalDays;
                if (numberOfDays <= 30)
                {
                    UserData obj = new ManageUser().CheckUserEmail(em);
                    if (obj.UserEmail!="" && obj.UserEmail!=null)
                    {
                        new ManageUser().UpdateStatus(em,obj.UserID);
                        return RedirectToAction("LoginUser", "User");
                    }
                    else
                    {
                        ViewBag.RegisterMassage = "Sorry this user not exist.Please Register Here.";
                        return RedirectToAction("Register", "User", new
                        {
                            UserId ="3tqJU97nj+c=",
                        });
                    }
                }
                string Subject = "Verifiy Your Account";
                string url = CommonMng.baseUrl + "/User/VerifyAccount?ID=" + em + "," + date;
                string body = new ManageUser().GetVerifyTempalte();
                body = body.Replace("@URL", url);
                int res = CommonLogic.SendGnericEmail(em, body, Subject);
                if (res == 1)
                {
                    Session["lomasg"] = "Sorry this link is expired.Please Check your Email Id for verifiy your Account.";
                    return RedirectToAction("LoginUser", "User");
                }
                else
                {
                    return RedirectToAction("VerifyAccount", "User", new
                    {
                        ID = ID
                    });
                }
            }
            else
            {
                return RedirectToAction("Error", "User");
            }


        }
        [HttpGet]
        public ActionResult Error()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetUserSubjects()
        {
            if (SessionManager.Current.UserID != 0)
            {
                var lst = new Mngr_Con_Class().GetAllUserSubjects(SessionManager.Current.UserID);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult UserSubjects()
        {
            if (SessionManager.Current.UserID!=0)
            {

                ViewBag.ViewName = "Subject List";
                ViewBag.AllClasses = new Mngr_Con_Class().GradeDropDown();
                ViewBag.AllSubjects = new Mngr_Con_Class().GetAllSubjects();
                ViewBag.UserSubject = new Mngr_Con_Class().GetAllUserSubjects(SessionManager.Current.UserID);
                return View();
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }
        [HttpGet]
        public ActionResult RegisterMasg()
        {
            return View();
        }
        [HttpGet]
        public ActionResult UpdateUser(string UserId = null)
        {
            try
            {
                if (SessionManager.Current.UserID!=0)
                {
                    if (Session["UpdateUserMasg"] != null)
                    {
                        ViewBag.UpdateUser = Session["UpdateUserMasg"];
                        Session["UpdateUserMasg"] = null;
                    }
                    ViewBag.ViewName = "Update Profile";
                    UserData obj = new ManageUser().GetUserById(SessionManager.Current.UserID);
                    obj.lstUserSubjects = new Mngr_Con_Class().GetAllUserSubjects(SessionManager.Current.UserID);
                    return View(obj);
                }
                else
                {
                    return RedirectToAction("LoginUser", "User");
                }

            }
            catch (Exception EX)
            {
                ViewBag.LoginError = "Invalid User.";
                return RedirectToAction("LoginUser", "User");
            }

        }
        [HttpPost]
        public ActionResult UpdateUser(UserData obj)
        {
            if (SessionManager.Current.UserID!=0)
            {
                obj.UserID = SessionManager.Current.UserID;
                if (ModelState.IsValid)
                {
                    if (obj.UserEmail.Contains("@testemail.com"))
                    {
                        Session["UpdateUserMasg"] = "Sorry this Email is not valid.";
                        return View(obj);
                    }
                    int UserID = new ManageUser().ManageUsers(obj);
                    if (UserID == -1)
                    {
                        Session["UpdateUserMasg"] = "Sorry this Email Id already exists.";
                        return View(obj);
                    }
                    SessionManager.Current.Email = obj.UserEmail;
                    SessionManager.Current.UserName = obj.UserFirstName + " " + obj.UserLastName;
                    Session["UpdateUserMasg"] = "Profile Updated Successfully...!";
                }

                return RedirectToAction("UpdateUser", "User", new
                {
                    UserId = EncyptionDcryption.GetEncryptedText(SessionManager.Current.UserID.ToString()),
                });
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }

        }
        [HttpGet]
        public ActionResult SetDefaultPage(string Page="")
        {
            if (SessionManager.Current.UserID!=0)
            {
                string msg = new ManageUser().SetDefaultPage(Page);
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("-1", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UploadIcon()
        {
            if (SessionManager.Current.UserID != 0)
            {
                try
                {
                    var pic = System.Web.HttpContext.Current.Request.Files["Myicon"];
                    if (pic != null && pic.ContentLength > 0)
                    {
                        var path = System.Configuration.ConfigurationSettings.AppSettings["MediaPath"] + @"User\";
                        if (pic.ContentLength > 0)
                        {
                            DirectoryInfo fol = new DirectoryInfo(path);
                            if (fol.Exists == false)
                            {
                                Directory.CreateDirectory(path);
                            }
                            string pth = path + "\\"+SessionManager.Current.UserID+".PNG";
                            pic.SaveAs(pth);
                            return Json(true, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(false, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return RedirectToAction("LoginUser", "User");

            }
        }
    }
}
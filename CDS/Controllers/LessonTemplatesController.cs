using CDS.Logic;
using CDS.Manager;
using CDS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class LessonTemplatesController : Controller
    {
        public ActionResult Index(int TemplID = 0,int LessonID=0,int BookID=0)
        {
            if (SessionManager.Current.UserID != 0 && LessonID!=0)
            {
                ViewBag.ViewName = "Lesson Templates";
                TemplateModel obj = new TemplateModel();
                obj.lst = new TemplateManager().GetLessonTemplates(LessonID, SessionManager.Current.UserID);
                DataSet ds = null;
                if (obj.lst!=null && obj.lst.Count!=0 && TemplID==0)
                {
                    ds = new TemplateManager().GetTemplateDetails(obj.lst[0].TemplateID,LessonID);
                    obj.SeletedTemplate = obj.lst[0].TemplateID;
                }
                else if(TemplID!=0)
                {
                    ds = new TemplateManager().GetTemplateDetails(TemplID,LessonID);
                    obj.SeletedTemplate = TemplID;
                }
                if (ds!=null)
                {
                    obj.obj = new TemplateDetail();
                    obj.LanguageID = ds.Tables[0].Rows[0]["LanguageID"] == DBNull.Value ? 1 : Convert.ToInt32(ds.Tables[0].Rows[0]["LanguageID"]);
                    obj.obj.lstSection=new LessonInfoManager().GetLessonSections(ds.Tables[1]);
                    obj.obj.lstSubsection = new LessonInfoManager().GetLessonSubSections(ds.Tables[2]);
                }
                int SelectedTab = 1;
                ListLessonInfo less =new LessonInfoManager().GetLessonPlanList(SessionManager.Current.UserID, SelectedTab);
                if (less.lstLessonInfo!=null)
                {
                    LessonInfo info = less.lstLessonInfo.Where(x => x.LessonID == LessonID).FirstOrDefault();
                    if (info != null)
                    {
                        obj.IsExist = info.LessonID;
                    }
                }

                obj.SeletedLessonId = LessonID;
                obj.SeletedBookID = BookID;
                return View(obj);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
        }

        public ActionResult SaveTemp(int TempID=0,int LessonId=0,string islessonscript=null)
        {
            if (TempID!=0 && LessonId!=0 && SessionManager.Current.UserID!=0)
            {
                int a = new TemplateManager().SaveTemplate(TempID, LessonId, SessionManager.Current.UserID);
                if (a==1)
                {
                    new ActivityLog().GenActivitylog(Convert.ToInt64(Session["LoginTrackID"].ToString()),
                                     SessionManager.Current.UserID, 1, "Lesson Import where LessonID=" + LessonId + " and Template ID="+TempID+" on " + DateTime.Now + ".", this.Request.UserHostAddress
                                    );
                    string mode = null;
                    if (islessonscript!=null)
                    {
                        mode = islessonscript;
                    }
                    else
                    {
                        mode = "SS";
                    }
                    if (true)
                    {

                    }
                    return RedirectToAction("LessonPlan", "LessonPlanEditor", new
                    {
                        LessonId = EncyptionDcryption.GetEncryptedText(LessonId.ToString()),
                        mode =mode
                    });
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
    }
}
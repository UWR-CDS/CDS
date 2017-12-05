using CDS.Manager;
using CDS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class SectionsHistoryController : Controller
    {
        // GET: SectionsHistory
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetSectionHistoryDetail(int ID,int LessonPlanId, int BooktypeId, string SectionName = null, string Direction = null, int IsindexPage = 0)
        {
            LessonPlanEditorUserWork obj = new LessonPlanEditorUserWork();
            obj.ID = ID;
            obj.LessonPlanID = LessonPlanId;
            obj.BookTypeID = BooktypeId;
            obj.IsIndexPage = IsindexPage;
            obj.Direction = Direction;
            if (ID != 0)
            {
                if (Request.IsAjaxRequest())
                {
                    if (BooktypeId == 1)
                    {
                        obj._lstSectionhistory = new LessonInfoManager().LessonPlanUserWork(ID,LessonPlanId);
                    }
                    else
                    {
                       // obj._lstSectionhistory = new BookComposingHandler().GetPageHistory(ID, IsindexPage);
                    }
                    obj.SectionName = SectionName;
                    return PartialView("_Index", obj);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult GetHistory(int ID, int LessonPlanId, int BooktypeId, int IsindexPage = 0, int Num = 0)
        {
            List<SectionHistory> lst = null;
            if (ID != 0)
            {
                if (BooktypeId == 1)
                {
                    lst = new LessonInfoManager().LessonPlanUserWork(ID,LessonPlanId,Num);
                }
                else
                {
                    //lst = new LessonInfoManager().GetPageHistory(ID, IsindexPage, Num);
                }
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}
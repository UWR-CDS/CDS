using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDS.Models;
namespace CDS.Controllers
{
    public class FAQController : Controller
    {
        // GET: FAQ
        public ActionResult Index()
        {
            if (SessionManager.Current.UserID != 0)
            {
                   ViewBag.ViewName = "FAQ";
                    var List = new FAQHandler().GetQuestionAnswers();
                    return View(List);
            }
            else
            {
                return RedirectToAction("LoginUser", "User");
            }
           
        }
    }
}
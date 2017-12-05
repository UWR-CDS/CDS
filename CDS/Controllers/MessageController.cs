using CDS.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CDS.Controllers
{
    public class MessageController : Controller
    {
        [HttpGet]
        public ActionResult GetMessage()
        {
            return Json(new Mngr_Message().GetMessage(), JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    public class ReportController : BaseController
    {
        [HttpPost]
        public ActionResult HourStatistics()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlertsStatistics()
        {
            return View();
        }
    }
}
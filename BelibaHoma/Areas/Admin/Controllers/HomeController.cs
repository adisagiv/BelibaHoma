using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.Controllers;


namespace BelibaHoma.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index","Home", new { Area = "Rackaz"});
        }
    }
}
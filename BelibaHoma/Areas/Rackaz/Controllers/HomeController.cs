using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Rackaz/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
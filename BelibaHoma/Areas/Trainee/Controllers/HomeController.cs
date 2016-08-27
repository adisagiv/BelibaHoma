using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Trainee.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Trainee/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}
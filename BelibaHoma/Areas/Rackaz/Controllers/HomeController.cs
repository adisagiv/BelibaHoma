using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Enums;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Rackaz/Home
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            return View(model);
        }
    }
}
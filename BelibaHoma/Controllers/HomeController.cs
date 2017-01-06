using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using Generic.Models;

namespace BelibaHoma.Controllers
{
    public class HomeController : BaseController
    {
        
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Error(StatusModel model = null)
        {
            if (model == null)
            {
                model = new StatusModel(false, "Error test");
            }
            return View(model);
        }
    }
}
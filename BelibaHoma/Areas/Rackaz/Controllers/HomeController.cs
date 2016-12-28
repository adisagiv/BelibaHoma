using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class HomeController : BaseController
    {
        private readonly IAlertService _alertService;

        public HomeController(IAlertService alertService)
        {
            this._alertService = alertService;
        }

        // GET: Rackaz/Home
        public ActionResult Index()
        {
            var model = new HomeViewModel();
            var status = _alertService.GenerateLateTutorsAlerts();
            if (!status.Success)
            {
                return Error(status);
            }
            return View(model);
        }
    }
}
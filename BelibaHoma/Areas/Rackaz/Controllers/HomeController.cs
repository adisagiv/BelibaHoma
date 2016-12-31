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
        private readonly ITutorService _tutorService;
        private readonly IAlertService _alertService;

        public HomeController(IAlertService alertService, ITutorService tutorService)
        {
            _tutorService = tutorService;
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
            var result = _alertService.GetAlertStatusCounts(CurrentUser.Area);
            if (!result.Success)
            {
                return Error(result);
            }
            var status2 = _tutorService.GetTutorHours(CurrentUser.Area);
            if (!status2.Success)
            {
                return Error(status2);
            }
            model.TutorHoursCount = status2.Data;
            model.newAlertsCount = result.Data[0];
            model.OnGoingAlertsCount = result.Data[1];
            return View(model);
        }
    }
}
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Tutor.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz, UserRole.Tutor })]
    public class TutorReportController : BaseController
    {
        private readonly ITutorReportService _tutorReportService;
        private readonly IAlertService _alertService;

        public TutorReportController(ITutorReportService tutorReportService, IAlertService alertService)
        {
            this._tutorReportService = tutorReportService;
            _alertService = alertService;
        }

        public ActionResult Index()
        {
            var result = _tutorReportService.Get();
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }


        public ActionResult TutorTraineeReports(int id)
        {
            var result = _tutorReportService.GetById(id);
            if (result.Success)
            {
                ViewBag.TutorTraineeId = id;
                return View(result.Data);

            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }



        public ActionResult Create(int tutorTraineeId)
        {
            var model = new TutorReportModel { TutorTraineeId = tutorTraineeId};
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TutorReportModel model)
        {
            var result = _tutorReportService.Add(model);
            var id = model.TutorTraineeId;
            if (result.Success)
            {
                if (model.IsProblem)
                {
                    var status = _alertService.AddInervention(result.Data);
                    if (!status.Success)
                    {
                        return Error(status);
                    }
                }
                return RedirectToAction("TutorTraineeReports", new { id = id });
            }
            return Error(result);
        }

        public ActionResult Edit(int id)
        {
            var result = _tutorReportService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, TutorReportModel model)
        {
            var result = _tutorReportService.Update(id, model);
            var tutorTraineeId = model.TutorTraineeId;

            if (result.Success)
            {
                if (model.IsProblem)
                {
                    result = _alertService.AddInervention(model.Id);
                    if (!result.Success)
                    {
                        return Error(result);
                    }
                }
                //return RedirectToAction("Index");
                return RedirectToAction("TutorTraineeReports", new { id = tutorTraineeId });
            }
            return Error(result);
        }

    }
}
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;
using BelibaHoma.DAL;
using Generic.Models;

namespace BelibaHoma.Areas.Tutor.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz, UserRole.Tutor })] 
    public class TutorSessionController : BaseController
    {
        private readonly ITutorSessionService _TutorSessionService;
        private readonly ITutorReportService _tutorReportService;

        public TutorSessionController(ITutorSessionService TutorSessionService, ITutorReportService tutorReportService)
        {
            this._TutorSessionService = TutorSessionService;
            _tutorReportService = tutorReportService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index()
        {
            var result = _TutorSessionService.Get();
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Create(int id)
        {
            var status = _tutorReportService.Get(id);

            if (status.Success)
            {
                var model = new TutorSessionModel
                {
                    TutorReportId = id,
                    TutorReport = new TutorReport
                    {
                        TutorTraineeId = status.Data.TutorTraineeId
                    }
                };
                return View(model);
            }
            return Error(status);

        }

        [HttpPost]
        public ActionResult Create(TutorSessionModel model)
        {
            var result = _TutorSessionService.Add(model);
            var tutorTraineeId = model.TutorReport.TutorTraineeId;
            
            if (result.Success)
            {
                ViewBag.TutorTraineeId = tutorTraineeId;
                return RedirectToAction("TutorTraineeReports", "TutorReport", new { id = tutorTraineeId });
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Details(int id)
        {
            var result = _TutorSessionService.GetById(id);
            if (result.Success)
            {
                return View(result.Data);

            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }


        public ActionResult Edit(int id)
        {
            var result = _TutorSessionService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, TutorSessionModel model)
        {
            var result = _TutorSessionService.Update(id, model);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            return Error(new StatusModel(false, result.Message));
        }

    }
}
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Repositories;
using Generic.Models;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class TutorTraineeController : BaseController
    {
        private readonly ITutorTraineeService _tutorTraineeService;
        private readonly ITraineeService _traineeService;
        private readonly ITutorService _tutorService;


        public TutorTraineeController(ITutorTraineeService tutorTraineeService, ITraineeService traineeService, ITutorService tutorService)
        {
            this._tutorTraineeService = tutorTraineeService;
            _traineeService = traineeService;
            _tutorService = tutorService;
        }

        // GET: Relationship
        public ActionResult Index()
        {
            if (CurrentUser.UserRole.ToString() == "Rackaz")
            {
                ViewBag.IsRackaz = true;
            }
            else
            {
                ViewBag.IsRackaz = false;
            }
            var result = _tutorTraineeService.Get(CurrentUser.Area);
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }
        public ActionResult Edit(int id)
        {
            var result = _tutorTraineeService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);

            return new HttpNotFoundResult();
        }

        [HttpPost]
        public ActionResult Edit(int id, TutorTraineeModel model)
        {
            var result = _tutorTraineeService.Update(id, model);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            return Error(new StatusModel(false, result.Message));
        }

        public ActionResult ManualMatch()
        {
            var model = new TutorTraineeViewModel();
            if (CurrentUser.UserRole.ToString() == "Rackaz")
            {
                ViewBag.IsRackaz = true;
                model.Area = CurrentUser.Area;
            }
            else
            {
                ViewBag.IsRackaz = false;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult ManualMatchAreaSelected(Area area)
        {
            var model = new TutorTraineeViewModel();
            if (CurrentUser.UserRole.ToString() == "Rackaz")
            {
                model.IsRackaz = true;
                model.Area = area;
            }
            else
            {
                model.IsRackaz = false;
                model.Area = null;
            }
            //Add trainee list
            var traineeStatus = _traineeService.GetUnMatchedTrainees(area, false);
            if (traineeStatus.Success)
            {
                model.Trainees = traineeStatus.Data;
            }
            else
            {
                traineeStatus.Message = traineeStatus.Message;
                Response.StatusCode = 404;
                return Error(traineeStatus);
            }
           // add tutor list
            var tutorStatus = _tutorService.GetUnMatchedTutors(area, false);
            if (tutorStatus.Success)
            {
                model.Tutors = tutorStatus.Data;
            }
            else
            {
                tutorStatus.Message = tutorStatus.Message;
                Response.StatusCode = 404;
                return Error(tutorStatus);
            }
            return View(model);
        }

        public ActionResult ManualMatchTraineeSelect(bool showMatched, Area area)
        {
            var traineeStatus = new StatusModel<List<TraineeMatchViewModel>>();
            traineeStatus = _traineeService.GetUnMatchedTrainees(area, showMatched);
            if (traineeStatus.Success)
            {
                return View(traineeStatus.Data);
            }
            traineeStatus.Message = traineeStatus.Message;
            Response.StatusCode = 404;
            return Error(traineeStatus);
        }

        public ActionResult ManualMatchTutorSelect(bool showMatched, Area area)
        {

            var tutorStatus = new StatusModel<List<TutorMatchViewModel>>();
            tutorStatus = _tutorService.GetUnMatchedTutors(area, showMatched);
            if (tutorStatus.Success)
            {
                return View(tutorStatus.Data);
            }
            tutorStatus.Message = tutorStatus.Message;
            Response.StatusCode = 404;
            return Error(tutorStatus);
        }

        [HttpPost]
        public ActionResult ManualMatchCreate(TutorTraineeModel model)
        {
            var result = _tutorTraineeService.AddManual(model);
            return Json(result);
        }
    }
}

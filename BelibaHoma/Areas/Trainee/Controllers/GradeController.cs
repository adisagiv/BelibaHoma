using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Trainee.Models;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Trainee.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz, UserRole.Trainee })]
    public class GradeController : BaseController
    {
        private readonly IGradeService _gradeService;
        private readonly ITraineeService _traineeService;
        private readonly IAlertService _alertService;
        private readonly int GradeThreshold = 70;

        public GradeController(IGradeService gradeService,ITraineeService traineeService, IAlertService alertService)
        {
            this._gradeService = gradeService;
            _traineeService = traineeService;
            _alertService = alertService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index(int id)
        {
            if (CurrentUser.UserRole == UserRole.Trainee)
            {
                id = CurrentUser.Id;
            }
            var result = _gradeService.GetById(id);
            if (result.Success)
            {
                var traineeResult = _traineeService.Get(id);
                if (traineeResult.Success)
                {
                    var gradeViewModel = new GradeViewModel
                    {
                        Trainee = traineeResult.Data,
                        Grades = result.Data
                    };

                    return View(gradeViewModel); 
                }
                
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        //TODO:Fix this : 
        public ActionResult Create(int traineeId) //,int semesterNumber )
        {
            ViewBag.IsCreate = true;
            var model = new GradeModel {TraineeId = traineeId};//, SemesterNumber = semesterNumber};
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(GradeModel model)
        {
            ViewBag.IsCreate = true;
            var result = _gradeService.Add(model);

            if (result.Success)
            {
                if (model.Grade1 < GradeThreshold)
                {
                    var status = _alertService.AddTraineeGrade(model.TraineeId);
                    if (!status.Success)
                    {
                        return Error(status);
                    }
                }
                //TODO: Change Index to other action
                return RedirectToAction("Index", new { id = model.TraineeId});
            }

            //TODO: What is these lines below supposed to do? If it needs to refer to Error page that is not the way..
            var status1 = new StatusModel(false, result.Message);
            ModelState.AddModelError(result.Data, result.Message);
            return View(model);
        }

        public ActionResult Edit(int id, int semesterNumber)
        {
            ViewBag.IsCreate = false;
            var result = _gradeService.Get(id,semesterNumber); //shell we do bet ny id and semester number?
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, int semesterNumber, GradeModel model)
        {
            ViewBag.IsCreate = false;
            var result = _gradeService.Update(id, model);
            if (result.Success)
            {
                if (model.Grade1 < GradeThreshold)
                {
                    var status = _alertService.AddTraineeGrade(model.TraineeId);
                    if (!status.Success)
                    {
                        return Error(status);
                    }
                }
                //TODO: Change Index to other action
                return RedirectToAction("Index", new { id = id });
            }
            return Error(result);
        }
        [HttpGet]
        public ActionResult Delete(int id, int semesterNumber)
        {
            _gradeService.Delete(id, semesterNumber);
            // TODO  : result.Success
            return RedirectToAction("Index", new { id = id });
        }

    }
}
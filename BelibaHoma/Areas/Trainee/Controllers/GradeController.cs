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
        private readonly IGradeService _GradeService;
        private readonly ITraineeService _traineeService;

        public GradeController(IGradeService GradeService,ITraineeService traineeService)
        {
            this._GradeService = GradeService;
            _traineeService = traineeService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index(int id)
        {
            var result = _GradeService.GetById(id);
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

        //public ActionResult Create()
        //{
        //    var model = new GradeModel { };
        //    return View(model);
        //}
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
            var result = _GradeService.Add(model);

            if (result.Success)
            {
                //TODO: Change Index to other action
                return RedirectToAction("Index", new { id = model.TraineeId});
            }

            var status = new StatusModel(false, result.Message);
            ModelState.AddModelError(result.Data, result.Message);
            return View(model);
        }

        public ActionResult Edit(int id, int semesterNumber) //I am here?????????
        {
            ViewBag.IsCreate = false;
            var result = _GradeService.Get(id,semesterNumber); //shell we do bet ny id and semester number?
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
            var result = _GradeService.Update(id, model);
            if (result.Success)
            {
                //TODO: Change Index to other action
                return RedirectToAction("Index", new { id = id });
            }
            return Error(new StatusModel(false, result.Message));
        }
        [HttpGet]
        public ActionResult Delete(int id, int semesterNumber)
        {
            _GradeService.Delete(id, semesterNumber);
            // TODO  : result.Success
            return RedirectToAction("Index", new { id = id });
        }

    }
}
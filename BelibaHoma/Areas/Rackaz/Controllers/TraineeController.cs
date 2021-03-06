﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Repositories;
using Extensions.DateTime;
using Generic.Models;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class TraineeController : BaseController
    {
        private readonly ITraineeService _traineeService;
        private readonly IAcademicInstitutionService _academicInstitutionService;
        private readonly IAcademicMajorService _academicMajorService;

        public TraineeController(ITraineeService traineeService, IAcademicInstitutionService academicInstitutionService,
            IAcademicMajorService academicMajorService)
        {
            this._traineeService = traineeService;
            this._academicInstitutionService = academicInstitutionService;
            this._academicMajorService = academicMajorService;
        }

        // GET: Rackaz/User
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
            var result = _traineeService.GetTrainees(CurrentUser.Area);
            if (!result.Success)
            {
                var status = new StatusModel(false, result.Message);
                return Error(status);
            }
            return View(result.Data);
        }

        public ActionResult Create()
        {
            ViewBag.IsCreate = true;
            var academicMajorResult = _academicMajorService.Get();
            if (!academicMajorResult.Success)
            {
                var status = new StatusModel(false, academicMajorResult.Message);
                return Error(status);
            }
            var academicInstitutionResult = _academicInstitutionService.Get(CurrentUser.Area);
            if (!academicInstitutionResult.Success)
            {
                var status = new StatusModel(false, academicMajorResult.Message);
                return Error(status);
            }

            TraineeViewModel model = new TraineeViewModel
            {
                AcademicInstitutionList = academicInstitutionResult.Data,
                AcademicMajorList = academicMajorResult.Data,
                Trainee = new TraineeModel()
                {
                    Birthday = DateTime.Now
                }
            };
            model.Trainee.User = new UserModel();
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsRackaz = false;
            }
            else
            {
                model.Trainee.User.Area = CurrentUser.Area;
                ViewBag.IsRackaz = true;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TraineeModel model)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                model.User.Area = CurrentUser.Area;
            }
            var result = _traineeService.Add(model);
            //The return status check is performed in Trainee Script..
            return Json(result);
        }

        public ActionResult Details(int id)
        {
            var updateResult = _traineeService.UpdateTraineePazam(id);
            string message;
            if (updateResult.Success)
            {


                var result = _traineeService.Get(id);
                if (result.Success)
                {
                    return View(result.Data);
                }
                else
                {
                    message = result.Message;
                }
            }
            else
            {
                message = updateResult.Message;
            }
            var status = new StatusModel(false,message);
            return Error(status);
        }

        public ActionResult Edit(int id)
        {
            var academicInstitutionResult = _academicInstitutionService.Get(CurrentUser.Area);
            if (!academicInstitutionResult.Success)
            {
                return Error(new StatusModel(false,academicInstitutionResult.Message));
            }
            var academicMajorResult = _academicMajorService.Get();
            if (!academicMajorResult.Success)
            {
                return Error(new StatusModel(false, academicMajorResult.Message));
            }
            TraineeViewModel model = new TraineeViewModel
            {
                AcademicInstitutionList = academicInstitutionResult.Data,
                AcademicMajorList = academicMajorResult.Data,
                Trainee = new TraineeModel()
            };
            ViewBag.IsRackaz = CurrentUser.UserRole == UserRole.Rackaz;
            ViewBag.IsCreate = false;
            var result = _traineeService.Get(id);
            if (result.Success)
            {
                model.Trainee = result.Data;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, TraineeModel model)
        {
            var result = _traineeService.Update(id, model);
            SetUserUpdate(id, DateTime.MinValue.Utc());
            //The return status check is performed in trainee script
            return Json(result);
        }
    }
}


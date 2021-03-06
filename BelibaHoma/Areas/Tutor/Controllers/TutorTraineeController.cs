﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Repositories;
using Generic.Models;

namespace BelibaHoma.Areas.Tutor.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Tutor, UserRole.Admin, UserRole.Rackaz })]
    public class TutorTraineeController : BaseController
    {
        private readonly ITutorTraineeService _tutorTraineeService;

        public TutorTraineeController(ITutorTraineeService tutorTraineeService)
        {
            _tutorTraineeService = tutorTraineeService;
        }

        public ActionResult Index(int id)
        {
            if (CurrentUser.UserRole == UserRole.Tutor)
            {
                id = CurrentUser.Id;
            }
            var result = _tutorTraineeService.GetById(id);
            if (result.Success)
            {
                if (result.Data.Count == 1)
                {
                    var tutorTraineeId = result.Data.First().Id;
                    return RedirectToAction("TutorTraineeReports", "TutorReport",
                        new {Area = "Tutor", id = tutorTraineeId});
                }
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }
    }
}
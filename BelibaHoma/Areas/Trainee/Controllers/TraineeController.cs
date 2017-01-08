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

namespace BelibaHoma.Areas.Trainee.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Trainee, UserRole.Admin, UserRole.Rackaz })]
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

        public ActionResult Details(int id)
        {
            var updateResult = _traineeService.UpdateTraineePazam(id);
            string message;
            if (updateResult.Success)
            {
                if (CurrentUser.UserRole == UserRole.Trainee)
                {
                    ViewBag.IsTrainee = true;
                }
                else
                {
                    ViewBag.IsTrainee = false;
                }
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
            var status = new StatusModel(false, message);
            return Error(status);
        }
    }
}
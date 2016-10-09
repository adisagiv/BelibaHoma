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
            return View(result);
        }

        public ActionResult Create()
        {
            ViewBag.IsCreate = true;
            TraineeViewModel model = new TraineeViewModel
            {
                AcademicInstitutionList = _academicInstitutionService.Get(CurrentUser.Area),
                AcademicMajorList = _academicMajorService.Get(),
                Trainee = new TraineeModel(),
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

            if (result.Success)
            {
                return RedirectToAction("Index", "Trainee", new { Area = "Rackaz" });
            }

            return Json(new StatusModel(true,"יאי שמרתי"));
        }
    }
}


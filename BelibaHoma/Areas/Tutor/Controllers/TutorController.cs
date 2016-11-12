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

namespace BelibaHoma.Areas.Tutor.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Tutor, UserRole.Admin,UserRole.Rackaz })]
    public class TutorController : BaseController
    {
        private readonly ITutorService _tutorService;
        private readonly IAcademicInstitutionService _academicInstitutionService;
        private readonly IAcademicMajorService _academicMajorService;

        public TutorController(ITutorService tutorService, IAcademicInstitutionService academicInstitutionService,
            IAcademicMajorService academicMajorService)
        {
            this._tutorService = tutorService;
            this._academicInstitutionService = academicInstitutionService;
            this._academicMajorService = academicMajorService;
        }

        public ActionResult Details(int id)
        {
            if (CurrentUser.UserRole == UserRole.Tutor)
            {
                ViewBag.IsTutor = true;
            }
            else
            {
                ViewBag.IsTutor = false;
            }
            var result = _tutorService.Get(id);
            if (result.Success)
            {
                return View(result.Data);
            }
            return null;
        }
    }
}
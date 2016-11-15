using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz})]
    public class AcademicInstitutionController : BaseController
    {
        private readonly IAcademicInstitutionService _academicInstitutionService;

        public AcademicInstitutionController(IAcademicInstitutionService academicInstitutionService)
        {
            this._academicInstitutionService = academicInstitutionService;
        }

        // GET: Rackaz/AcademicInstitution
        public ActionResult Index()
        {
            var result = _academicInstitutionService.Get(CurrentUser.Area);
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Create()
        {
            var model = new AcademicInstitutionModel();
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsRackaz = false;
            }
            else
            {
                model.Area = CurrentUser.Area.Value;
                ViewBag.IsRackaz = true;
            }
            // TODO: Add the user area incase of him being a Rackz
            // TODO: change the viewbag to AcademicInstitutionVM to transfer area
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AcademicInstitutionModel model)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                model.Area = CurrentUser.Area.Value;   
            }
            var result =_academicInstitutionService.Add(model);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            return Error(new StatusModel(false,result.Message));
        }

        public ActionResult Edit(int id)
        {
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsRackaz = false;
            }
            else
            {
                ViewBag.IsRackaz = true;
            }
            var result = _academicInstitutionService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }

            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, AcademicInstitutionModel model)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                model.Area = CurrentUser.Area.Value;
            }
            var result = _academicInstitutionService.Update(id,model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return Error(result);
        }

        public ActionResult Details(int id)
        {
            var result = _academicInstitutionService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false,result.Message));
            }

            return View(result.Data);
        }
    }
}
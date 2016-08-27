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

        // TODO: /remove area int? from action only for testing replace with nothing
        // GET: Rackaz/AcademicInstitution
        public ActionResult Index(int? area = null)
        {
            var result = _academicInstitutionService.Get((Area?)area);
            return View(result);
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
            var result =_academicInstitutionService.Add(model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return null;
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

            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, AcademicInstitutionModel model)
        {
            var result = _academicInstitutionService.Update(id,model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return null;
        }

        public ActionResult Details(int id)
        {
            var result = _academicInstitutionService.Get(id);

            return View(result.Data);
        }
    }
}
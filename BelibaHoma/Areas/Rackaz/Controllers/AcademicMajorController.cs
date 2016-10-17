using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class AcademicMajorController : BaseController
    {
        private readonly IAcademicMajorService _academicMajorService;

        public AcademicMajorController(IAcademicMajorService academicMajorService)
        {
            this._academicMajorService = academicMajorService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index()
        {
            var result = _academicMajorService.Get();
            return View(result);
        }

        public ActionResult Create()
        {
            var model = new AcademicMajorModel {  };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(AcademicMajorModel model)
        {
            var result = _academicMajorService.Add(model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return null;
        }

        public ActionResult Edit(int id)
        {
            var result = _academicMajorService.Get(id);

            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, AcademicMajorModel model)
        {
            var result = _academicMajorService.Update(id, model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return null;
        }

    }
}
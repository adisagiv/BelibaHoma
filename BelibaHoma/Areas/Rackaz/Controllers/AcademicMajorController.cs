﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;
using Generic.Models;

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
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
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
            var status = new StatusModel(false,result.Message);
            return Error(status);
        }

        public ActionResult Edit(int id)
        {
            var result = _academicMajorService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
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
            return Error(new StatusModel(false,result.Message));
        }

    }
}
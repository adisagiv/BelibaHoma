﻿using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.GenericModel.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Tutor.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz, UserRole.Tutor })]
    public class TutorReportController : BaseController
    {
        private readonly ITutorReportService _TutorReportService;

        public TutorReportController(ITutorReportService TutorReportService)
        {
            this._TutorReportService = TutorReportService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index()
        {
            var result = _TutorReportService.Get();
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }


        //TODO: how to create index of only one specific TutorTraineeId
        public ActionResult TutorTraineeReports(int id)
        {
            var result = _TutorReportService.GetById(id);
            if (result.Success)
            {
                return View(result.Data);

            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }



        public ActionResult Create(int tutorTraineeId)
        {
            var model = new TutorReportModel { TutorTraineeId = tutorTraineeId};
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TutorReportModel model)
        {
            var result = _TutorReportService.Add(model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Edit(int id)
        {
            var result = _TutorReportService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, TutorReportModel model)
        {
            var result = _TutorReportService.Update(id, model);
            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            return Error(new StatusModel(false, result.Message));
        }

    }
}
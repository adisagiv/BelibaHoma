﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class JobOfferController : BaseController //Controller ?? to add?
    {
        private readonly IJobOfferService _JobofferService;
        private readonly IAcademicMajorService _academicMajorService;

        public JobOfferController(IJobOfferService JobofferService, IAcademicMajorService academicMajorService)
        {
            this._JobofferService = JobofferService;
            _academicMajorService = academicMajorService;
        }

        // TODO: /remove area int? from action only for testing replace with nothing
        // GET: Rackaz/AcademicInstitution
        public ActionResult Index()
        {
            var result = _JobofferService.Get();
            return View(result.Data);
        }

        public ActionResult Create()
        {
            var model = new JobOfferViewModel();
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsRackaz = false;
            }
            else
            {
                //model.Area = CurrentUser.Area.Value;  //NOT WORKING!!
                ViewBag.IsRackaz = true;
            }
            // TODO: Add the user area incase of him being a Rackz


            // TODO: change the viewbag to AcademicInstitutionVM to transfer area
            model.AcademicMajors = _academicMajorService.Get();


            return View(model);
        }

        [HttpPost]
        public ActionResult Create(JobOfferViewModel model)
        {
            var result = _JobofferService.Add(model.JobOffer);

            if (result.Success)
            {
                return RedirectToAction("Index", "JobOffer", new { area = "Trainee" });
            }

            return null;
        }

        public ActionResult Edit(int id)
        {
            var model = new JobOfferViewModel();
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsRackaz = false;
            }
            else
            {
                ViewBag.IsRackaz = true;
            }
            var result1 = _JobofferService.Get(id);
            //var result2 = _academicMajorService.Get();

            //  Validate that the requierd job offer was pulled from the DB (result.Success)
            //we think it should be that way: is it correct?

            if (result1.Success)//TODO: && resul2.Sucess)
            {
              model.JobOffer = result1.Data;
              model.AcademicMajors = _academicMajorService.Get(); //TODO: Adi needs to ensoure that data is good to go! model.succes to AM
              return View(model);
            }

            var result = new StatusModel(result1.Success, result1.Message); //TODO: this is an example for redirect to error page!
            return Error(result);
        }

        [HttpPost]
        public ActionResult Edit(int id, JobOfferViewModel model) //TODO: viewmodel
        {
            var result = _JobofferService.Update(id, model.JobOffer); // TODO: model.jobOffer

            if (result.Success)
            {
                return RedirectToAction("Index", "JobOffer", new { area ="Trainee" });
            }

            return null;
        }

        public ActionResult Details(int id)
        {
            var result = _JobofferService.Get(id);

            return View(result.Data);
        }
    }
}
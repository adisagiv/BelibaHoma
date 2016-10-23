
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Admin.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class JobOfferController : BaseController //Controller ?? to add?
    {
        private readonly IJobOfferService _JobofferService;

        public JobOfferController(IJobOfferService JobofferService)
        {
            this._JobofferService = JobofferService;
        }

        // TODO: /remove area int? from action only for testing replace with nothing
        // GET: Rackaz/AcademicInstitution
        public ActionResult Index(int? Jobarea = null) //remove? because admin not restricted to area?
        {
            var result = _JobofferService.Get((JobArea?)Jobarea);
            return View(result);
        }

        public ActionResult Create()
        {
            var model = new JobOfferModel();
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsAdmin = false;
            }
            else
            {
                //model.Area = CurrentUser.Area.Value;  //NOT WORKING!!
                ViewBag.IsAdmin = true;
            }
            // TODO: Add the user area incase of him being a Rackz


            // TODO: change the viewbag to AcademicInstitutionVM to transfer area

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(JobOfferModel model)
        {
            var result = _JobofferService.Add(model);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }

            return null;
        }

        public ActionResult Edit(int id)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                ViewBag.IsAdmin = false;
            }
            else
            {
                ViewBag.IsAdmin = true;
            }
            var result = _JobofferService.Get(id);

            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, JobOfferModel model)
        {
            var result = _JobofferService.Update(id, model);

            if (result.Success)
            {
                return RedirectToAction("Index");
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
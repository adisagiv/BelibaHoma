using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.Areas.Rackaz.Models;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Generic.Models;

namespace BelibaHoma.Areas.Trainee.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Trainee, UserRole.Admin, UserRole.Rackaz })] 
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
        public ActionResult Index()
        {

            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsTrainee = false;
            }
            else if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                ViewBag.IsTrainee = false;
            }
            else if (CurrentUser.UserRole == UserRole.Trainee)
            {
                ViewBag.IsTrainee = true;
            }

            var result = _JobofferService.Get();
            if (!result.Success)
            {
                return Error(result);
            }
            return View(result.Data);
        }



        public ActionResult Details(int id)
        {
            var result = _JobofferService.Get(id);
            if (!result.Success)
            {
                return Error(result);
            }
            if (CurrentUser.UserRole == UserRole.Admin)
            {
                ViewBag.IsTrainee = false;
            }
            else if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                ViewBag.IsTrainee = false;
            }
            else if (CurrentUser.UserRole == UserRole.Trainee)
            {
                ViewBag.IsTrainee = true;
            }
            return View(result.Data);
        }
    }
}
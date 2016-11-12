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
    public class TutorTraineeController : BaseController
    {
        private readonly ITutorTraineeService _tutorTraineeService;
      

        public TutorTraineeController(ITutorTraineeService tutorTraineeService)
        {
            this._tutorTraineeService = tutorTraineeService;
    
        }

        // GET: Relationship
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
            var result = _tutorTraineeService.Get(CurrentUser.Area);
            return View(result);
        }

        //public ActionResult Create()
        //{
        //    ViewBag.IsCreate = true;
        //    TutorViewModel model = new TutorViewModel
        //    {
        //        AcademicInstitutionList = _academicInstitutionService.Get(CurrentUser.Area),
        //        AcademicMajorList = _academicMajorService.Get(),
        //        Tutor = new TutorModel(),
        //    };
        //    model.Tutor.User = new UserModel();
        //    if (CurrentUser.UserRole == UserRole.Admin)
        //    {
        //        ViewBag.IsRackaz = false;
        //    }
        //    else
        //    {
        //        model.Tutor.User.Area = CurrentUser.Area;
        //        ViewBag.IsRackaz = true;
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Create(TutorModel model)
        //{
        //    if (CurrentUser.UserRole == UserRole.Rackaz)
        //    {
        //        model.User.Area = CurrentUser.Area;
        //    }
        //    var result = _tutorService.Add(model);

        //    return Json(result);
        //}


        //public ActionResult Edit(int id)
        //{
        //    TutorViewModel model = new TutorViewModel
        //    {
        //        AcademicInstitutionList = _academicInstitutionService.Get(CurrentUser.Area),
        //        AcademicMajorList = _academicMajorService.Get(),
        //        Tutor = new TutorModel()
        //    };
        //    ViewBag.IsRackaz = CurrentUser.UserRole == UserRole.Rackaz;
        //    ViewBag.IsCreate = false;
        //    var result = _tutorService.Get(id);
        //    if (result.Success)
        //    {
        //        model.Tutor = result.Data;
        //    }

        //    return View(model);
        //}

        //[HttpPost]
        //public ActionResult Edit(int id, TutorModel model)
        //{
        //    var result = _tutorService.Update(id, model);

        //    return Json(result);
        //}

        //public ActionResult Details(int id)
        //{
        //    var result = _tutorService.Get(id);
        //    if (result.Success)
        //    {
        //        return View(result.Data);
        //    }
        //    return null;
        //}
    }
}


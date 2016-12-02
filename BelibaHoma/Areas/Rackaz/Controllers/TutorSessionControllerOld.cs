using BelibaHoma.BLL.Enums;
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

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })] //TODO: userole=Tutor
    public class TutorSessionController : BaseController
    {
        private readonly ITutorSessionService _TutorSessionService;

        public TutorSessionController(ITutorSessionService TutorSessionService)
        {
            this._TutorSessionService = TutorSessionService;
        }

        // GET: Rackaz/AcademicMajor
        public ActionResult Index()
        {
            var result = _TutorSessionService.Get();
            if (result.Success)
            {
                return View(result.Data);
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Create()
        {
            var model = new TutorSessionModel { };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TutorSessionModel model)
        {
            var result = _TutorSessionService.Add(model);

            if (result.Success)
            {
                //TODO: Change Index to other action
                return RedirectToAction("Index");
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Edit(int id)
        {
            var result = _TutorSessionService.Get(id);
            if (!result.Success)
            {
                return Error(new StatusModel(false, result.Message));
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, TutorSessionModel model)
        {
            var result = _TutorSessionService.Update(id, model);
            if (result.Success)
            {
                //TODO: Change Index to other action
                return RedirectToAction("Index");
            }
            return Error(new StatusModel(false, result.Message));
        }

    }
}
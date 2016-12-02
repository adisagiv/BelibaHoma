using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.Models;

namespace BelibaHoma.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz,UserRole.Trainee,UserRole.Tutor })]
    public class ChangePasswordController : BaseController
    {
        private readonly IUserService _userService;

        public ChangePasswordController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: ChangePassword
        public ActionResult Index()
        {
            var model = new ChangePasswordViewModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ChangePasswordViewModel model)
        {
            var status = _userService.ChangePassword(CurrentUser.Id, model.CurrentPassword, model.NewPassword, model.ReTypePassword);

            if (status.Success)
            {
                SetUserLastPasswordUpdate(CurrentUser.Id, status.Data);
                RemoveAuthCookie();
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("NewPassword", status.Message);
            return View(model);

        }
    }
}
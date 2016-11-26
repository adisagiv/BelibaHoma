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
    [CustomAuthorization(UserRoles = new UserRole[] { UserRole.Admin, UserRole.Rackaz })]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        // GET: Rackaz/User
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
            var result = _userService.GetAdminAndRackaz();
            if (!result.Success)
            {
                var status = new StatusModel(false, result.Message);
                return Error(status);
            }
            return View(result.Data);
        }

        [HttpPost]
        public ActionResult ZeroPassword(int id)
        {
            var status = _userService.ZeroPassword(id);

            if (status.Success)
            {
                SetUserLastPasswordUpdate(id);
            }

            return Json(status);
        }
    }
}
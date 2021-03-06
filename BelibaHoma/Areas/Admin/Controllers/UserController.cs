﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.Controllers;
using Extensions.DateTime;
using Generic.Models;


namespace BelibaHoma.Areas.Admin.Controllers
{
    [CustomAuthorization(UserRoles = new UserRole[] {UserRole.Admin})]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.IsCreate = true;
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(UserModel model)
        {
            var result = _userService.Add(model);

            if (result.Success)
            {
                return RedirectToAction("Index","User", new {Area = "Rackaz"});
            }
            var status = new StatusModel(false, result.Message);
            return Error(status);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.IsCreate = false;
            var result = _userService.Get(id);

            return View(result.Data);
        }

        [HttpPost]
        public ActionResult Edit(int id, UserModel model)
        {
            var result = _userService.Update(id, model);

            if (result.Success)
            {
                SetUserUpdate(id, DateTime.MinValue.Utc());
                return RedirectToAction("Index", "User", new { Area = "Rackaz" });
            }

            return Error(result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public LoginController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        // GET: Login
        public ActionResult Index(string urlRedirect)
        {
            var model = new LoginModel {UrlRedirect = urlRedirect};

            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            var result  = _authenticationService.Authenticate(model);

            if (result.Success)
            {
                var cookie = _authenticationService.CreateAuthenticationTicket(result.Data, model.RememberMe);
                Response.Cookies.Add(cookie);
                if (model.UrlRedirect == null)
                {
                    model.UrlRedirect = String.Format("/{0}/Home", result.Data.UserRole.ToString());
                }
            }
            
            var status = new StatusModel<string>(result.Success, result.Message, model.UrlRedirect);

            return Json(status);

        }

        public ActionResult Logout()
        {
            RemoveAuthCookie();
            
            return RedirectToAction("Index");
        }
    }
}
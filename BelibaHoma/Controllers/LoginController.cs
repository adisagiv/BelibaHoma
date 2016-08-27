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
    public class LoginController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogService _logService;

        public LoginController(IAuthenticationService authenticationService,ILogService logService)
        {
            _authenticationService = authenticationService;
            _logService = logService;
        }

        // GET: Login
        public ActionResult Index(string urlRedirect)
        {
            _logService.Logger.Info("tese");
            _logService.Logger.Error("error test");
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
            var authCookie = Response.Cookies.Get("AuthCookie");
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1); // make it expire yesterday
                Response.Cookies.Add(authCookie); // overwrite it
                Session.Abandon();
            }
            
            return RedirectToAction("Index");
        }
    }
}
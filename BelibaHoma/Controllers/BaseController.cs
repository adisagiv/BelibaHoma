using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Generic.Models;
using Ninject;

namespace BelibaHoma.Controllers
{
    public class BaseController : Controller
    {
        #region protected

        private readonly IAuthenticationService _authenticationService;
        private UserModel _currentUser;
        

        #endregion


        protected UserModel CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }

        public BaseController()
        {
            _authenticationService = new AuthenticationService();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authTicket = _authenticationService.GetAuthentication(Request);

            if (authTicket != null)
            {
                _currentUser = new UserModel(authTicket);

                ViewBag.CurrentUser = CurrentUser;
            }
           


            base.OnActionExecuting(filterContext);
        }

        public ActionResult Error(StatusModel status)
        {
            return View("~/Views/Shared/Error.cshtml",status);
        }
    }
}
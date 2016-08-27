using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;

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
    }
}
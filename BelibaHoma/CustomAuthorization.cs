﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using WebGrease;

namespace BelibaHoma
{
    public class CustomAuthorization : AuthorizeAttribute
    {
        private readonly IAuthenticationService _authenticationService;
        //private static ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserRole[] UserRoles { get; set; }

        public CustomAuthorization()
        {
            _authenticationService = new AuthenticationService();

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");


            var authenticationLevel = _authenticationService.GetAuthentication(httpContext.Request);


            if (authenticationLevel != null)
            {
                var user = new UserModel(authenticationLevel);
               // _logger.Info(String.Format("after getting user from auth id:{0}, name: {1}", login_model.ID, login_model.CellPhone));
                if (UserRoles.Contains(user.UserRole))
                {

                    return true;
                }
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                var url = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(@"~/Login/Index?urlRedirect=" + url);
            }
        }
    }
}
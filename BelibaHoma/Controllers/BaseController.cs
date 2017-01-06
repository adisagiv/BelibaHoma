using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using System.Web.Routing;
using System.Web.Security;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Services;
using Extensions.DateTime;
using Generic.Models;
using Ninject;

namespace BelibaHoma.Controllers
{
    public class BaseController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private UserModel _currentUser;
        private static readonly Dictionary<int, long?> LastPasswordsUpdate = new Dictionary<int, long?>();
        private static readonly  Dictionary<int,long> LastUserUpdate = new Dictionary<int, long>(); 
        
        #region protected
        protected UserModel CurrentUser
        {
            get
            {
                return _currentUser;
            }
        }
        #endregion

        public BaseController()
        {
            _authenticationService = new AuthenticationService();
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var authTicket = _authenticationService.GetAuthentication(Request);

            if (authTicket != null)
            {
                try
                {
                    _currentUser = new UserModel(authTicket);
                    long? updatedPassword = null;
                    long lastUserUpdate;

                    if (LastPasswordsUpdate.ContainsKey(CurrentUser.Id))
                    {
                        updatedPassword = LastPasswordsUpdate[CurrentUser.Id];
                    }
                    else
                    {
                        var status = _authenticationService.GetLastPasswordUpdate(CurrentUser.Id);

                        if (status.Success)
                        {
                            updatedPassword = status.Data;

                        }
                        else
                        {
                            updatedPassword = null;
                        }

                        SetUserLastPasswordUpdate(CurrentUser.Id, updatedPassword);
                    }

                    if (LastUserUpdate.ContainsKey(CurrentUser.Id))
                    {
                        lastUserUpdate = LastUserUpdate[CurrentUser.Id];
                    }
                    else
                    {
                        var status = _authenticationService.GetLastUserUpdate(CurrentUser.Id);

                        if (status.Success)
                        {
                            lastUserUpdate = status.Data.Utc();

                        }
                        else
                        {
                            lastUserUpdate = DateTime.MinValue.Utc();
                        }

                        SetUserUpdate(CurrentUser.Id, lastUserUpdate);
                    }

                    // TODO : Add _lastPasswordsUpdate to cookie as unix time to check next time user enter site need to reset password
                    if (updatedPassword == null && !filterContext.IsChildAction && !(filterContext.ActionDescriptor.ActionName == "Index" && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName == "ChangePassword"))
                    {
                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                        {
                            {"controller", "ChangePassword"},
                            {"action", "Index"},
                            { "area" , ""}
                        });
                    }

                    if (updatedPassword != null && updatedPassword != CurrentUser.LastPasswordUpdate ||
                        lastUserUpdate == DateTime.MinValue.Utc() || lastUserUpdate != CurrentUser.UpdateTime.Utc())
                    {
                        RemoveAuthCookie();

                        filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary
                        {
                            {"controller", "Login"},
                            {"action", "Index"},
                            { "area" , ""}
                        });
                    }


                    ViewBag.CurrentUser = CurrentUser;
                    if (CurrentUser.UserRole == UserRole.Rackaz)
                    {
                        ViewBag.IsRackaz = true;
                    }
                    else
                    {
                        ViewBag.IsRackaz = false;
                    }
                }
                catch (Exception)
                {
                    // if we can't parse user we send him to login page
                    RemoveAuthCookie();

                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {"controller", "Login"},
                            {"action", "Index"},
                            { "area" , ""}
                        });
                }
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetUserUpdate(int id, long lastUserUpdate)
        {
            LastUserUpdate[id] = lastUserUpdate;
        }

        protected void SetUserLastPasswordUpdate(int id, long? passwordUpdate = null)
        {
            LastPasswordsUpdate[id] = passwordUpdate;
        }

        protected void RemoveAuthCookie()
        {
            var authCookie = Response.Cookies.Get("AuthCookie");
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1); // make it expire yesterday
                Response.Cookies.Add(authCookie); // overwrite it
                Session.Abandon();
            }
        }

        public ActionResult Error(StatusModel status, bool isPartial = false)
        {
            if (!isPartial)
            {
                return View("~/Views/Shared/Error.cshtml", status);    
            }

            return PartialView("~/Views/Shared/_Error.cshtml",status);
        }
    }
}
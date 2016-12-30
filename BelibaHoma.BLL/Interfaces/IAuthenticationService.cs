using System;
using System.Net;
using System.Web;
using System.Web.Security;
using BelibaHoma.BLL.Models;
using Generic.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface IAuthenticationService
    {
        StatusModel<UserModel> Authenticate(LoginModel model);

        HttpCookie CreateAuthenticationTicket(UserModel user, bool remeberMe);
        FormsAuthenticationTicket GetAuthentication(HttpRequestBase request);

        StatusModel<long?> GetLastPasswordUpdate(int id);
        StatusModel<DateTime> GetLastUserUpdate(int id);
    }
}
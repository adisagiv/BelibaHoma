using System;
using System.Linq;
using System.Web;
using System.Web.Security;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using Generic.Models;

namespace BelibaHoma.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public StatusModel<UserModel> Authenticate(LoginModel model)
        {
            var result = new StatusModel<UserModel>(false, "�� ����� �� ����� �� ������", null);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var userEntity = userRepository.GetAll().SingleOrDefault(u => u.IdNumber == model.Username && u.Password == model.Password);

                    if (userEntity != null)
                    {
                        var user = new UserModel(userEntity);

                        result = new StatusModel<UserModel>(true, "", user);
                    }
                }
            }
            catch (Exception ex)
            {
                //  TODO : Handle exception
                result.Success = false;
                result.Message = "";
            }
            

            return result;
        }

        public HttpCookie CreateAuthenticationTicket(UserModel user,bool rememberMe)
        {
            try
            {
                string[] userDetailsArray = new string[] { user.UserRole.ToString(), user.LastName, user.FirstName, user.Area.ToString() };
                string userDetails = CreateDetailsString(userDetailsArray);

                FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                    user.Id,
                    user.IdNumber,
                    DateTime.Now,
                    DateTime.MaxValue,
                    rememberMe,
                    userDetails);
                string encTicket = FormsAuthentication.Encrypt(authTicket);
                HttpCookie faCookie = new HttpCookie("AuthCookie", encTicket);
                if (!rememberMe)
                {
                    faCookie.Expires = authTicket.Expiration;
                }

                return faCookie;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string CreateDetailsString(string[] userDetails)
        {
            string unitedString = String.Join(";", userDetails);
            return unitedString;
        }

        public FormsAuthenticationTicket GetAuthentication(HttpRequestBase request)
        {
            var authCookie = request.Cookies["AuthCookie"];
            try
            {
                if (authCookie != null)
                {
                    var authenticationLevel = FormsAuthentication.Decrypt(authCookie.Value);
                    return authenticationLevel;

                }
            }
            catch (Exception ex)
            {
                //_logger.Error("error getting authentication ticket from request: {0}.", ex);
                return null;
            }

            return null;

        }

    }
}
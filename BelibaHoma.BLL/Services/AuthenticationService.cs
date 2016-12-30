using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using Generic.Models;
using log4net;
using Services.Log;
using Extensions.DateTime;

namespace BelibaHoma.BLL.Services
{
    public class AuthenticationService : IAuthenticationService
    {

        public StatusModel<UserModel> Authenticate(LoginModel model)
        {
            var result = new StatusModel<UserModel>(false, "שם משתמש או סיסמא לא נכונים", null);

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
                LogService.Logger.Error(result.Message, ex);
            }
            

            return result;
        }

        public HttpCookie CreateAuthenticationTicket(UserModel user,bool rememberMe)
        {
            try
            {
                string[] userDetailsArray = new string[] { user.LastPasswordUpdate == null ? user.LastPasswordUpdate.ToString() : user.LastPasswordUpdate.Value.ToString(), user.UserRole.ToString(), user.LastName, user.FirstName, user.Area.ToString(), user.UpdateTime.Utc().ToString() };
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
                LogService.Logger.Error(String.Format("Unable to create authentication cookie"), ex);
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
                ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                logger.Error(string.Format("Error getting authentication ticket: {0}", ex));
                return null;
            }

            return null;

        }

        public StatusModel<long?> GetLastPasswordUpdate(int id)
        {
            var result = new StatusModel<long?>(false, string.Empty, null);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var userEntity = userRepository.GetByKey(id);

                    if (userEntity != null)
                    {

                        result = new StatusModel<long?>(true, "", userEntity.LastPasswordUpdate);
                    }
                }
            }
            catch (Exception ex)
            {
                //  TODO : Handle exception
                result.Success = false;
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

        public StatusModel<DateTime> GetLastUserUpdate(int id)
        {
            var result = new StatusModel<DateTime>(false, string.Empty, DateTime.MinValue);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var userEntity = userRepository.GetByKey(id);

                    if (userEntity != null)
                    {

                        result = new StatusModel<DateTime>(true, "", userEntity.UpdateTime);
                    }
                }
            }
            catch (Exception ex)
            {
                //  TODO : Handle exception
                result.Success = false;
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }
    }
}
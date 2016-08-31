using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Get a list of all admins and Rackaz
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAdminAndRackaz()
        {
            var result = new List<UserModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    
                    result = userRepository.GetAll().Where(u => u.UserRole < 2).OrderBy(u => u.Area).ToList().Select(u => new UserModel(u)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting Admins and Rackazes from DB");
                LogService.Logger.Error(message, ex);
            }


            return result;
        }


        public StatusModel Add(UserModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    if (model.UserRole.ToString() == "Rackaz" && model.Area == null)
                    {
                        throw new System.ArgumentException("User is Rackaz but Area is not initialized", "model");
                    }
                    if (model.UserRole.ToString() == "Admin" && model.Area != null)
                    {
                        throw new System.ArgumentException("User is Admin but Area is initialized", "model");
                    }
                    if (model.IdNumber.Length != 9)
                    {
                        throw new System.ArgumentException("User Id Number is invalid", "model");
                    }
                    model.CreationTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.IsActive = true;
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    var entity = model.MapTo<User>();
                    userRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("המשתמש {0} הוזן בהצלחה", model.FullName);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת המשתמש");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}
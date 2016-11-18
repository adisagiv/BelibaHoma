using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public StatusModel<List<UserModel>> GetAdminAndRackaz()
        {
            var result = new StatusModel<List<UserModel>>(false, String.Empty, new List<UserModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    
                    result.Data = userRepository.GetAll().Where(u => u.UserRole < 2).OrderBy(u => u.Area).ToList().Select(u => new UserModel(u)).ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת רכזים ואדמינים ממסד הנתונים");
                LogService.Logger.Error(result.Message, ex);
            }
            return result;
        }


        public StatusModel<int> Add(UserModel model)
        {
            var status = new StatusModel<int>(false, String.Empty, 0);

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
                    model.LastPasswordUpdate = DateTime.Now;
                    model.IsActive = true;
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    var entity = model.MapTo<User>();
                    userRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("המשתמש {0} הוזן בהצלחה", model.FullName);
                    status.Data = entity.Id;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת המשתמש");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Update a User in db
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public StatusModel Update(int id, UserModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var user = userRepository.GetByKey(id);
                    if (user != null)
                    {
                        if (updatedModel.UserRole.ToString() == "Rackaz" && updatedModel.Area == null)
                        {
                            throw new System.ArgumentException("User is Rackaz but Area is not initialized", "model");
                        }
                        if (updatedModel.UserRole.ToString() == "Admin" && updatedModel.Area != null)
                        {
                            throw new System.ArgumentException("User is Admin but Area is initialized", "model");
                        }
                        if (updatedModel.IdNumber.Length != 9)
                        {
                            throw new System.ArgumentException("User Id Number is invalid", "model");
                        }
                        user.FirstName = updatedModel.FirstName;
                        user.LastName = updatedModel.LastName;
                        //TODO: ask Roey if needed here - user.Password = updatedModel.Password;
                        user.UserRole = (int)updatedModel.UserRole;
                        user.Email = updatedModel.Email;
                        user.IdNumber = updatedModel.IdNumber;
                        user.IsActive = updatedModel.IsActive;
                        user.UpdateTime = DateTime.Now;
                        user.Area = (int?)updatedModel.Area;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("פרטי המתשמש {0} עודכנו בהצלחה", updatedModel.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון פרטי המתשתמש");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Get User by ID (index/key) from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<UserModel> Get(int id)
        {
            var status = new StatusModel<UserModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var user = userRepository.GetByKey(id);

                    status.Data = new UserModel(user);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. המשתמש אינו קיים במערכת.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}
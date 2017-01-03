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
using Extensions.DateTime;
using Generic.Models;
using Services.Log;
using Extensions.DateTime;

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
                        status.Message = "המשתמש המוזן הינו רכז אך לא הוזן עבורו אזור פעילות";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (model.UserRole.ToString() == "Admin" && model.Area != null)
                    {
                        status.Message = "המשתמש המוזן הינו אדמין אך הוזן עבורו אזור פעילות";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if ((model.UserRole == UserRole.Trainee || model.UserRole == UserRole.Tutor) && model.Area == null)
                    {
                        status.Message = "המשתמש המוזן הינו מסוג חניך / חונך אך לא הוזן עבורו אזור פעילות";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (model.IdNumber.Length != 9)
                    {
                        status.Message = "נא להזין תעודת זהות תקינה בעלת 9 ספרות (כולל ספרת ביקורת)";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();
                    var IsIdExist = userRepository.GetAll().FirstOrDefault(u => u.IdNumber == model.IdNumber);
                    if (IsIdExist != null)
                    {
                        status.Message = "על תעודת הזהות להיות ערך ייחודי, קיים משתמש במסד בעל תעודת זהות זהה";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    model.CreationTime = DateTime.Now;
                    model.UpdateTime = DateTime.Now;
                    model.LastPasswordUpdate = DateTime.Now.Utc();
                    model.IsActive = true;
                    
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
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת המשתמש");    
                }
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
            var status = new StatusModel<DateTime>(false, String.Empty, DateTime.MinValue);

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
                            status.Message = "המשתמש המוזן הינו רכז אך לא הוזן עבורו אזור פעילות";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (updatedModel.UserRole.ToString() == "Admin" && updatedModel.Area != null)
                        {
                            status.Message = "המשתמש המוזן הינו אדמין אך הוזן עבורו אזור פעילות";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if ((updatedModel.UserRole == UserRole.Trainee || updatedModel.UserRole == UserRole.Tutor) && updatedModel.Area == null)
                        {
                            status.Message = "המשתמש המוזן הינו מסוג חניך / חונך אך לא הוזן עבורו אזור פעילות";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (updatedModel.IdNumber.Length != 9)
                        {
                            status.Message = "נא להזין תעודת זהות תקינה בעלת 9 ספרות (כולל ספרת ביקורת)";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (user.IdNumber != updatedModel.IdNumber)
                        {
                            var IsIdExist = userRepository.GetAll().FirstOrDefault(u => u.IdNumber == updatedModel.IdNumber);
                            if (IsIdExist != null && IsIdExist.Id != id)
                            {
                                status.Message = "תקלה בעת ניסיון לשנות את ערך תעודת הזהות של המשתמש הקיים.\nבמסד הנתונים משתמש נוסף בעל אותו מספר תעודת זהות.";
                                throw new System.ArgumentException(status.Message, "updatedModel");
                            }   
                        }
                        user.FirstName = updatedModel.FirstName;
                        user.LastName = updatedModel.LastName;
                        user.UserRole = (int)updatedModel.UserRole;
                        user.Email = updatedModel.Email;
                        user.IdNumber = updatedModel.IdNumber;
                        user.IsActive = updatedModel.IsActive;
                        user.UpdateTime = DateTime.Now;
                        user.Area = (int?)updatedModel.Area;

                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("פרטי המתשמש {0} עודכנו בהצלחה", updatedModel.FullName);
                        status.Data = user.UpdateTime;
                    }
                }
            }
            catch (Exception ex)
            {
                if (status.Message == string.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך עדכון פרטי המתשתמש");   
                }
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


        public StatusModel<long> ChangePassword(int id, string currentPassword, string newPassword, string reTypePassword)
        {
            var status = new StatusModel<long>(false, String.Empty, 0);

            if (currentPassword == newPassword)
            {
                status.Message = String.Format("אנא בחר סיסמא שונה מהסיסמא הנוחכית");
                return status;
            }
            else if (newPassword != reTypePassword)
            {
                status.Message = String.Format("הסיסמא לא תואמת את הסיסמא שהוזנה");
                return status;
            }

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var user = userRepository.GetByKey(id);

                    if (user.Password != currentPassword)
                    {
                        status.Message = String.Format("הסיסמא הנוחכית אינה תואמת את הסיסמא השמורה במערכת");
                        return status;
                    }

                    user.Password = newPassword;
                    user.LastPasswordUpdate = DateTime.Now.Utc();

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Data = user.LastPasswordUpdate.Value;


                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. המשתמש אינו קיים במערכת.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel ZeroPassword(int id)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    var user = userRepository.GetByKey(id);

                    user.LastPasswordUpdate = null;

                    var newPassword = Guid.NewGuid().ToString().Substring(0, 6);

                    user.Password = newPassword;

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("{0} : העבר סיסמא  חדשה זו למשתמש", newPassword);
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
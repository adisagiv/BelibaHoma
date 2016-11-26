using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class TutorService : ITutorService
    {
        private readonly IUserService _userService;

        public TutorService(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get list of all tutors from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public StatusModel<List<TutorModel>> GetTutors(Area? area)
        {
            var result = new StatusModel<List<TutorModel>>(false, String.Empty, new List<TutorModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    result.Data =
                        tutorRepository.GetAll()
                            .Where(t => (!area.HasValue || t.User.Area == (int) area.Value) && t.User.UserRole == 2)
                            .OrderBy(t => t.User.Area)
                            .ThenBy(t => t.User.LastName)
                            .ThenBy(t => t.User.FirstName)
                            .ToList()
                            .Select(t => new TutorModel(t))
                            .ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת החונכים מ");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Add new Tutor
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public StatusModel Add(TutorModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    //Updating the User Role in the model
                    model.User.UserRole = UserRole.Tutor;

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();
                    var academicInstitution = academicInstitutionRepository.GetByKey(model.AcademicInstitution.Id);

                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();
                    var academicMajor = academicMajorRepository.GetByKey(model.AcademicMajor.Id);
                    var academicMajor1 = academicMajorRepository.GetByKey(model.AcademicMajor1.Id);

                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    //Running some server side validations
                    if (model.Birthday > DateTime.Now.AddYears(-15))
                    {
                        throw new System.ArgumentException("Tutor Birthday doesn't make sense", "model");
                    }
                    if (academicInstitution.InstitutionType == (int)InstitutionType.Mechina && model.AcademicYear != 0 && model.SemesterNumber != 0)
                    {
                        throw new System.ArgumentException("Tutor is in Mechina, Academic Year and Semester should be 0", "model");
                    }
                    if (academicInstitution.InstitutionType != (int)InstitutionType.Mechina && (model.AcademicYear == 0 || model.SemesterNumber == 0))
                    {
                        throw new System.ArgumentException("Trainee is in Mechina, Academic Year and Semester should be 0", "model");
                    }

                    //Adding the User Model to the DB (By using the Add function in UserService)
                    var userStatus = _userService.Add(model.User);
                    if (userStatus.Success)
                    {
                        //Get the new user entity from DB (also linked to Tutor)
                        var user = userRepository.GetByKey(userStatus.Data);

                        //Updating "not input" fields in Tutor model
                        model.TutorHours = 0;
                        model.TutorHoursBonding = 0;

                        //Mapping the model into Tutor Entity
                        var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                        var entity = model.MapTo<Tutor>();

                        //Linking the Complexed entities to the retrieved ones
                        entity.AcademicInstitution = academicInstitution;
                        entity.AcademicMajor = academicMajor;
                        entity.AcademicMajor1 = academicMajor1;
                        entity.User = user;

                        entity.AcademicInstitutionId = academicInstitution.Id;
                        entity.AcademicMajorId = academicMajor.Id;
                        entity.AcademicMinorId = academicMajor1.Id;
                        entity.UserId = user.Id;

                        //Finally Adding the entity to DB
                        tutorRepository.Add(entity);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("חונך {0} הוזן בהצלחה", model.User.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת החונך");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Get Tutor by the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<TutorModel> Get(int id)
        {
            var status = new StatusModel<TutorModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();

                    var tutor = tutorRepository.GetByKey(id);

                    status.Data = new TutorModel(tutor);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא החונך המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Update tutor in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public StatusModel Update(int id, TutorModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                    var academicInstitution =
                        academicInstitutionRepository.GetByKey(updatedModel.AcademicInstitution.Id);
                    var academicMajor = academicMajorRepository.GetByKey(updatedModel.AcademicMajor.Id);
                    var academicMajor1 = academicMajorRepository.GetByKey(updatedModel.AcademicMajor1.Id);
                   

                    var tutor = tutorRepository.GetByKey(id);
                    if (tutor != null)
                    {
                        //Running some server side validations
                        if (updatedModel.User.IdNumber.Length != 9)
                        {
                            throw new System.ArgumentException("User Id Number is invalid", "updatedModel");
                        }
                        if (updatedModel.Birthday > DateTime.Now.AddYears(-15))
                        {
                            throw new System.ArgumentException("Tutor Birthday doesn't make sense", "updatedModel");
                        }
                        if (academicInstitution.InstitutionType == (int)InstitutionType.Mechina && updatedModel.AcademicYear != 0 && updatedModel.SemesterNumber != 0)
                        {
                            throw new System.ArgumentException("Tutor is in Mechina, Academic Year and Semester should be 0", "updatedModel");
                        }
                        if (academicInstitution.InstitutionType != (int)InstitutionType.Mechina && (updatedModel.AcademicYear == 0 || updatedModel.SemesterNumber == 0))
                        {
                            throw new System.ArgumentException("Tutor is in Mechina, Academic Year and Semester should be 0", "updatedModel");
                        }
                   

                        //Updating the entity from the model received by the form
                        tutor.User.FirstName = updatedModel.User.FirstName;
                        tutor.User.LastName = updatedModel.User.LastName;
                        tutor.User.Email = updatedModel.User.Email;
                        tutor.User.IdNumber = updatedModel.User.IdNumber;
                        tutor.User.IsActive = updatedModel.User.IsActive;
                        tutor.User.UpdateTime = DateTime.Now;
                        if (updatedModel.User.Area != null)
                        {
                            tutor.User.Area = (int?)updatedModel.User.Area;
                        }
                        tutor.Address = updatedModel.Address;
                        tutor.Gender = (int)updatedModel.Gender;
                        tutor.Birthday = updatedModel.Birthday;
                        tutor.AcademicYear = updatedModel.AcademicYear;
                        tutor.SemesterNumber = updatedModel.SemesterNumber;
                        tutor.PhoneNumber = updatedModel.PhoneNumber;
                        tutor.PhysicsLevel = (int)updatedModel.PhysicsLevel;
                        tutor.MathLevel = (int)updatedModel.MathLevel;
                        tutor.EnglishLevel = (int)updatedModel.EnglishLevel;
                       

                        //Linked Entities - need to verify Academic Institutions and Majors
                        tutor.AcademicInstitution = academicInstitution;
                        tutor.AcademicInstitutionId = academicInstitution.Id;
                        tutor.AcademicMajor = academicMajor;
                        tutor.AcademicMajorId = academicMajor.Id;
                        tutor.AcademicMajor1 = academicMajor1;
                        tutor.AcademicMinorId = academicMajor1.Id;
                     
                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("פרטי החונך {0} עודכנו בהצלחה", updatedModel.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך עדכון פרטי החונך");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }


        public StatusModel<List<TutorMatchViewModel>> GetUnMatchedTutors(Area area, bool showMatched)
        {
            var result = new StatusModel<List<TutorMatchViewModel>>(false, String.Empty, new List<TutorMatchViewModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();
                    var tutorList = tutorRepository.GetAll()
                        .Where(t => t.User.IsActive && t.User.Area == (int)area && t.User.UserRole == (int)UserRole.Tutor && (showMatched || t.TutorTrainee.All(tt => tt.Status == (int)TTStatus.InActive)))
                        .OrderBy(t => t.User.LastName).ThenBy(t => t.User.FirstName).ToList()
                        .Select(t => new TutorMatchViewModel(t, t.TutorTrainee.Count(tt => tt.Status == (int)TTStatus.Active), t.TutorTrainee.Count(tt => tt.Status == (int)TTStatus.UnApproved) > 0 ? 1 : 0)).ToList();

                    result = new StatusModel<List<TutorMatchViewModel>>(true, String.Empty, tutorList);
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת חונכים ממסד הנתונים");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }
        
    }
}
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
    public class TraineeService : ITraineeService
    {
        private readonly IPredictionTrainingService _predictionTrainingService;
        private readonly IUserService _userService;
        private readonly ITutorTraineeService _tutorTraineeService;

        public TraineeService(IUserService userService, IPredictionTrainingService predictionTrainingService,ITutorTraineeService tutorTraineeService)
        {
            _predictionTrainingService = predictionTrainingService;
            _userService = userService;
            _tutorTraineeService = tutorTraineeService;
        }


        /// <summary>
        /// Get list of all trainees from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public StatusModel<List<TraineeModel>> GetTrainees(Area? area)
        {

            var result = new StatusModel<List<TraineeModel>>(false, String.Empty, new List<TraineeModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    result.Data = traineeRepository.GetAll().Where(t => (!area.HasValue || t.User.Area == (int)area.Value) && t.User.UserRole == 3).OrderBy(t => t.User.Area).ThenBy(t => t.User.LastName).ThenBy(t => t.User.FirstName).ToList().Select(t => new TraineeModel(t)).ToList();
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת חניכים ממסד הנתונים");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }

        /// <summary>
        /// Add new Trainee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public StatusModel Add(TraineeModel model)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    //Updating the User Role in the model
                    model.User.UserRole = UserRole.Trainee;

                    //Retrieving Related Entities by using the repositories and GetById function (all but User which was not yet created)
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();
                    var academicInstitution = academicInstitutionRepository.GetByKey(model.AcademicInstitution.Id);

                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();
                    var academicMajor = academicMajorRepository.GetByKey(model.AcademicMajor.Id);
                    //academic major 1 & 2 can be null
                    var academicMajor1 = new AcademicMajor();
                    academicMajor1 = model.AcademicMajor1.Id != 0 ? academicMajorRepository.GetByKey(model.AcademicMajor1.Id) : null;
                    var academicMajor2 = new AcademicMajor();
                    academicMajor2 = model.AcademicMajor2.Id != 0 ? academicMajorRepository.GetByKey(model.AcademicMajor2.Id) : null;
                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    //Running some server side validations
                    if (model.User.IdNumber.Length != 9)
                    {
                        status.Message = "מספר תעודת הזהות צריך להכיל בדיוק 9 ספרות";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (model.Birthday > DateTime.Now.AddYears(-15))
                    {
                        status.Message = "תאריך הלידה של החניך צריך להיות לפחות לפני 15 שנים";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (academicInstitution.InstitutionType == (int) InstitutionType.Mechina && model.AcademicYear != 0 && model.SemesterNumber != 0)
                    {
                        status.Message = "החניך במכינה, שנת הלימודים ומספר הסמסטר צריכים להיות 0";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (academicInstitution.InstitutionType != (int)InstitutionType.Mechina && (model.AcademicYear == 0 || model.SemesterNumber == 0))
                    {
                        status.Message = "אם החניך איננו ממוסד מסוג מכינה, על מספר הסמסטר והשנה האקדמית להיות שונים מ-0";
                        throw new System.ArgumentException(status.Message, "model");
                    }
                    if (model.User.Area != null && academicInstitution.Area != (int)model.User.Area)
                    {
                        status.Message = "המוסד האקדמי של החונך נמצא באזור פעילות שונה מהאזור שהוזן לחונך";
                        throw new System.ArgumentException(status.Message, "model");
                    }

                    //Adding the User Model to the DB (By using the Add function in UserService)
                    var userStatus = _userService.Add(model.User);
                    if (userStatus.Success)
                    {
                        //Get the new user entity from DB (also linked to Trainee)
                        var user = userRepository.GetByKey(userStatus.Data);

                        //Updating "not input" fields in Trainee model
                        model.TutorHours = 0;
                        model.TutorHoursBonding = 0;
                        model.DroppedOut = false;

                        //Mapping the model into Trainee Entity
                        var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                        var entity = model.MapTo<Trainee>();

                        //Linking the Complexed entities to the retrieved ones
                        entity.AcademicInstitution = academicInstitution;
                        entity.AcademicMajor = academicMajor;
                        entity.AcademicMajor1 = academicMajor1;
                        entity.AcademicMajor2 = academicMajor2;
                        entity.User = user;

                        entity.AcademicInstitutionId = academicInstitution.Id;
                        entity.AcademicMajorId = academicMajor.Id;
                        if (model.AcademicMajor1.Id != 0)
                        {
                            entity.AcademicMinorId = academicMajor1.Id;
                        }
                        if (model.AcademicMajor2.Id != 0)
                        {
                            entity.AcademicMajorNeededHelpId = academicMajor2.Id;
                        }
                        entity.UserId = user.Id;

                        //Finally Adding the entity to DB
                        traineeRepository.Add(entity);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("חניך {0} הוזן בהצלחה", model.User.FullName);
                    }
                    else
                    {
                        status.Message = userStatus.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                if (status.Message == string.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך הוספת חניך");   
                }
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Get Trainee by the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusModel<TraineeModel> Get(int id)
        {
            var status = new StatusModel<TraineeModel>();

            try
            {
                status.Message = String.Empty;
                status.Success = false;

                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();

                    var trainee = traineeRepository.GetByKey(id);

                    status.Data = new TraineeModel(trainee);

                    status.Success = true;
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה. לא נמצא החניך המבוקש.");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Update trainee in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        public StatusModel Update(int id, TraineeModel updatedModel)
        {
            var status = new StatusModel(false, String.Empty);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var academicInstitutionRepository = unitOfWork.GetRepository<IAcademicInstitutionRepository>();
                    var academicMajorRepository = unitOfWork.GetRepository<IAcademicMajorRepository>();

                    var academicInstitution =
                        academicInstitutionRepository.GetByKey(updatedModel.AcademicInstitution.Id);
                    var academicMajor = academicMajorRepository.GetByKey(updatedModel.AcademicMajor.Id);
                    var academicMajor1 = new AcademicMajor();
                    academicMajor1 = updatedModel.AcademicMajor1.Id != 0 ? academicMajorRepository.GetByKey(updatedModel.AcademicMajor1.Id) : null;
                    var academicMajor2 = new AcademicMajor();
                    academicMajor2 = updatedModel.AcademicMajor2.Id != 0 ? academicMajorRepository.GetByKey(updatedModel.AcademicMajor2.Id) : null;

                    var trainee = traineeRepository.GetByKey(id);
                    if (trainee != null)
                    {
                        //Running some server side validations
                        if (updatedModel.User.IdNumber.Length != 9)
                        {
                            status.Message = "מספר תעודת הזהות צריך להכיל בדיוק 9 ספרות";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (updatedModel.Birthday > DateTime.Now.AddYears(-15))
                        {
                            status.Message = "תאריך הלידה של החניך צריך להיות לפחות לפני 15 שנים";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (academicInstitution.InstitutionType == (int)InstitutionType.Mechina && updatedModel.AcademicYear != 0 && updatedModel.SemesterNumber != 0)
                        {
                            status.Message = "החניך במכינה, שנת הלימודים ומספר הסמסטר צריכים להיות 0";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (academicInstitution.InstitutionType != (int)InstitutionType.Mechina && (updatedModel.AcademicYear == 0 || updatedModel.SemesterNumber == 0))
                        {
                            status.Message = "אם החניך איננו ממוסד מסוג מכינה, על מספר הסמסטר והשנה האקדמית להיות שונים מ-0";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (updatedModel.User.IsActive != false && updatedModel.DroppedOut == true)
                        {
                            status.Message = "במידה והחניך נשר מהתוכנית, הסטטוס שלו לא יכול להיות פעיל";
                            throw new System.ArgumentException(status.Message, "updatedModel");
                        }
                        if (updatedModel.User.Area != null && academicInstitution.Area != (int)updatedModel.User.Area)
                        {
                            status.Message = "המוסד האקדמי של החונך נמצא באזור פעילות שונה מהאזור שהוזן לחונך";
                            throw new System.ArgumentException(status.Message,"updatedModel");
                        }
                        if (updatedModel.User.IsActive == false && trainee.User.IsActive == true)
                        //{
                        //    var tutortraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();
                        //    var tutortrainees =
                        //        tutortraineeRepository.GetAll().Where(tt => tt.TraineeId == trainee.UserId && tt.Status == (int)TTStatus.Active).ToList();
                        //    if (tutortrainees[0] != null)
                        //    {
                        //        foreach (var tt in tutortrainees)
                        //        {

                        //        }
                        //    }

                        //}
                        //Updating the entity from the model received by the form
                        trainee.User.FirstName = updatedModel.User.FirstName;
                        trainee.User.LastName = updatedModel.User.LastName;
                        trainee.User.Email = updatedModel.User.Email;
                        trainee.User.IdNumber = updatedModel.User.IdNumber;
                        trainee.User.IsActive = updatedModel.User.IsActive;
                        trainee.User.UpdateTime = DateTime.Now;
                        if (updatedModel.User.Area != null)
                        {
                            trainee.User.Area = (int?)updatedModel.User.Area;
                        }
                        trainee.Address = updatedModel.Address;
                        trainee.Gender = (int)updatedModel.Gender;
                        trainee.Birthday = updatedModel.Birthday;
                        trainee.AcademicYear = updatedModel.AcademicYear;
                        trainee.SemesterNumber = updatedModel.SemesterNumber;
                        trainee.PhoneNumber = updatedModel.PhoneNumber;
                        trainee.MaritalStatus = (int) updatedModel.MaritalStatus;
                        trainee.EmploymentStatus = (int?) updatedModel.EmploymentStatus;
                        trainee.NeededHelpDescription = updatedModel.NeededHelpDescription;
                        trainee.PhysicsLevel = (int) updatedModel.PhysicsLevel;
                        trainee.MathLevel = (int) updatedModel.MathLevel;
                        trainee.EnglishLevel = (int) updatedModel.EnglishLevel;
                        if (trainee.DroppedOut == false && updatedModel.DroppedOut == true)
                        {
                            _predictionTrainingService.AddFromDropping(trainee.UserId);
                        }
                        trainee.DroppedOut = updatedModel.DroppedOut;
                        
                        //Linked Entities - need to verify Academic Institutions and Majors
                        trainee.AcademicInstitution = academicInstitution;
                        trainee.AcademicInstitutionId = academicInstitution.Id;
                        trainee.AcademicMajor = academicMajor;
                        trainee.AcademicMajorId = academicMajor.Id;
                        trainee.AcademicMajor1 = academicMajor1;
                        if (updatedModel.AcademicMajor1.Id != 0)
                        {
                            trainee.AcademicMinorId = academicMajor1.Id;
                        }
                        else
                        {
                            trainee.AcademicMinorId = null;
                        }
                        trainee.AcademicMajor2 = academicMajor2;
                        if (updatedModel.AcademicMajor2.Id != 0)
                        {
                            trainee.AcademicMajorNeededHelpId = academicMajor2.Id;
                        }
                        else
                        {
                            trainee.AcademicMajorNeededHelpId = null;
                        }


                        //traineeRepository.Update(trainee);
                        unitOfWork.SaveChanges();

                        status.Success = true;
                        status.Message = String.Format("פרטי החניך {0} עודכנו בהצלחה", updatedModel.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                if (status.Message == String.Empty)
                {
                    status.Message = String.Format("שגיאה במהלך עדכון פרטי החניך");   
                }
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }

        /// <summary>
        /// Get unmatched / mached trainees from DB by area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="showMatched"></param>
        /// <returns></returns>
        public StatusModel<List<TraineeMatchViewModel>> GetUnMatchedTrainees(Area area, bool showMatched)
        {
            var result = new StatusModel<List<TraineeMatchViewModel>>(false, String.Empty, new List<TraineeMatchViewModel>());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var traineeList = traineeRepository.GetAll()
                        .Where(t => t.User.IsActive && t.User.Area == (int)area && t.User.UserRole == (int)UserRole.Trainee && (showMatched || t.TutorTrainee.All(tt => tt.Status == (int)TTStatus.InActive)))
                        .OrderBy(t => t.User.LastName).ThenBy(t => t.User.FirstName).ToList()
                        .Select(t => new TraineeMatchViewModel(t, t.TutorTrainee.Count(tt => tt.Status == (int)TTStatus.Active), t.TutorTrainee.Count(tt => tt.Status == (int)TTStatus.UnApproved) > 0 ? 1 : 0)).ToList();

                    result = new StatusModel<List<TraineeMatchViewModel>>(true, String.Empty, traineeList);
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("שגיאה בשליפת חניכים ממסד הנתונים");
                LogService.Logger.Error(result.Message, ex);
            }

            return result;
        }
    }
}

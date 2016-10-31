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
        private readonly IUserService _userService;

        public TraineeService(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Get list of all trainees from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<TraineeModel> GetTrainees(Area? area)
        {
            var result = new List<TraineeModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    result = traineeRepository.GetAll().Where(t => (!area.HasValue || t.User.Area == (int)area.Value) && t.User.UserRole == 3).OrderBy(t => t.User.Area).ThenBy(t => t.User.LastName).ThenBy(t => t.User.FirstName).ToList().Select(t => new TraineeModel(t)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting Trainees from DB");
                LogService.Logger.Error(message, ex);
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
                    var academicMajor1 = academicMajorRepository.GetByKey(model.AcademicMajor1.Id);
                    var academicMajor2 = academicMajorRepository.GetByKey(model.AcademicMajor2.Id);

                    var userRepository = unitOfWork.GetRepository<IUserRepository>();

                    //Running some server side validations
                    if (model.Birthday > DateTime.Now.AddYears(-15))
                    {
                        throw new System.ArgumentException("Trainee Birthday doesn't make sense", "model");
                    }
                    if (academicInstitution.InstitutionType == (int) InstitutionType.Mechina && model.AcademicYear != 0 && model.SemesterNumber != 0)
                    {
                        throw new System.ArgumentException("Trainee is in Mechina, Academic Year and Semester should be 0", "model");
                    }
                    if (academicInstitution.InstitutionType != (int)InstitutionType.Mechina && (model.AcademicYear == 0 || model.SemesterNumber == 0))
                    {
                        throw new System.ArgumentException("Trainee is in Mechina, Academic Year and Semester should be 0", "model");
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
                        entity.AcademicMinorId = academicMajor1.Id;
                        entity.AcademicMajorNeededHelpId = academicMajor2.Id;
                        entity.UserId = user.Id;

                        //Finally Adding the entity to DB
                        traineeRepository.Add(entity);
                        unitOfWork.SaveChanges();

                        //If we got here - Yay! :)
                        status.Success = true;
                        status.Message = String.Format("חניך {0} הוזן בהצלחה", model.User.FullName);
                    }
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת חניך");
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
    }
}

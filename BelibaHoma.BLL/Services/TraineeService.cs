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
using System.Web.ModelBinding;
using BelibaHoma.BLL.Models;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class TraineeService : ITraineeService
    {
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

                    model.TutorHours = 0;
                    model.TutorHoursBonding = 0;
                    model.User.UserRole = UserRole.Trainee;

                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();
                    var entity = model.MapTo<Trainee>();
                    traineeRepository.Add(entity);

                    unitOfWork.SaveChanges();

                    status.Success = true;
                    status.Message = String.Format("חניך {0} הוזן בהצלחה", model.User.FullName);
                }
            }
            catch (Exception ex)
            {
                status.Message = String.Format("שגיאה במהלך הוספת חניך");
                LogService.Logger.Error(status.Message, ex);
            }

            return status;
        }
    }
}

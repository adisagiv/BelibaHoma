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
    public class TutorTraineeService : ITutorTraineeService
    {
        /// <summary>
        /// Get all TutorTrainee from DB
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<TutorTraineeModel> Get(Area? area)
        {
            var result = new List<TutorTraineeModel>();
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorTraineeRepository = unitOfWork.GetRepository<ITutorTraineeRepository>();

                    var tutorTrainees = tutorTraineeRepository.GetAll().Where(t => !area.HasValue || t.Trainee.User.Area == (int)area.Value).OrderBy(t => t.Trainee.User.Area).ThenBy(t => t.Trainee.User.LastName).ThenBy(t => t.Trainee.User.FirstName).ToList().Select(t => new TutorTraineeModel(t)).ToList();
                }
            }
            catch (Exception ex)
            {
                var message = String.Format("Error getting Relationship from DB");
                LogService.Logger.Error(message, ex);
            }

            return result;
        }
    }
}
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
    }
}

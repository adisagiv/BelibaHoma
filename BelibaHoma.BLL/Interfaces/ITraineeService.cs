using BelibaHoma.BLL.Enums;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface ITraineeService
    {
        /// <summary>
        /// Get list of all trainees from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TraineeModel>> GetTrainees(Area? area);

        /// <summary>
        /// Add new Trainee
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel Add(TraineeModel model);

        /// <summary>
        /// Get Trainee by the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TraineeModel> Get(int id);

        /// <summary>
        /// Update trainee in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, TraineeModel updatedModel);
    }
}

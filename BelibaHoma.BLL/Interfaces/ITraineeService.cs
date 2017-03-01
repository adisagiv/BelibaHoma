using BelibaHoma.BLL.Enums;
using Generic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using Catel.Data;

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
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        StatusModel Update(int id, TraineeModel updatedModel, UnitOfWork<BelibaHomaDBEntities> unitOfWork = null);

        /// <summary>
        /// Get unmatched / mached trainees from DB by area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="showMatched"></param>
        /// <returns></returns>
        StatusModel<List<TraineeMatchViewModel>> GetUnMatchedTrainees(Area area, bool showMatched);

        /// <summary>
        /// Get unmatched trainee models by area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TraineeModel>> GetUnMatchedAlg(Area area);

        /// <summary>
        /// Update the trainne pazam
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        StatusModel UpdateTraineePazam(int userId);

        /// <summary>
        /// Move a list of trainees to next year 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="chooseTrainee">list of tutors id's</param>
        /// <returns></returns>
        StatusModel MoveToNextYear(Area area, List<int> chooseTrainee);
    }
}

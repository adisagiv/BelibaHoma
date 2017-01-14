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
    public interface ITutorTraineeService
    {
        /// <summary>
        /// Get list of all TutorTrainee from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TutorTraineeModel>> Get(Area? area);

        /// <summary>
        /// Get TutorTrainee by the User Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TutorTraineeModel> Get(int id);

        ///// <summary>
        ///// Update TutirTrainee's Status in DB
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="updatedModel"></param>
        ///// <returns></returns>
        //StatusModel Update(int id, TutorTraineeModel updatedModel);

        /// <summary>
        /// Add new TutorTrainee relation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel AddManual(TutorTraineeModel model);

        /// <summary>
        /// Get UnApproved Matches and relevant details from DB
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TutorTraineeUnApprovedViewModel>> GetUnApprovedMatches(Area area);

        /// <summary>
        /// Remove unApproved tutorTrainee relation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel Remove(int id);

        /// <summary>
        /// Get list of recommended TutorTrainee from db
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TutorTraineeModel>> GetRecommended(Area area);

        /// <summary>
        /// Run Auto Matching Algorithm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        StatusModel RunAlgorithm(AlgorithmModel model);

        /// <summary>
        /// Add recomendation to DB
        /// </summary>
        /// <param name="tutor"></param>
        /// <param name="trainee"></param>
        /// <returns></returns>
        StatusModel TutorTraineeAdd(TutorModel tutor, TraineeModel trainee);

        /// <summary>
        /// Reset TutorTrainee relations in a given area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel ResetTutorTrainee(Area area);

        /// <summary>
        /// Reset Recommended TutorTrainee relations in a given area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel ResetRecommended(Area area);

        /// <summary>
        /// Get all TutorTrainee relations by tutor Id from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<List<TutorTraineeModel>> GetById(int id);

        /// <summary>
        /// Returns if any tutors / rainees without recommendations
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<bool> IsUnRecommended(Area area);

        /// <summary>
        /// Changes the TutorTrainee relation Status Active <=> Inactive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<TutorTraineeModel> ChangeStatus(int id);
    }

}


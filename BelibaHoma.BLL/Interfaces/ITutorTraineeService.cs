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

        /// <summary>
        /// Update TutirTrainee's Status in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedModel"></param>
        /// <returns></returns>
        StatusModel Update(int id, TutorTraineeModel updatedModel);

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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        StatusModel<List<TutorTraineeModel>> GetById(int id);
    }

}


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
    public interface IPredictionTrainingService
    {
        /// <summary>
        /// Add Training entity to PredictionTraining table when a grade is added to a Trainee
        /// </summary>
        /// <param name="traineeId"></param>
        /// <param name="semesterNumber"></param>
        /// <returns></returns>
        StatusModel AddFromGrade(int traineeId, int semesterNumber);

        /// <summary>
        /// Add training entity to DB due to a trainee dropping out
        /// </summary>
        /// <param name="traineeId"></param>
        /// <returns></returns>
        StatusModel AddFromDropping(int traineeId);

        /// <summary>
        /// Generate about to Drop Out prediction for the gien area (Or all)
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        StatusModel<List<TraineeModel>> GeneratePrediction(Area? area);
    }
}

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
        List<TraineeModel> GetTrainees(Area? area);

    }
}

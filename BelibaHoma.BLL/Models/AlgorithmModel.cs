using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BelibaHoma.BLL.Enums;

namespace BelibaHoma.BLL.Models
{
    public class AlgorithmModel
    {
        public List<TraineeModel> TraineeList;
        public List<TutorModel> TutorList;

        public AlgorithmModel(List<TutorModel> tutors, List<TraineeModel> trainees)
        {
            TraineeList = trainees;
            TutorList = tutors;
        }
    }
}

using System.Collections.Generic;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Admin.Models
{
    public class TutorTraineeNewYearViewModel
    {
        public List<TutorModel> Tutors { get; set; }
        public List<TraineeModel> Trainees { get; set; }

        public List<TutorTraineeModel> TutorTrainee { get; set; }
        public Area Area { get; set; }
    }
}
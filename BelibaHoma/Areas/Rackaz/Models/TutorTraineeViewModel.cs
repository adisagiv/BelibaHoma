using System.Collections.Generic;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Enums;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class TutorTraineeViewModel
    {
        public List<TraineeMatchViewModel> Trainees { get; set; }
        public List<TutorMatchViewModel> Tutors { get; set; }
        public bool IsRackaz { get; set; }
        public Area? Area { get; set; }
    }
}
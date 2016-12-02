using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BelibaHoma.DAL;

namespace BelibaHoma.BLL.Models
{
    public class TutorTraineeUnApprovedViewModel
    {
        public TutorTraineeModel TutorTrainee { get; set; }
        public int TraineeMatchCount { get; set; }
        public int TutorMatchCount { get; set; }

        public TutorTraineeUnApprovedViewModel(TutorTrainee entity, int traineeMatchCount, int tutorMatchCount)
            : this(new TutorTraineeModel(entity), traineeMatchCount, tutorMatchCount)
        {
        }

        public TutorTraineeUnApprovedViewModel(TutorTraineeModel tutortrainee, int traineeMatchCount, int tutorMatchCount)
        {
            TutorTrainee = tutortrainee;
            TraineeMatchCount = traineeMatchCount;
            TutorMatchCount = tutorMatchCount;
        }
    }
}
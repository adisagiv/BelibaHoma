using System.Collections.Generic;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class AlertViewModel
    {
        public List<AlertModel> LateTutor;
        public List<AlertModel> TraineeGrade;
        public List<AlertModel> TutorReport;

        public AlertViewModel(List<AlertModel> lateTutor, List<AlertModel> traineeGrade, List<AlertModel> tutorReport)
        {
            LateTutor = lateTutor;
            TraineeGrade = traineeGrade;
            TutorReport = tutorReport;
        }
    }
}
using System.ComponentModel.DataAnnotations;
using BelibaHoma.BLL.Enums;
using BelibaHoma.DAL;
using Generic.GenericModel.Models;
using Generic.Models;

namespace BelibaHoma.BLL.Models
{
    public class TutorTraineeModel : GenericModel
    {
        public int Id { get; set; }

        public int TutorId { get; set; }

        public int TraineeId { get; set; }

        [Display(Name = "סטטוס")]
        public TTStatus Status { get; set; }

        [Display(Name = "חניך")]
        public TraineeModel Trainee { get; set; }

        [Display(Name = "חונך")]
        public TutorModel Tutor { get; set; }

        public TutorTraineeModel(TutorTrainee entity)
            : base(entity)
        {

        }

        public TutorTraineeModel()
        {

        }
    }
}

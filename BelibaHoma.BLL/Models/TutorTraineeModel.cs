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

        //TODO: Consider adding required for Status +/ IsException
        [Display(Name = "סטטוס")]
        public TTStatus Status { get; set; }

        [Display(Name = "?קשר חריג")]
        public bool IsException { get; set; }

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

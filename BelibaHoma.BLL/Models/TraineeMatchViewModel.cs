using BelibaHoma.DAL;

namespace BelibaHoma.BLL.Models
{
    public class TraineeMatchViewModel
    {
        public TraineeModel Trainee { get; set; }
        public int IsMatchedCount { get; set; }
        public int IsUnAuthMatch { get; set; }

        public TraineeMatchViewModel(Trainee entity, int numMatched, int unApprovedMatch)
            : this(new TraineeModel(entity), numMatched, unApprovedMatch)
        {
        }

        public TraineeMatchViewModel(TraineeModel trainee, int numMatched, int unApprovedMatch)
        {
            Trainee = trainee;
            IsMatchedCount = numMatched;
            IsUnAuthMatch = unApprovedMatch;
        }
    }
}
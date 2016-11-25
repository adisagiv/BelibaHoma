using BelibaHoma.DAL;

namespace BelibaHoma.BLL.Models
{
    public class TraineeMatchViewModel
    {
        public TraineeModel Trainee { get; set; }
        public int IsMatchedCount { get; set; }

        public TraineeMatchViewModel(Trainee entity, int numMatched)
            : this(new TraineeModel(entity), numMatched)
        {
        }

        public TraineeMatchViewModel(TraineeModel trainee, int numMatched)
        {
            Trainee = trainee;
            IsMatchedCount = numMatched;
        }
    }
}
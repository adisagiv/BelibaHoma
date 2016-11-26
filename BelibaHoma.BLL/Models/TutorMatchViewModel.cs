using BelibaHoma.DAL;

namespace BelibaHoma.BLL.Models
{
    public class TutorMatchViewModel
    {
        public TutorModel Tutor { get; set; }
        public int IsMatchedCount { get; set; }
        public int IsUnAuthMatch { get; set; }

        public TutorMatchViewModel(Tutor entity, int isMatched, int unApprovedMatch)
            : this(new TutorModel(entity), isMatched, unApprovedMatch)
        {
        }

        public TutorMatchViewModel(TutorModel tutor, int isMatched, int unApprovedMatch)
        {
            Tutor = tutor;
            IsMatchedCount = isMatched;
            IsUnAuthMatch = unApprovedMatch;
        }
    }
}
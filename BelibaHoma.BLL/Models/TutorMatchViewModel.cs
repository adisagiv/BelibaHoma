using BelibaHoma.DAL;

namespace BelibaHoma.BLL.Models
{
    public class TutorMatchViewModel
    {
        public TutorModel Tutor { get; set; }
        public int IsMatchedCount { get; set; } 
        
        public TutorMatchViewModel(Tutor entity, int isMatched)
            :this(new TutorModel(entity),isMatched)
        {
        }

        public TutorMatchViewModel(TutorModel tutor, int isMatched)
        {
            Tutor = tutor;
            IsMatchedCount = isMatched;
        }
    }
}
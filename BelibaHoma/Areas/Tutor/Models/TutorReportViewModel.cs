using System.Collections.Generic;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Tutor.Models
{
    public class TutorReportViewModel
    {
        public TutorReportModel TutorReport { get; set; }
        public List<TutorSessionModel> TutorSessios { get; set; }
    }
}
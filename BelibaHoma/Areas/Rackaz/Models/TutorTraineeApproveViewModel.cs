using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Models;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class TutorTraineeApproveViewModel
    {
        public List<TutorTraineeUnApprovedViewModel> MatchesList { get; set; }
        public bool IsRackaz { get; set; }
        public Area? Area { get; set; }
    }
}
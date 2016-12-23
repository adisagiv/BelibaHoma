using System.Collections.Generic;
using BelibaHoma.BLL.Models.Reports;

namespace BelibaHoma.BLL.Models
{
    public class JoinDropStatisticsModel
    {
        public List<string> YearList { get; set; }
        public List<Series1> Series { get; set; }
    }
}
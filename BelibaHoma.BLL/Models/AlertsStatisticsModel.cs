using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelibaHoma.BLL.Models
{
    public class AlertsStatisticsModel
    {
        public Dictionary<string, int> AlertsStatistics { get; set; }
        public List<Series1> Series { get; set; }

    }
}

using BelibaHoma.Models;

namespace BelibaHoma.Areas.Rackaz.Models
{
    public class HomeViewModel
    {
        public int newAlertsCount;
        public int OnGoingAlertsCount;
        public double TutorHoursCount;
        public ReportType ReportType { get; set; }
    }
}
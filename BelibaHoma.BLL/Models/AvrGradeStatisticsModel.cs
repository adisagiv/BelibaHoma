using System.Collections.Generic;
using BelibaHoma.BLL.Enums;
namespace BelibaHoma.BLL.Models
{
    public class AvrGradeStatisticsModel
    {
        public Dictionary<int, double> AvrGradeStatistics { get; set; } //todo: change to year and semestertype
        //public Pair<int, SemesterType> pair { get; set; }
        
        //public class Pair<T1,T2>
        //{
        //    public int year;
        //    public SemesterType semesterType;
        //}
    }
 
}
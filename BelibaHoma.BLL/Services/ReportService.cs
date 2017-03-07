using System;
using System.Collections.Generic;
using System.Linq;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Collections;
using Catel.Data;
using Extensions.Enums;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class ReportService : IReportService
    {
        public StatusModel<HourStatisticsModel> GetHourStatistics(Area? area, DateTime startTime, DateTime endTime, HourStatisticsType hourStatisticsType)
        {
            var result = new StatusModel<HourStatisticsModel>(false,String.Empty, new HourStatisticsModel());

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepoistory = unitOfWork.GetRepository<ITutorRepository>();

                    var tutors = tutorRepoistory.GetAll().Where(t => !area.HasValue || t.User.Area == (int) area.Value);

                    var tutorSessions = tutors.SelectMany(t => t.TutorTrainee)
                        .SelectMany(tt => tt.TutorReport)
                        .SelectMany(tr => tr.TutorSession)
                        .Where(ts => ts.MeetingDate >= startTime && ts.MeetingDate <= endTime);

                    var traineesCount = tutorSessions.GroupBy(ts => ts.TutorReport.TutorTrainee.Trainee).Count();

                    if (hourStatisticsType == HourStatisticsType.Sum)
                    {
                        result.Data.HourStatistics = tutorSessions.GroupBy(ts => ts.MeetingDate.Month).ToDictionary(ts => ts.Key,
                            tss => tss.Sum(ts => (ts.EndTime - ts.StartTime).TotalHours));
                    }
                    else
                    {
                        result.Data.HourStatistics = tutorSessions.GroupBy(ts => ts.MeetingDate.Month).ToDictionary(ts => ts.Key,
                            tss => tss.Sum(ts => (ts.EndTime - ts.StartTime).TotalHours)/traineesCount);
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Hour Statistics from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

        
        /// //////////////// 
        

        public StatusModel<JoinDropStatisticsModel> GetJoinDropStatistics(Area? area)
        {
            var result = new StatusModel<JoinDropStatisticsModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();

                    var JoinedTrainee = traineeRepository.GetAll().Where(
                        t => t.User.UserRole == (int)UserRole.Trainee &&
                                (!area.HasValue || t.User.Area == (int?)area));

                    var DropedTrainee = traineeRepository.GetAll().Where(
                        t => t.User.Trainee.DroppedOut && t.User.UserRole == (int)UserRole.Trainee &&
                                (!area.HasValue || t.User.Area == (int?)area));

                    var sumJoined = JoinedTrainee.OrderBy(ts=>ts.User.CreationTime.Year).GroupBy(ts => ts.User.CreationTime.Year)
                        .ToDictionary(ts => ts.Key, tss => tss.Sum(ts => 1));
                    var sumDropedTrainee = DropedTrainee.OrderBy(ts => ts.User.CreationTime.Year).GroupBy(ts => ts.User.UpdateTime.Year)
                        .ToDictionary(ts => ts.Key, tss => tss.Sum(ts => 1));

                    result.Data = new JoinDropStatisticsModel();
                    var minYearJoined = sumJoined.Count > 0 ? sumJoined.Min(s => s.Key) : DateTime.MaxValue.Year;
                    var minYearDropped = sumDropedTrainee.Count > 0
                        ? sumDropedTrainee.Min(s => s.Key)
                        : DateTime.MaxValue.Year;

                    var minYear = minYearDropped > minYearJoined ? minYearJoined : minYearDropped;

                    var maxYearJoined = sumJoined.Count > 0 ? sumJoined.Max(s => s.Key) : DateTime.MinValue.Year;
                    var maxYearDropped = sumDropedTrainee.Count > 0
                        ? sumDropedTrainee.Max(s => s.Key)
                        : DateTime.MinValue.Year;

                    var maxYear = maxYearDropped < maxYearJoined ? maxYearJoined : maxYearDropped;

                    var yearList = new List<string>();
                    var yearListInt = new List<int>();
                    for (var year = minYear; year <= maxYear; year++)
                    {
                        yearList.Add(year.ToString());
                        yearListInt.Add(year);
                    }

                    result.Data.YearList = yearList;


                    var joined = new Series1
                    {
                        name = "כמות המצטרפים"
                    };

                    var dropped = new Series1
                    {
                        name = "כמות הנושרים"
                    };

                    var joinedSeries = new List<int>();
                    var droppedSeries = new List<int>();

                    foreach (var year in yearListInt)
                    {
                        if (sumJoined.ContainsKey(year))
                        {
                            joinedSeries.Add(sumJoined[year]);
                        }
                        else
                        {
                            joinedSeries.Add(0);
                        }

                        if (sumDropedTrainee.ContainsKey(year))
                        {
                            droppedSeries.Add(sumDropedTrainee[year]);
                        }
                        else
                        {
                            droppedSeries.Add(0);
                        }
                    }

                    joined.data = joinedSeries.ToArray();
                    dropped.data = droppedSeries.ToArray();

                    result.Data.Series = new List<Series1> {joined, dropped};

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Join Drop Statistics from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        } 

        //////////// Alerts

        /// //////////////// 


        public StatusModel<AlertsStatisticsModel> GetAlertsStatistics(Area? area, DateTime startTime, DateTime endTime)
        {
            var result = new StatusModel<AlertsStatisticsModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertsRepository = unitOfWork.GetRepository<IAlertRepository>();

                    var JoinedAlerts = alertsRepository.GetAll().Where(
                        t => (!area.HasValue || t.Area == (int?)area));

                    var JoinedAlertsCurYear = JoinedAlerts.Where(ts => ts.CreationTime >= startTime && ts.CreationTime <= endTime);



                    var groupedAlertsType = JoinedAlertsCurYear.GroupBy(ts => ts.AlertType);

                    var dict0 = new Dictionary<string, int>(); // dictionary of AlertType, Alerts count.
                    var dict1 = new Dictionary<string, int>(); // dictionary of AlertType, Alerts count.
                    var dict2 = new Dictionary<string, int>(); // dictionary of AlertType, Alerts count.


                    var alertType0 = new Series1
                    {
                        name = "ציון חניך"
                    };

                    var alertType1 = new Series1
                    {
                        name = "דרושה התערבות"
                    };

                    var alertType2 = new Series1
                    {
                        name = "איחור בדיווח"
                    };



                    var Type0Series = new List<int>();
                    var Type1Series = new List<int>();
                    var Type2Series = new List<int>();

                 
                    
                    foreach (var tt in groupedAlertsType)
                    {
                        var groupedAlerts = tt.GroupBy(tr => tr.CreationTime.Month);

                        var key = tt.Key;
                        foreach (var alert in tt)
                        {
                            var month = alert.CreationTime.Month.ToString();
                            switch (key)
                            {
                                case 0 :
                                    
                                    if (!dict0.ContainsKey(month))
                                    {
                                        dict0.Add(month, 1);
                                    }
                                    else
                                    {
                                        dict0[month]++;
                                    }
                                    break;
                                case 1 :
                                    if (!dict1.ContainsKey(month))
                                    {
                                        dict1.Add(month, 1);
                                    }
                                    else
                                    {
                                        dict1[month]++;
                                    }
                                    break;
                                 case 2 :
                                    if (!dict2.ContainsKey(month))
                                    {
                                        dict2.Add(month, 1);
                                    }
                                    else
                                    {
                                        dict2[month]++;
                                    }
                                    break;
                            }   
                        }

                        //var key = tt.Key;
                        //var value = tt.Where();
                        

                        //if (tt.Key==0)
                        //{
                        //    //dict0.Add(groupedAlerts.First().Key.ToString(),groupedAlerts.Count());
                        //    if (dict0.ContainsKey(tt))
                            
                        //}
                        //if (tt.Key==1)
                        //{
                        //    dict1.Add(groupedAlerts.First().Key.ToString(),groupedAlerts.Count());
                        //}
                        //if (tt.Key==2)
                        //{
                        //    dict2.Add(groupedAlerts.First().Key.ToString(),groupedAlerts.Count());
                        //}

                    }

                   

                  //  result.Data.AlertsStatistics = dict;
                    var monthListInt = new List<int>();
                    for (var month = 9; month <= 20; month++)
                    {
                        monthListInt.Add(month%12 + 1);
                       
                    }

                    foreach (var month in monthListInt)
                    {
                        if (dict0.ContainsKey(month.ToString()))
                        {
                            Type0Series.Add(dict0[month.ToString()]);
                        }
                        else
                        {
                            Type0Series.Add(0);
                        }

                        if (dict1.ContainsKey(month.ToString()))
                        {
                            Type1Series.Add(dict1[month.ToString()]);
                        }
                        else
                        {
                            Type1Series.Add(0);
                        }

                        if (dict2.ContainsKey(month.ToString()))
                        {
                            Type2Series.Add(dict2[month.ToString()]);
                        }
                        else
                        {
                            Type2Series.Add(0);
                        }

                    }

                    alertType0.data = Type0Series.ToArray();
                    alertType1.data = Type1Series.ToArray();
                    alertType2.data = Type2Series.ToArray();

                    result.Data = new AlertsStatisticsModel();
                    result.Data.Series = new List<Series1> { alertType0, alertType1, alertType2 };


                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Alerts Statistics from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }



        //////////

        public StatusModel<InvestedHoursStatisticsModel> GetInvestedHoursStatistics(Area? area)
        {
            var result = new StatusModel<InvestedHoursStatisticsModel>();
            
            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();

                    var trainees = traineeRepository.GetAll().Where(
                        t => t.User.IsActive && t.User.UserRole == (int)UserRole.Trainee &&
                                (!area.HasValue || t.User.Area == (int?)area));

                    // grouping by year in program:
                    var groupedTrainees =
                        trainees.GroupBy(ts => (ts.User.CreationTime.Year));

                    var thisYear = DateTime.Now.Year;
                    var dic = new Dictionary<int, double>(); // dictionary of vetek, avg of that vetek.
                    foreach (var tr in groupedTrainees)
                    {
                        var totalTutorHoursGiven = 0;
                        var i = 0.0;
                        var vetek = thisYear - tr.Key;
                        var avrTotalTutorHoursGiven = tr.Average(t => t.TutorHours);
                        //var vetek = thisYear - tr.First().User.CreationTime.Year;
                        //foreach (var person in tr)
                        //{
                        //    i = i++;
                        //    totalTutorHoursGiven = totalTutorHoursGiven + person.TutorHours;
                        //}
                        //var avrTotalTutorHoursGiven = totalTutorHoursGiven/i;
                        dic.Add(vetek, avrTotalTutorHoursGiven);
                    }


                    result.Data = new InvestedHoursStatisticsModel
                    {
                        InvestedHoursStatistics = dic
                    };

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                
                result.Message = String.Format("Error getting Hours Statistics from DB"); 
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
            


        }

        /// ///////AvrGrade...
        /// 


        public StatusModel<AvrGradeStatisticsModel> GetAvrGradeStatistics(Area? area)
        {
            var result = new StatusModel<AvrGradeStatisticsModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var gradeRepository = unitOfWork.GetRepository<IGradeRepository>();
                        
                    var grades = gradeRepository.GetAll().Where(
                        t =>(!area.HasValue || t.Trainee.User.Area == (int?)area)).ToList();

                    var groupedGrades = grades.OrderBy(tr=>tr.Year).ThenBy(tr=>tr.SemesterType).GroupBy(tr => tr.Year);
                    
                    var dict = new Dictionary<string, double>(); // dictionary of vetek, avg of that vetek.
                    foreach (var tt in groupedGrades)
                    {
                        var groupedGrades2 = tt.GroupBy(ts => ts.SemesterType);
                        foreach (var tr in groupedGrades2)
                        {
                            var avrSumOfGradesInYearSemester = tr.Average(t => t.Grade1);
                            //todo: Atalia and Manor
                            var key = String.Format("{0}_{1}", tt.Key, ((SemesterType)tr.Key).ToDescription());
                            dict.Add(key, avrSumOfGradesInYearSemester);

                        }
                    }

                    var thisYear = DateTime.Now.Year;
                    

                    result.Data = new AvrGradeStatisticsModel
                    {
                        AvrGradeStatistics = dict
                    };

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting avarage grades from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;



        }






        public StatusModel<List<int>> GetHourStatisticsProssibleYears()
        {
            var result = new StatusModel<List<int>>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();

                    
                     var minDate = tutorSessionRepository.GetAll().Select(ts=>ts.MeetingDate).Min(ts => ts);

                     var minPossibleYears = minDate.Month >= 10 ? minDate.Year : minDate.Year - 1;

                     var maxDate = tutorSessionRepository.GetAll().Select(ts => ts.MeetingDate).Max(ts => ts);
                    
                    var maxPossibleYears = maxDate.Month >= 10 ? maxDate.Year : maxDate.Year - 1;


                    var possibleYears = new List<int>();

                    for (int i = maxPossibleYears; i >= minPossibleYears; i--)
                    {
                        possibleYears.Add(i);
                    }

                    

                    result.Data = possibleYears;

                    

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting years from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

        public StatusModel<List<int>> GeAlertStatisticsProssibleYears()
        {
            var result = new StatusModel<List<int>>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var alertRepository = unitOfWork.GetRepository<IAlertRepository>();

                    var minDate = alertRepository.GetAll().Select(a => a.CreationTime).Min(ts => ts);

                    var minPossibleYears = minDate.Month >= 10 ? minDate.Year : minDate.Year - 1;

                    var maxDate = alertRepository.GetAll().Select(a => a.CreationTime).Max(ts => ts);

                    var maxPossibleYears = maxDate.Month >= 10 ? maxDate.Year : maxDate.Year - 1;
                        

                    var possibleYears = new List<int>();

                    for (int i = minPossibleYears; i <= maxPossibleYears; i++)
                    {
                        possibleYears.Add(i);
                    }


                    result.Data = possibleYears;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting years from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

        public StatusModel<List<Series1>> GetHourHistogram(Area? area)
        {
            var result = new StatusModel<List<Series1>>();
            var startTime = new DateTime(DateTime.Now.Year, 10, 1);
            if (DateTime.Now.Month < 10)
            {
                startTime = new DateTime(DateTime.Now.Year - 1, 10, 1);
            }

            var endTime = new DateTime(startTime.Year + 1, 10, 1);

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var series = new List<Series1>
                    {
                        new Series1
                        {
                            name = "מתחת ל 6 שעות",
                            color = "violet"
                        },
                        new Series1
                        {
                            name = "בין 6 - 12",
                            
                        },
                        new Series1
                        {
                            name = "מעל ל 12 שעות",
                            color = "lightgreen"
                        }
                    };

                    var tutorSessionRepository = unitOfWork.GetRepository<ITutorSessionRepository>();

                    var tutorSessionMonth =
                        tutorSessionRepository.GetAll()
                            .Where(ts => ts.MeetingDate >= startTime && ts.MeetingDate < endTime)
                            .GroupBy(ts => ts.MeetingDate.Month);

                    var below6 = new List<int>  { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    var below12 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; 
                    var above12 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    tutorSessionMonth.ForEach(m =>
                    {
                        var index = (m.Key + 2) % 12;

                        var byTutors = m.GroupBy(ts => ts.TutorReport.TutorTrainee.TutorId);

                        var tutorHourSum = byTutors.ToDictionary(t => t, t => t.Sum(ts => (ts.EndTime - ts.StartTime).TotalHours));



                        below6[index] = tutorHourSum.Count(t => t.Value < 6);
                        below12[index] = tutorHourSum.Count(t => t.Value >= 6 && t.Value < 12);
                        above12[index] = tutorHourSum.Count(t => t.Value > 12);
                    });

                    series[0].data = below6.ToArray();
                    series[1].data = below12.ToArray();
                    series[2].data = above12.ToArray();
                    result.Data = series;


                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting Alerts Statistics from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

        public StatusModel<int> GetMaxPazam()
        {
            var result = new StatusModel<int>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var traineeRepository = unitOfWork.GetRepository<ITraineeRepository>();

                    var trainees = traineeRepository.GetAll().Where(
                        t => t.User.IsActive && t.User.UserRole == (int)UserRole.Trainee);
                    var now = DateTime.Now.Year;
                    var maxYear = 0;
                    foreach (var tr in trainees)
                    {
                        if (maxYear < (now - tr.User.CreationTime.Year))
                        {
                            maxYear = now - tr.User.CreationTime.Year;
                        }
                    }

                    result.Data = maxYear;

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting seniorest trainee from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;



        }
    }
}
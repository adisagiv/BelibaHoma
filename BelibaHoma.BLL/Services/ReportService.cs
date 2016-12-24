using System;
using System.Collections.Generic;
using System.Linq;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.DAL;
using BelibaHoma.DAL.Interfaces;
using Catel.Data;
using Generic.Models;
using Services.Log;

namespace BelibaHoma.BLL.Services
{
    public class ReportService : IReportService
    {
        public StatusModel<HourStatisticsModel> GetHourStatistics(Area? area, DateTime startTime, DateTime endTime, HourStatisticsType hourStatisticsType)
        {
            var result = new StatusModel<HourStatisticsModel>();

            try
            {
                using (var unitOfWork = new UnitOfWork<BelibaHomaDBEntities>())
                {
                    var tutorRepository = unitOfWork.GetRepository<ITutorRepository>();

                    var tutors = tutorRepository.GetAll().Where(
                        t => t.User.IsActive && t.User.UserRole == (int) UserRole.Tutor && 
                                (!area.HasValue || t.User.Area == (int?) area));

//                    var tutors = users.Select(u => u.Tutor);
                    
                    var tutorSessions = tutors.SelectMany(t => t.TutorTrainee)
                        .SelectMany(tt => tt.TutorReport)
                        .SelectMany(tr => tr.TutorSession)
                        .Where(ts => ts.MeetingDate >= startTime && ts.MeetingDate <= endTime);


                   result.Data = new HourStatisticsModel();

                    var groupedMonth = tutorSessions.GroupBy(ts => ts.MeetingDate.Month);

                    if (hourStatisticsType == HourStatisticsType.Sum)
                    {
                        result.Data.HourStatistics = groupedMonth.ToDictionary(ts => ts.Key,
                            tss => tss.Sum(ts => (ts.EndTime - ts.StartTime).TotalHours));
                    }
                    else
                    {
                        result.Data.HourStatistics = groupedMonth.ToDictionary(ts => ts.Key,
                            tss => tss.Average(ts => (ts.EndTime - ts.StartTime).TotalHours));
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting job offers from DB");
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
                result.Message = String.Format("Error getting job offers from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;
        }

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
                        var vetek = thisYear - tr.First().User.CreationTime.Year;
                        foreach (var person in tr)
                        {
                            i = i++;
                            totalTutorHoursGiven = totalTutorHoursGiven + person.TutorHours;
                        }
                        var avrTotalTutorHoursGiven = totalTutorHoursGiven/i;
                        dic.Add(vetek, avrTotalTutorHoursGiven);
                    }


                    //result.Data = new InvestedHoursStatisticsModel();
                    result.Data.InvestedHoursStatistics = dic;

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting job offers from DB");
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

                    var possibleYears =
                        tutorSessionRepository.GetAll()
                            .GroupBy(ts => ts.MeetingDate.Year)
                            .Select(ts => ts.Key)
                            .OrderBy(ts => ts)
                            .ToList();

                    result.Data = possibleYears;

                    

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Message = String.Format("Error getting job offers from DB");
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
                result.Message = String.Format("Error getting job offers from DB");
                LogService.Logger.Error(result.Message, ex);
            }


            return result;



        }
    }
}
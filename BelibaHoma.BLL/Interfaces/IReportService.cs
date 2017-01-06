using System;
using System.Collections.Generic;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Models;
using Generic.Models;

namespace BelibaHoma.BLL.Interfaces
{
    public interface IReportService
    {
        StatusModel<HourStatisticsModel> GetHourStatistics(Area? area, DateTime startTime, DateTime endTime, HourStatisticsType hourStatisticsType);
        StatusModel<JoinDropStatisticsModel> GetJoinDropStatistics(Area? area);
        StatusModel<InvestedHoursStatisticsModel> GetInvestedHoursStatistics(Area? area);
        StatusModel<AvrGradeStatisticsModel> GetAvrGradeStatistics(Area? area);
        StatusModel<AlertsStatisticsModel> GetAlertsStatistics(Area? area,DateTime startTime, DateTime endTime);
        StatusModel<List<int>> GetHourStatisticsProssibleYears();
        StatusModel<int> GetMaxPazam();
        StatusModel<List<int>> GeAlertStatisticsProssibleYears();

    }
}
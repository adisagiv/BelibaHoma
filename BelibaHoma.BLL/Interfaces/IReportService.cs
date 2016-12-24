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
        StatusModel<List<int>> GetHourStatisticsProssibleYears();
    }
}
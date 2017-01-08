using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Models.Reports;
using BelibaHoma.Controllers;
using Extensions.Enums;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Extensions;

namespace BelibaHoma.Areas.Rackaz.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // return the view of the report
        [HttpPost]
        public ActionResult HourStatistics()
        {
            var result = _reportService.GetHourStatisticsProssibleYears();

            if (result.Success)
            {
                return View(result.Data);
            }
            return Error(result);
        }

        private List<double> RemoveLastsZeros(List<double> series)
        {
            var newlist = new List<double>(series);
            var j = newlist.Count;
            while (j-- > 0 && newlist[j] == 0.0)
            {
                newlist.RemoveAt(j);
            }
            return newlist;
        }

        private List<int> RemoveLastsZeros(List<int> series)
        {
            var newlist = new List<int>(series);
            var j = newlist.Count;
            while (j-- > 0 && newlist[j] == 0)
            {
                newlist.RemoveAt(j);
            }
            return newlist;
        }

        //return the data for the report
        [HttpPost]
        public ActionResult GetHourStatistics(HourStatisticsType hourStatisticsType ,int? year, Area?area)
        {
            
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                area = CurrentUser.Area;
            }
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var startTime = new DateTime(year.Value, 10, 1);
            var endTime = new DateTime(year.Value + 1, 10, 1);

            var result = _reportService.GetHourStatistics(area, startTime, endTime, hourStatisticsType);

            //TODO: dont show if no data exists:
            //if (result.Data == null)
            //{
            //    return Error(result);
            //}

            if (result.Success)
            {
                var series = new List<double>();
                
                for (var i = 1; i <= 12; i++)
                {
                    var hours = 0.0;
                    var month = (i + 8)%12 + 1;

                    if (result.Data.HourStatistics.ContainsKey(month))
                    {
                        hours = result.Data.HourStatistics[month];
                    }

                    series.Add(Math.Round(hours, 2));
                }

                series = RemoveLastsZeros(series);

                var chartModel = new HighChartModel
                {
                    chart = new Chart
                    {
                        type ="line"
                    },
                    title = new Title
                    {
                        
                        text = (area.HasValue ? string.Format("דוח סטטיסטיקה שעות חניכה באיזור {0}",area.Value.ToDescription() ) : "דוח סטטיסטיקה שעות חניכה"),
                        x = -20
                    },
                    xAxis = new Xaxis
                    {
                        categories = new List<string>
                        {
                            "אוקטובר",
                            "נובמבר",
                            "דצמבר",
                            "ינואר",
                            "פברואר",
                            "מרץ",
                            "אפריל",
                            "מאי",
                            "יוני",
                            "יולי",
                            "אוגוסט",
                            "ספטמבר"
                        },
                        title = new Title1
                        {
                            text = string.Format("אוקטובר {0} - ספטמבר {1}", year, year+1)
                            //text = "אוקטובר שנה נבחרת עד ספטמבר שנה לאחר מכן"
                        }
                    },
                    yAxis = new Yaxis
                    {
                        title = new Title1
                        {
                            text = "סכום שעות החניכה"
                        },
                        plotLines = new List<Plotline> {
                            new Plotline
                            {
                                color = "red",
                                dashStyle = "Solid",
                                // TODO : Change to real values from or
                                value = hourStatisticsType == HourStatisticsType.Sum ? 5 : 1,
                                width = 3
                            }
                        }
                    },
                    series =new List<Series>
                    {
                        new Series
                        {
                            data = series,
                            name= (hourStatisticsType==0? string.Format("סכום שעות חניכה"): "ממוצע שעות חניכה" ),
                            
                        }
                        
                    },
                    legend = new Legend
                    {
                        layout= "vertical",
                        align= "right",
                        verticalAlign= "middle",
                        borderWidth = 0
                    },
                    tooltip = new Tooltip1()
                    {
                        headerFormat = "<span style='font-size:10px'>{point.key}</span><table>",
                        pointFormat = "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y}</b></td></tr>",
                        footerFormat = "</table>",
                        shared = true,
                        useHTML = true

                    },
                    exporting = new Exporting
                    {
                        enabled = false
                    }

                };

                return Json(chartModel);
            }
            return Error(result);

            //return null;
        }


        /// //////////////////////////  for JoinedDroped
        /// 
        /// 
        /// 


        // return the view of the report
        [HttpPost]
        public ActionResult JoinDropStatistics()
        {
            return View();
        }

        //return the data for the report
        [HttpPost]
        public ActionResult GetJoinDropStatistics(Area? area)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                area = CurrentUser.Area;
            }
                var result = _reportService.GetJoinDropStatistics(area);
            if (result.Success)
            {
                foreach (var serie in result.Data.Series)
                {
                    serie.data = RemoveLastsZeros(serie.data.ToList()).ToArray();
                }
                

                var chartModel1 = new HighChartJDModel
                {
                    chart = new Chart1
                    {
                        type = "column"
                    },
                    title = new Title2
                    {
                        text = (area.HasValue ? string.Format("דוח סטטיסטיקה הצטרפות ונשירת חניכים באיזור {0}", area.Value.ToDescription()) : "דוח סטטיסטיקה הצטרפות ונשירת חניכים"),
                        //text = "דוח סטטיסטיקה הצטרפות ונשירת חניכים",
                    },
                    xAxis = new Xaxis1
                    {
                        // we will take the years from the dictionary
                        
                        categories = result.Data.YearList.ToArray(),
                      

                    crosshair= true
                    
                        
                    },
                    yAxis = new Yaxis1
                    {

                        min= new int {},  
                        title = new Title2()
                        {
                            text = "כמות מצטרפים ונושרים",
                            //useHTML = false
                        },
                    
                    },
                    series = result.Data.Series.ToArray(),
            tooltip = new Tooltip1()
            {
                headerFormat= "<span style='font-size:10px'>{point.key}</span><table>",
                pointFormat= "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y}</b></td></tr>",
                footerFormat = "</table>",
                shared= true,
                useHTML= true

            },
             plotOptions= new Plotoptions1()
                {
                 column= new Column2()
                 {
                    pointPadding= new float(),
                    borderWidth= new int()
                 }

                },
                    exporting = new Exporting1
                    {
                        enabled = false
                    }  


                };

                return Json(chartModel1);
            }
            return Error(result);
            //return null;
        }




        /// 
        /// 
        /// ///////  /// Alerts
        /// 
        /// 


        // return the view of the report
        [HttpPost]
        public ActionResult AlertsStatistics()
        {
            var result = _reportService.GeAlertStatisticsProssibleYears();

            if (result.Success)
            {
                return View(result.Data);
            }
            return Error(result);
        }

        //return the data for the report
        [HttpPost]
        public ActionResult GetAlertsStatistics(Area? area, int? year)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                area = CurrentUser.Area;
            }
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var startTime = new DateTime(year.Value, 10, 1);
            var endTime = new DateTime(year.Value + 1, 10, 1);

            var result = _reportService.GetAlertsStatistics(area, startTime, endTime);

            if (result.Success)
            {
                foreach (var serie in result.Data.Series)
                {
                    serie.data = RemoveLastsZeros(serie.data.ToList()).ToArray();
                }
                
                var chartModel1 = new HighChartJDModel
                {
                    chart = new Chart1
                    {
                        type = "column"
                    },
                    title = new Title2
                    {
                        text = (area.HasValue ? string.Format("דוח סטטיסטיקת התרעות באיזור {0}", area.Value.ToDescription()) : "דוח סטטיסטיקת התרעות"),

                        //text = "דוח סטטיסטיקת התרעות",
                        //useHTML = false
                    },
                    xAxis = new Xaxis1
                    {
                        // we will take the years from the dictionary
                        
                    categories = new string[]
                        {
                            "אוקטובר",
                            "נובמבר",
                            "דצמבר",
                            "ינואר",
                            "פברואר",
                            "מרץ",
                            "אפריל",
                            "מאי",
                            "יוני",
                            "יולי",
                            "אוגוסט",
                            "ספטמבר"
                        },                      

                    crosshair= true
                    
                        
                    },
                    yAxis = new Yaxis1
                    {

                        min= new int {},  
                        title = new Title2()
                        {
                            text = "כמות התרעות",
                            //useHTML = false
                        },
                    
                    },
                    series = result.Data.Series.ToArray(),

            tooltip = new Tooltip1()
            {
                headerFormat= "<span style='font-size:10px'>{point.key}</span><table>",
                pointFormat= "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y}</b></td></tr>",
                footerFormat = "</table>",
                shared= true,
                useHTML= true

            },
             plotOptions= new Plotoptions1()
                {
                 column= new Column2()
                 {
                    pointPadding= new float(),
                    borderWidth= new int()
                 }

                },
                    exporting = new Exporting1
                    {
                        enabled = false
                    }  


                };

                return Json(chartModel1);
            }
            return Error(result);
            //return null;
        }







        /// //////////////////////////InvestedHoursStatistics



        // return the view of the report
        [HttpPost]
        public ActionResult InvestedHoursStatistics()
        {
            return View();
        }

        //return the data for the report
        [HttpPost]
        public ActionResult GetInvestedHoursStatistics(Area? area)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                area = CurrentUser.Area;
            }
            var result = _reportService.GetInvestedHoursStatistics(area);

            if (result.Success)
            {
                var series = new List<double>();

                //function that findes the most pazam trainee!!!
                var maxYearT = _reportService.GetMaxPazam();
                var max = maxYearT.Data;

                for (var i = max; i >= 0; i--)
                {
                    var investedHours = 0.0;

                    if (result.Data.InvestedHoursStatistics.ContainsKey(i))
                    {
                        investedHours = result.Data.InvestedHoursStatistics[i];
                    }

                    series.Add(Math.Round(investedHours, 2));

                }
                series = RemoveLastsZeros(series);
                //int[] pazamList = new int[max];
                var pazamList = new List<string>();
                
                for (var i = 0; i <= max; i++)
                {
                    //pazamList[i] = i.ToString();
                    var relevantYear = DateTime.Now.Year - max + i;
                    pazamList.Add(relevantYear.ToString());
                }


                var chartModel = new HighChartModel
                {
                    chart = new Chart
                    {
                        type = "line"
                    },
                    title = new Title
                    {
                        text = (area.HasValue ? string.Format("דוח סטטיסטיקה שעות חניכה כתלות בותק באיזור {0}", area.Value.ToDescription()) : "דוח סטטיסטיקה שעות חניכה כתלות בותק"),
                        //text = "דוח סטטיסטיקה שעות חניכה כתלות בותק",
                        x = -20
                    },
                    xAxis = new Xaxis
                    {
                        categories = pazamList,  // new List<string>
                        title = new Title1
                        {
                            text = "שנת הצטרפות החניך"
                        }
                        
                    },
                    yAxis = new Yaxis
                    {
                        title = new Title1
                        {
                            text = "סכום שעות החניכה"
                        },
                        plotLines = new List<Plotline> {
                            new Plotline
                            {
                                color = "#808080",
                                value = 0,
                                width = 1
                            }
                        }
                    },
                    series = new List<Series>
                    {
                        new Series
                        {
                            data = series,
                            name = "סכום שעות חניכה"
                        }
                        
                    },
                    legend = new Legend
                    {
                        layout = "vertical",
                        align = "right",
                        verticalAlign = "middle",
                        borderWidth = 0
                    },
                    tooltip = new Tooltip1()
                    {
                        headerFormat = "<span style='font-size:10px'>{point.key}</span><table>",
                        pointFormat = "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y}</b></td></tr>",
                        footerFormat = "</table>",
                        shared = true,
                        useHTML = true

                    },
                    exporting = new Exporting
                    {
                        enabled = false
                    }

                };

                return Json(chartModel);
            }
            return Error(result);
            //return null;
        }

        /// //////////////////////////  for AvrGrade

        // return the view of the report
        [HttpPost]
        public ActionResult AvrGradeStatistics()
        {
            return View();
        }

        //return the data for the report
        [HttpPost]
        public ActionResult GetAvrGradeStatistics(Area? area)
        {
            if (CurrentUser.UserRole == UserRole.Rackaz)
            {
                area = CurrentUser.Area;
            }
            var result = _reportService.GetAvrGradeStatistics(area);


            if (result.Success)
            {
                var series = new List<double>();

                foreach (var ts in result.Data.AvrGradeStatistics)
                {
                    var avrGrade = 0.0;
                    avrGrade = ts.Value;
                    series.Add(Math.Round(avrGrade,2));

                }
                series = RemoveLastsZeros(series);
                var yearAndSemesterList = new List<string>();

                foreach (var ts in result.Data.AvrGradeStatistics)
                {
                    var i = ts.Key;
                    yearAndSemesterList.Add(i);
                }

                

                var chartModel = new HighChartModel
                {
                    chart = new Chart
                    {
                        type = "line"
                    },
                    title = new Title
                    {
                        text = (area.HasValue ? string.Format("דוח ממוצע חניכים במהלך הסמסטרים באיזור {0}", area.Value.ToDescription()) : "דוח ממוצע חניכים במהלך הסמסטרים"),
                        //text = "דוח ממוצע חניכים במהלך הסמסטרים",
                        x = -20
                    },
                    xAxis = new Xaxis
                    {
                        categories = yearAndSemesterList,
                        title = new Title1
                        {
                            text = "שנה_סמסטר"
                        }
                    },
                    yAxis = new Yaxis
                    {
                        title = new Title1
                        {
                            text = "ממוצע ציונים"
                        },
                        plotLines = new List<Plotline> {
                            new Plotline
                            {
                                color = "#808080",
                                value = 0,
                                width = 1
                            }
                        }
                    },
                    series = new List<Series>
                    {
                        new Series
                        {
                            data = series,
                            name = "ממוצע ציונים"
                        }
                        
                    },
                    legend = new Legend
                    {
                        layout = "vertical",
                        align = "right",
                        verticalAlign = "middle",
                        borderWidth = 0
                    },
                    tooltip = new Tooltip1()
                    {
                        headerFormat = "<span style='font-size:10px'>{point.key}</span><table>",
                        pointFormat = "<tr><td style='color:{series.color};padding:0'>{series.name}: </td><td style='padding:0'><b>{point.y}</b></td></tr>",
                        footerFormat = "</table>",
                        shared = true,
                        useHTML = true

                    },
                    exporting = new Exporting
                    {
                        enabled = false
                    }
                     

                };

                return Json(chartModel);
            }
            return Error(result);
            //return null;
        }





    }
}
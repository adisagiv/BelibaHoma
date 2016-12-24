﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BelibaHoma.BLL.Enums;
using BelibaHoma.BLL.Interfaces;
using BelibaHoma.BLL.Models;
using BelibaHoma.BLL.Models.Reports;
using BelibaHoma.Controllers;
using Microsoft.Ajax.Utilities;

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

        //return the data for the report
        [HttpPost]
        public ActionResult GetHourStatistics(HourStatisticsType hourStatisticsType ,int? year)
        {
            if (!year.HasValue)
            {
                year = DateTime.Now.Year;
            }

            var startTime = new DateTime(year.Value, 10, 1);
            var endTime = new DateTime(year.Value + 1, 10, 1);

            var result = _reportService.GetHourStatistics(null, startTime, endTime, hourStatisticsType);

            if (result.Success)
            {
                var series = new List<double>();
                
                for (var i = 1; i <= 12; i++)
                {
                    var hours = 0.0;
                    var month = (i + 10)%12 + 1;

                    if (result.Data.HourStatistics.ContainsKey(month))
                    {
                        hours = result.Data.HourStatistics[month];
                    }

                    series.Add(hours);
                }


                var chartModel = new HighChartModel
                {
                    chart = new Chart
                    {
                        type ="line"
                    },
                    title = new Title
                    {
                        text = "דוח סטטיסטיקה שעות חניכה",
                        x = -20
                    },
                    xAxis = new Xaxis
                    {
                        categories = new List<string>
                        {
                            "Jan",
                            "Feb",
                            "Mar",
                            "Apr",
                            "May",
                            "Jun",
                            "Jul",
                            "Aug",
                            "Sep",
                            "Oct",
                            "Nov",
                            "Dec"
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
                    series =new List<Series>
                    {
                        new Series
                        {
                            data = series,
                            name = "סכום שעות חניכה"
                        }
                        
                    },
                    legend = new Legend
                    {
                        layout= "vertical",
                        align= "right",
                        verticalAlign= "middle",
                        borderWidth = 0
                    },
                    tooltip = new Tooltip
                    {
                        valueSuffix = "H"
                    },
                    exporting = new Exporting
                    {
                        enabled = false
                    }

                };

                return Json(chartModel);
            }

            return null;
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
        public ActionResult GetJoinDropStatistics()
        {
            //if (!year.HasValue)
            //{
            //    year = DateTime.Now.Year;
            //}

            ///// start time begining of time, first creation time, end time, this year 
            //var startTime = new DateTime(2016);
            //var endTime = new DateTime(year.Value + 1, 10, 1);
            //var area1 = null;
            
            
            //if (CurrentUser.UserRole == UserRole.Rackaz)
            //{
            //    var area1 = CurrentUser.Area;
            //    var result1 = _reportService.GetJoinDropStatistics(area1);

            //}

            //else
            //{
            //    var result1 = _reportService.GetJoinDropStatistics(null);
            //}

            /// TODO: if Rackaz send his area
                var result = _reportService.GetJoinDropStatistics(null);
            if (result.Success)
            {
                var chartModel1 = new HighChartJDModel
                {
                    chart = new Chart1
                    {
                        type = "column"
                    },
                    title = new Title2
                    {
                        text = "דוח סטטיסטיקה הצטרפות ונשירת חניכים",
                        //x = -20
                    },
                    xAxis = new Xaxis1
                    {
                        // we will take the years from the dictionary
                        
                        categories = result.Data.YearList.ToArray(),
                      

                        //categories1 = yearList;


                    crosshair= true
                    
                        
                    },
                    yAxis = new Yaxis1
                    {

                        min= new int {},  
                        title = new Title3
                        {
                            text = "כמות מצטרפים ונושרים"
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

            return null;
        }








        [HttpPost]
        public ActionResult AlertsStatistics()
        {
            return View();
        }
    }
}
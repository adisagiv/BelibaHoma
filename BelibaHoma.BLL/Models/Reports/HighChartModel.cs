using System.Collections.Generic;
using System.Security.AccessControl;

namespace BelibaHoma.BLL.Models.Reports
{

    public class HighChartModel
    {
        public Chart chart { get; set; }
        public Title title { get; set; }
        public Subtitle subtitle { get; set; }
        public Xaxis xAxis { get; set; }
        public Yaxis yAxis { get; set; }
        public Tooltip1 tooltip { get; set; }
        public Legend legend { get; set; }
        public List<Series> series { get; set; }
        public Exporting exporting { get; set; }
    }

    public class Exporting
    {
        public bool enabled;
    }

    public class Chart
    {
        public string type { get; set; }
    }

    public class Title
    {
        public string text { get; set; }
        public int x { get; set; }
    }

    public class Subtitle
    {
        public string text { get; set; }
        public int x { get; set; }
    }

    public class Xaxis
    {
        public Title1 title { get; set; }
        public List<string> categories { get; set; }
    }

    public class Yaxis
    {
        public Title1 title { get; set; }
        public List<Plotline> plotLines { get; set; }
    }

    public class Title1
    {
        public string text { get; set; }
    }

    public class Plotline
    {
        public int value { get; set; }
        public int width { get; set; }
        public string color { get; set; }
    }

    public class Tooltip
    {
        public string valueSuffix { get; set; }
    }

    public class Legend
    {
        public string layout { get; set; }
        public string align { get; set; }
        public string verticalAlign { get; set; }
        public int borderWidth { get; set; }
        public bool rtl { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public List<double> data { get; set; }
    }
   
}





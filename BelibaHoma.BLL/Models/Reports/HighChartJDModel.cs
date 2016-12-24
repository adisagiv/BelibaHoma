
using BelibaHoma.BLL.Models;

public class HighChartJDModel
{
    public Chart1 chart { get; set; }
    public Title2 title { get; set; }
    public Subtitle1 subtitle { get; set; }
    public Xaxis1 xAxis { get; set; }
    public Yaxis1 yAxis { get; set; }
    public Tooltip1 tooltip { get; set; }
    public Plotoptions1 plotOptions { get; set; }
    public Series1[] series { get; set; }
    public Exporting1 exporting { get; set; }
}

public class Exporting1
{
    public bool enabled;
}

public class Chart1
{
    public string type { get; set; }
}

public class Title2
{
    public string text { get; set; }
}

public class Subtitle1
{
    public string text { get; set; }
}

public class Xaxis1
{
    public string[] categories { get; set; }
    public bool crosshair { get; set; }
}

public class Yaxis1
{
    public int min { get; set; }
    public Title3 title { get; set; }
}

public class Title3
{
    public string text { get; set; }
}

public class Tooltip1
{
    public string headerFormat { get; set; }
    public string pointFormat { get; set; }
    public string footerFormat { get; set; }
    public bool shared { get; set; }
    public bool useHTML { get; set; }
}

public class Plotoptions1
{
    public Column2 column { get; set; }
}

public class Column2
{
    public float pointPadding { get; set; }
    public int borderWidth { get; set; }
}

public class Series1
{
    public string name { get; set; }
    public int[] data { get; set; }
}

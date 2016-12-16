
public class Rootobject
{
    public Chart chart { get; set; }
    public Title title { get; set; }
    public Xaxis xAxis { get; set; }
    public Yaxis yAxis { get; set; }
    public Series[] series { get; set; }
}

public class Chart
{
    public string type { get; set; }
}

public class Title
{
    public string text { get; set; }
}

public class Xaxis
{
    public string[] categories { get; set; }
}

public class Yaxis
{
    public Title1 title { get; set; }
}

public class Title1
{
    public string text { get; set; }
}

public class Series
{
    public string name { get; set; }
    public int[] data { get; set; }
}

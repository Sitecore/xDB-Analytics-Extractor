namespace xDBAnalyticsExtractor.Models;

public class WebVisitModel
{
    public Browser? Browser { get; set; }
    public Screen? Screen { get; set; }
    public OperatingSystem? OperatingSystem { get; set; }
    public string? Language { get; set; }
}
public record Browser(string BrowserVersion, string BrowserMajorName, string BrowserMinorName);
public record OperatingSystem(string MajorVersion, string MinorVersion, string Name);
public record Screen(int ScreenHeight, int ScreenWidth);
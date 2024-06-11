using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

public class BrowserDataBuilder : XConnectEntityNode
{
    private readonly BrowserData _browserData = new();

    public BrowserDataBuilder AddBrowserMajorName(string majorName)
    {
        _browserData.BrowserMajorName = majorName;
        return this;
    }

    public BrowserDataBuilder AddBrowserMinorName(string minorName)
    {
        _browserData.BrowserMinorName = minorName;
        return this;
    }

    public BrowserDataBuilder AddBrowserVersion(string version)
    {
        _browserData.BrowserVersion = version;
        return this;
    }

    public BrowserData Build()
    {
        return _browserData;
    }
}
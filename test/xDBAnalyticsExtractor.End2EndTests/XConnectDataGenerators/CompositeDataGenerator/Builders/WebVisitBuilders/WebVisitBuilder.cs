using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

public class WebVisitBuilder : XConnectEntityNode
{
    private readonly WebVisit _webVisit;
    private readonly Interaction _interaction;

    public WebVisitBuilder(Interaction interaction)
    {
        _interaction = interaction;
        _webVisit = new WebVisit();
    }

    public WebVisitBuilder AddBrowserData(BrowserData browserData)
    {
        _webVisit.Browser = browserData;
        return this;
    }

    public WebVisitBuilder AddLanguage(string language)
    {
        _webVisit.Language = language;
        return this;
    }

    public WebVisitBuilder AddOperatingSystemData(OperatingSystemData operatingSystemData)
    {
        _webVisit.OperatingSystem = operatingSystemData;
        return this;
    }

    public WebVisitBuilder AddReferrer(string referrer)
    {
        _webVisit.Referrer = referrer;
        return this;
    }

    public WebVisitBuilder AddIsSelfReferrer(bool isSelfReferrer)
    {
        _webVisit.IsSelfReferrer = isSelfReferrer;
        return this;
    }

    public WebVisitBuilder AddScreenData(ScreenData screenData)
    {
        _webVisit.Screen = screenData;
        return this;
    }

    public WebVisitBuilder AddSearchKeywords(string searchKeywords)
    {
        _webVisit.SearchKeywords = searchKeywords;
        return this;
    }

    public WebVisitBuilder AddSiteName(string siteName)
    {
        _webVisit.SiteName = siteName;
        return this;
    }

    public WebVisit Build()
    {
        return _webVisit;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.SetWebVisit(_interaction, _webVisit);
    }
}
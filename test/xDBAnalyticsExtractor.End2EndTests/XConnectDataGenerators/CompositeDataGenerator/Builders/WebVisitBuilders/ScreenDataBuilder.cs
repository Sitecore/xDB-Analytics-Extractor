using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

public class ScreenDataBuilder : XConnectEntityNode
{
    private readonly ScreenData _screenData = new();

    public ScreenDataBuilder AddScreenWidth(int screenWidth)
    {
        _screenData.ScreenWidth = screenWidth;
        return this;
    }

    public ScreenDataBuilder AddScreenHeight(int screenHeight)
    {
        _screenData.ScreenHeight = screenHeight;
        return this;
    }

    public ScreenData Build()
    {
        return _screenData;
    }
}
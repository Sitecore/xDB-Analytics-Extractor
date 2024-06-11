using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;

public class LocaleInfoBuilder : XConnectEntityNode
{
    private readonly LocaleInfo _localeInfo;
    private readonly Interaction _interaction;

    public LocaleInfoBuilder(Interaction interaction)
    {
        _interaction = interaction;
        _localeInfo = new LocaleInfo();
    }

    public LocaleInfoBuilder AddTimeZoneOffset(TimeSpan timeZoneOffset)
    {
        _localeInfo.TimeZoneOffset = timeZoneOffset;
        return this;
    }

    public LocaleInfoBuilder AddGeoCoordinate(GeoCoordinate geoCoordinate)
    {
        _localeInfo.GeoCoordinate = geoCoordinate;
        return this;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.SetLocaleInfo(_interaction, _localeInfo);
    }
}
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;

public class GeoCoordinateBuilder : XConnectEntityNode
{
    private readonly GeoCoordinate _geoCoordinate;

    public GeoCoordinateBuilder(double latitude, double longitude)
    {
        _geoCoordinate = new GeoCoordinate(latitude, longitude);
    }

    public GeoCoordinateBuilder AddLatitude(double latitude)
    {
        _geoCoordinate.Latitude = latitude;
        return this;
    }

    public GeoCoordinateBuilder AddLongitude(double latitude)
    {
        _geoCoordinate.Longitude = latitude;
        return this;
    }

    public GeoCoordinate Build()
    {
        return _geoCoordinate;
    }
}
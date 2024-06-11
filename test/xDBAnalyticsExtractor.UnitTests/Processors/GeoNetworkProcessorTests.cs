using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class GeoNetworkProcessorTests
{
    private readonly Guid _interactionId = Guid.Parse("39f20607-0f05-4af1-bb46-14fcd36c93fe");
    private IpInfo? _geoNetwork;
    private IpInfo? _geoNetworkWithNulls;
    private GeoNetworkModel? _expectedValidGeoNetworkModel;
    private GeoNetworkModel? _expectedGeoNetworkModelWithNulls;

    [OneTimeSetUp]
    public void Setup()
    {
        _geoNetwork = new IpInfo("127.0.0.1")
        {
            City = "test",
            Country = "test",
            AreaCode = "test",
            Region = "test",
            MetroCode = "test",
            Latitude = 20D,
            Longitude = 20D,
            LocationId = Guid.Empty,
            PostalCode = "test",
            BusinessName = "test",
            Isp = "test",
        };
        _geoNetworkWithNulls = new IpInfo("127.0.0.1")
        {
            City = null,
            Country = null,
            AreaCode = null,
            Region = null,
            MetroCode = null,
            Latitude = null,
            Longitude = null,
            LocationId = null,
            PostalCode = null,
            BusinessName = null,
            Isp = null,
        };
        
        _expectedValidGeoNetworkModel = new GeoNetworkModel()
        {
            City = "test",
            BusinessName = "test",
            Country = "test",
            Latitude = 20D,
            Longitude = 20D,
            LocationId = Guid.Empty,
            PostalCode = "test",
            InteractionId = _interactionId,
            Region = "test",
            Area = "test",
            Metro = "test",
            IspName = "test",
        };
        
        _expectedGeoNetworkModelWithNulls = new GeoNetworkModel()
        {
            City = "null",
            BusinessName = "null",
            Country = "null",
            Latitude = null,
            Longitude = null,
            LocationId = null,
            PostalCode = "null",
            InteractionId = _interactionId,
            Region = "null",
            Area = "null",
            Metro = "null",
            IspName = "null",
        };
    }
    
    [Test]
    public void Process_WhenProvidedValidData_ReturnsGeoNetworkModel()
    {
        var actualGeoNetworkModel = GeoNetworkProcessor.Process(_geoNetwork, _interactionId);

        actualGeoNetworkModel.Should().NotBeNull()
            .And.BeEquivalentTo(_expectedValidGeoNetworkModel);
    }

    [Test]
    public void Process_WhenProvidedNullValues_ReturnsGeoNetworkModelWithNullAsString()
    {
        var actualGeoNetworkModel = GeoNetworkProcessor.Process(_geoNetworkWithNulls, _interactionId);

        actualGeoNetworkModel.Should().NotBeNull()
            .And.BeEquivalentTo(_expectedGeoNetworkModelWithNulls);
    }
}
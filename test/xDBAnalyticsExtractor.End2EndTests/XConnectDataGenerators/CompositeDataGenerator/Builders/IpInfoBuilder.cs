using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;

public class IpInfoBuilder : XConnectEntityNode
{
    private Interaction _interaction;
    private readonly IpInfo _ipInfo;

    public IpInfoBuilder(Interaction interaction, string iPv4Address)
    {
        _interaction = interaction;
        _ipInfo = new IpInfo(iPv4Address);
    }

    public IpInfoBuilder AddAreaCode(string areaCode)
    {
        _ipInfo.AreaCode = areaCode;
        return this;
    }

    public IpInfoBuilder AddBusinessName(string businessName)
    {
        _ipInfo.BusinessName = businessName;
        return this;
    }

    public IpInfoBuilder AddCity(string city)
    {
        _ipInfo.City = city;
        return this;
    }

    public IpInfoBuilder AddCountry(string country)
    {
        _ipInfo.Country = country;
        return this;
    }

    public IpInfoBuilder AddIsp(string isp)
    {
        _ipInfo.Isp = isp;
        return this;
    }

    public IpInfoBuilder AddDns(string dns)
    {
        _ipInfo.Dns = dns;
        return this;
    }

    public IpInfoBuilder AddLatitude(double latitude)
    {
        _ipInfo.Latitude = latitude;
        return this;
    }

    public IpInfoBuilder AddLongitude(double longitude)
    {
        _ipInfo.Longitude = longitude;
        return this;
    }

    public IpInfoBuilder AddLocationId(Guid locationId)
    {
        _ipInfo.LocationId = locationId;
        return this;
    }

    public IpInfoBuilder AddMetroCode(string metroCode)
    {
        _ipInfo.MetroCode = metroCode;
        return this;
    }

    public IpInfoBuilder AddPostalCode(string postalCode)
    {
        _ipInfo.PostalCode = postalCode;
        return this;
    }

    public IpInfoBuilder AddRegion(string region)
    {
        _ipInfo.Region = region;
        return this;
    }

    public IpInfoBuilder AddUrl(string url)
    {
        _ipInfo.Url = url;
        return this;
    }

    public IpInfo Build()
    {
        return _ipInfo;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.SetIpInfo(_interaction, _ipInfo);
    }
}
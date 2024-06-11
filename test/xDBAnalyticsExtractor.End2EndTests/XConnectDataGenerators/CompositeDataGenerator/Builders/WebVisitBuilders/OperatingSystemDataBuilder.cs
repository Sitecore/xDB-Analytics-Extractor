using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

public class OperatingSystemDataBuilder : XConnectEntityNode
{
    private readonly OperatingSystemData _operatingSystemData = new();

    public OperatingSystemDataBuilder AddMajorVersion(string majorVersion)
    {
        _operatingSystemData.MajorVersion = majorVersion;
        return this;
    }

    public OperatingSystemDataBuilder AddMinorVersion(string minorVersion)
    {
        _operatingSystemData.MinorVersion = minorVersion;
        return this;
    }

    public OperatingSystemDataBuilder AddName(string name)
    {
        _operatingSystemData.Name = name;
        return this;
    }
    public OperatingSystemData Build()
    {
        return _operatingSystemData;
    }
}
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;

public class UserAgentInfoBuilder : XConnectEntityNode
{
    private readonly Interaction _interaction;
    private readonly UserAgentInfo _userAgentInfo;

    public UserAgentInfoBuilder(Interaction interaction)
    {
        _interaction = interaction;
        _userAgentInfo = new UserAgentInfo();
    }

    public UserAgentInfoBuilder AddCanSupportTouchScreen(bool canSupportTouchScreen)
    {
        _userAgentInfo.CanSupportTouchScreen = canSupportTouchScreen;
        return this;
    }

    public UserAgentInfoBuilder AddDeviceType(string deviceType)
    {
        _userAgentInfo.DeviceType = deviceType;
        return this;
    }

    public UserAgentInfoBuilder AddDeviceVendor(string deviceVendor)
    {
        _userAgentInfo.DeviceVendor = deviceVendor;
        return this;
    }

    public UserAgentInfoBuilder AddDeviceVendorHardwareModel(string deviceVendorHardwareModel)
    {
        _userAgentInfo.DeviceVendorHardwareModel = deviceVendorHardwareModel;
        return this;
    }

    public override void BuildInteraction()
    {
        xConnectClient.Result.SetUserAgentInfo(_interaction, _userAgentInfo);
    }

    public UserAgentInfo Build()
    {
        return _userAgentInfo;
    }
}
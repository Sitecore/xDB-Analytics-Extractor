using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class DeviceProcessorTests
{
    private WebVisit? _webVisit;
    private WebVisit? _webVisitWithNulls;
    private UserAgentInfo? _userAgentInfo;
    private UserAgentInfo? _userAgentInfoWithNulls;
    private readonly Guid _interactionId = Guid.Parse("39f20607-0f05-4af1-bb46-14fcd36c93fe");
    private DeviceModel? _expectedValidDeviceModel;
    private DeviceModel? _expectedDeviceModelWithNulls;

    [OneTimeSetUp]
    public void Setup()
    {
        _webVisit = new WebVisit()
        {
            Browser = new BrowserData()
            {
                BrowserMajorName = "Google Chrome",
                BrowserMinorName = "test",
                BrowserVersion = "10.2.3"
            },
            IsSelfReferrer = true,
            Language = "English",
            Referrer = "Chrome",
            SiteName = "www.google.com",
            SearchKeywords = "test",
            OperatingSystem = new OperatingSystemData()
            {
                Name = "Linux",
                MajorVersion = "22",
                MinorVersion = "04"
            },
            Screen = new ScreenData()
            {
                ScreenHeight = 15,
                ScreenWidth = 15
            },
            XObject = { }
        };
        _userAgentInfo = new UserAgentInfo()
        {
            DeviceType = "Desktop",
            DeviceVendor = "Vendor",
            CanSupportTouchScreen = true,
            DeviceVendorHardwareModel = "test"
        };
        
        _webVisitWithNulls = new WebVisit()
        {
            Browser = new BrowserData()
            {
                BrowserMajorName = null,
                BrowserMinorName = null,
                BrowserVersion = null
            },
            IsSelfReferrer = true,
            Language = null,
            Referrer = "Chrome",
            SiteName = "www.google.com",
            SearchKeywords = "test",
            OperatingSystem = null,
            Screen = null,
            XObject = { }
        };
        _userAgentInfoWithNulls = new UserAgentInfo()
        {
            DeviceType = null,
            DeviceVendor = null,
            CanSupportTouchScreen = false,
            DeviceVendorHardwareModel = null
        };
        
        _expectedValidDeviceModel = new DeviceModel()
        {
            Language = "English",
            BrowserVersion = "10.2.3",
            DeviceVendor = "Vendor",
            CanSupportTouchScreen = true,
            BrowserMinorName = "test",
            OperatingSystem = "Linux",
            DeviceVendorHardwareModel = "test",
            BrowserMajorName = "Google Chrome",
            DeviceCategory = "Desktop",
            InteractionId = Guid.Parse("39f20607-0f05-4af1-bb46-14fcd36c93fe"),
            ScreenSize = "15x15",
            OperatingSystemVersion = "22.04"
        };
        _expectedDeviceModelWithNulls = new DeviceModel()
        {
            Language = "null",
            BrowserVersion = "null",
            DeviceVendor = "null",
            CanSupportTouchScreen = false,
            BrowserMinorName = "null",
            OperatingSystem = "null",
            DeviceVendorHardwareModel = "null",
            BrowserMajorName = "null",
            DeviceCategory = "null",
            InteractionId = Guid.Parse("39f20607-0f05-4af1-bb46-14fcd36c93fe"),
            ScreenSize = "null",
            OperatingSystemVersion = "null"
        };
    }

    [Test]
    public void Process_WhenProvidedValidData_ReturnsDeviceModel()
    {
        var actualDeviceModel = DeviceProcessor.Process(_webVisit, _userAgentInfo, _interactionId);

        actualDeviceModel.Should().NotBeNull()
            .And.BeEquivalentTo(_expectedValidDeviceModel);
    }

    [Test]
    public void Process_WhenProvidedNullWebVisit_ReturnsNull()
    {
        var actualDeviceModel = DeviceProcessor.Process(null, _userAgentInfo, _interactionId);

        actualDeviceModel.Should().BeNull();
    }

    [Test]
    public void Process_WhenProvidedNullUserAgentInfo_ReturnsNull()
    {
        var actualDeviceModel = DeviceProcessor.Process(_webVisit, null, _interactionId);

        actualDeviceModel.Should().BeNull();
    }

    [Test]
    public void Process_WhenProvidedNullInteractionId_ReturnsNull()
    {
        var actualDeviceModel = DeviceProcessor.Process(_webVisit, _userAgentInfo, null);

        actualDeviceModel.Should().BeNull();
    }

    [Test]
    public void Process_WhenProvidedNullInAllParameters_ReturnsNull()
    {
        var actualDeviceModel = DeviceProcessor.Process(null, null, null);

        actualDeviceModel.Should().BeNull();
    }

    [Test]
    public void Process_WhenProvidedNullInnerValues_ReturnsDeviceModelWithNullAsString()
    {
        var actualDeviceModel = DeviceProcessor.Process(_webVisitWithNulls, _userAgentInfoWithNulls, _interactionId);
        
        actualDeviceModel.Should().NotBeNull()
            .And.BeEquivalentTo(_expectedDeviceModelWithNulls);
    }
}
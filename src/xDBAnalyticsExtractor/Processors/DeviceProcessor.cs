using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.Processors;

public static class DeviceProcessor
{
    public static DeviceModel? Process(WebVisit? webVisit, UserAgentInfo? userAgentInfo, Guid? interactionId)
    {
        if (webVisit is null || userAgentInfo is null || interactionId is null)
            return null;
        
        var browserData = webVisit?.Browser;
        var screenData = webVisit?.Screen;
        var operatingSystemData = webVisit?.OperatingSystem;

        return new DeviceModel()
        {
            BrowserMajorName = browserData?.BrowserMajorName ?? "null",
            BrowserMinorName = browserData?.BrowserMinorName ?? "null",
            BrowserVersion = browserData?.BrowserVersion ?? "null",
            OperatingSystem = operatingSystemData?.Name ?? "null",
            OperatingSystemVersion = operatingSystemData is null
                ? "null"
                : $"{operatingSystemData?.MajorVersion}.{operatingSystemData?.MinorVersion}",
            CanSupportTouchScreen = userAgentInfo?.CanSupportTouchScreen ?? false,
            DeviceCategory = userAgentInfo?.DeviceType ?? "null",
            DeviceVendor = userAgentInfo?.DeviceVendor ?? "null",
            DeviceVendorHardwareModel = userAgentInfo?.DeviceVendorHardwareModel ?? "null",
            InteractionId = interactionId,
            Language = webVisit?.Language ?? "null",
            ScreenSize = screenData is null
                ? "null"
                : $"{screenData?.ScreenWidth}x{screenData?.ScreenHeight}"
        };
    }
}
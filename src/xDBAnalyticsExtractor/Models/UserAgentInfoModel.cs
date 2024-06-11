namespace xDBAnalyticsExtractor.Models;

public class UserAgentInfoModel
{
    public bool CanSupportTouchScreen { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceVendor { get; set; }
    public string? DeviceVendorHardwareModel { get; set; }
}
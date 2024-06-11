using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Devices")]
public class DeviceModel : IModel
{
    public long Id { get; }
    public string? BrowserVersion { get; set; }
    public string? BrowserMajorName { get; set; }
    public string? BrowserMinorName { get; set; }
    public string? DeviceCategory { get; set; }
    public string? ScreenSize { get; set; }
    public string? OperatingSystem { get; set; }
    public string? OperatingSystemVersion { get; set; }
    public string? Language { get; set; }
    public bool? CanSupportTouchScreen { get; set; }
    public string? DeviceVendor { get; set; }
    public string? DeviceVendorHardwareModel { get; set; }
    public Guid? InteractionId { get; set; }
}
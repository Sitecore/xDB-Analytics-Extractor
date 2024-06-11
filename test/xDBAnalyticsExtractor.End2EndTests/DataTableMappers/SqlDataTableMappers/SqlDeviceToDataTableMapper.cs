using System.Data;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;

public class SqlDeviceToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable devicesDataTable = new DataTable();
        
        DataColumn browserVersion = new DataColumn("BrowserVersion", typeof(string))
        {
            MaxLength = 255
        };
        devicesDataTable.Columns.Add(browserVersion);

        DataColumn browserMajorName = new DataColumn("BrowserMajorName", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(browserMajorName);

        DataColumn browserMinorName = new DataColumn("BrowserMinorName", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(browserMinorName);

        DataColumn deviceCategory = new DataColumn("DeviceCategory", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(deviceCategory);

        DataColumn screenSize = new DataColumn("ScreenSize", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(screenSize);

        DataColumn operatingSystem = new DataColumn("OperatingSystem", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(operatingSystem);

        DataColumn operatingSystemVersion = new DataColumn("OperatingSystemVersion", typeof(string))
        {
            MaxLength = 255
        };
        devicesDataTable.Columns.Add(operatingSystemVersion);

        DataColumn language = new DataColumn("Language", typeof(string))
        {
            MaxLength = 10
        };
        devicesDataTable.Columns.Add(language);

        DataColumn canSupportTouchScreen = new DataColumn("CanSupportTouchScreen", typeof(bool));
        devicesDataTable.Columns.Add(canSupportTouchScreen);

        DataColumn deviceVendor = new DataColumn("DeviceVendor", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(deviceVendor);

        DataColumn deviceVendorHardwareModel = new DataColumn("DeviceVendorHardwareModel", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        devicesDataTable.Columns.Add(deviceVendorHardwareModel);

        DataColumn interactionId = new DataColumn("InteractionId", typeof(Guid));
        devicesDataTable.Columns.Add(interactionId);

        foreach (var interaction in interactions)
        {
            devicesDataTable.Rows.Add(
                interaction.WebVisit().Browser.BrowserVersion ?? "null",
                interaction.WebVisit().Browser.BrowserMajorName ?? "null",
                interaction.WebVisit().Browser.BrowserMinorName ?? "null",
                interaction.GetFacet<UserAgentInfo>().DeviceType ?? "null",
                interaction.WebVisit().Screen is null
                    ? "null"
                    : $"{interaction.WebVisit().Screen.ScreenWidth}x{interaction.WebVisit().Screen.ScreenHeight}",
                interaction.WebVisit().OperatingSystem.Name ?? "null",
                interaction.WebVisit().OperatingSystem is null
                    ? "null"
                    : $"{interaction.WebVisit().OperatingSystem.MajorVersion}.{interaction.WebVisit().OperatingSystem.MinorVersion}",
                interaction.WebVisit().Language ?? "null",
                interaction.GetFacet<UserAgentInfo>().CanSupportTouchScreen,
                interaction.GetFacet<UserAgentInfo>().DeviceVendor ?? "null",
                interaction.GetFacet<UserAgentInfo>().DeviceVendorHardwareModel ?? "null",
                interaction.Id
            ).AcceptChanges();
        }

        return devicesDataTable;
    }
}
using xDBAnalyticsExtractor.Interfaces;
using xDBAnalyticsExtractor.Models;
using System.Data;
using System.Data.SqlClient;

namespace xDBAnalyticsExtractor.Extentions;

internal static class SqlConnectionExtenstions
{
    public static void BulkMerge<T>(this SqlConnection connection, IEnumerable<T>? records) where T : IModel?
    {
        try
        {
            if (records is null)
            {
                return;
            }
            var tmpTable = CreateTempTableStatementBasedOnEntity(typeof(T));

            var cmd = new SqlCommand(tmpTable, connection);
            cmd.ExecuteNonQuery();
            var dt = BuildDataSet(records);

            using (var bulk = new SqlBulkCopy(connection))
            {
                bulk.BulkCopyTimeout = 0;
                bulk.DestinationTableName = "#temp";
                foreach (DataColumn column in dt.Columns)
                {
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping(column.ColumnName, column.ColumnName));
                }
                bulk.WriteToServer(dt);
            }

            cmd.CommandTimeout = 0;
            cmd.CommandText = CreateMergeStatementBasedOnEntity(typeof(T));
            cmd.ExecuteNonQuery();
            cmd.CommandText = "drop table #temp";
            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            var sql = "drop table #temp";
            var cmd = new SqlCommand(sql, connection);
            cmd.ExecuteNonQuery();
        }
        
    }

    private static string CreateTempTableStatementBasedOnEntity(Type type)
    {
        string sql = string.Empty;
        if (type == typeof(InteractionModel))
        {
            sql = @"CREATE TABLE #temp (
        InteractionId UNIQUEIDENTIFIER,
        StartDateTime DATETIME2,
        EndDateTime DATETIME2,
        LastModified DATETIME2,
        CampaignId UNIQUEIDENTIFIER,
        ChannelId UNIQUEIDENTIFIER NOT NULL,
        EngagementValue INT,
        Duration TIME,
        UserAgent NVARCHAR(MAX),
        Bounces INT,
        Conversions INT,
        Converted INT,
        TimeOnSite INT,
        PageViews INT,
        OutcomeOccurrences INT,
        MonetaryValue MONEY
    );";

        }
        else if (type == typeof(EventDefinitionModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );";
        }
        else if (type == typeof(CampaignDefinitionModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );";
        }
        else if (type == typeof(GoalDefinitionModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );";
        }
        else if (type == typeof(OutcomeDefinitionModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        Alias NVARCHAR(MAX),
        CreatedDate DATETIME2,
        LastModifiedDate DATETIME2,
        CreatedBy NVARCHAR(MAX),
        LastModifiedBy NVARCHAR(MAX),
        Culture NVARCHAR(10),
        Description NVARCHAR(MAX)
    );";
        }
        else if (type == typeof(CampaignModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        CampaignDefinitionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(DeviceModel))
        {
            sql = @"CREATE TABLE #temp (
        Id BIGINT,
        BrowserVersion NVARCHAR(255),
        BrowserMajorName NVARCHAR(MAX),
        BrowserMinorName NVARCHAR(MAX),
        DeviceCategory NVARCHAR(MAX),
        ScreenSize NVARCHAR(MAX),
        OperatingSystem NVARCHAR(MAX),
        OperatingSystemVersion NVARCHAR(255),
        Language NVARCHAR(10),
        CanSupportTouchScreen BIT,
        DeviceVendor NVARCHAR(MAX),
        DeviceVendorHardwareModel NVARCHAR(MAX),
        InteractionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(DownloadModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        EventDefinitionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(GeoNetworkModel))
        {
            sql = @"CREATE TABLE #temp (
        Id BIGINT IDENTITY(1,1),
        Area NVARCHAR(MAX),
        Country NVARCHAR(MAX),
        Region NVARCHAR(MAX),
        Metro NVARCHAR(MAX),
        City NVARCHAR(MAX),
        Latitude FLOAT,
        Longitude FLOAT,
        LocationId UNIQUEIDENTIFIER,
        PostalCode NVARCHAR(20),
        IspName NVARCHAR(MAX),
        BusinessName NVARCHAR(MAX),
        InteractionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(GoalModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME
    );";
        }
        else if (type == typeof(OutcomeModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        CurrencyCode NVARCHAR(3),
        MonetaryValue MONEY
    );";
        }
        else if (type == typeof(PageViewModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        ItemLanguage NVARCHAR(10),
        ItemVersion INT,
        Url NVARCHAR(MAX),
        EventDefinitionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(SearchModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        InteractionId UNIQUEIDENTIFIER,
        CustomValues NVARCHAR(MAX),
        Data NVARCHAR(MAX),
        DataKey NVARCHAR(MAX),
        DefinitionId UNIQUEIDENTIFIER,
        ItemId UNIQUEIDENTIFIER,
        EngagementValue INT,
        ParentEventId UNIQUEIDENTIFIER,
        Text NVARCHAR(MAX),
        Timestamp DATETIME2,
        Duration TIME,
        Keywords NVARCHAR(MAX),
        EventDefinitionId UNIQUEIDENTIFIER
    );";
        }
        else if (type == typeof(ChannelModel))
        {
            sql = @"CREATE TABLE #temp (
        Id UNIQUEIDENTIFIER,
        Code NVARCHAR(MAX) NULL,
        Description NVARCHAR(MAX) NULL,
        Name NVARCHAR(MAX) NULL,
        UriCulture NVARCHAR(MAX) NULL,
        UriPath NVARCHAR(MAX) NULL,
        Uri NVARCHAR(MAX) NULL
    );";
        }
        return sql;
    }

    private static DataTable BuildDataSet<T>(IEnumerable<T> records) where T : IModel?
    {
        var dt = new DataTable();
        if (typeof(T) == typeof(InteractionModel))
        {
            AddInteractionsToDataTable(dt, records as IEnumerable<InteractionModel>);
        }
        else if (typeof(T) == typeof(EventDefinitionModel))
        {
            AddDefinitionsToDataTable(dt, records as IEnumerable<EventDefinitionModel>);
        }
        else if (typeof(T) == typeof(CampaignDefinitionModel))
        {
            AddDefinitionsToDataTable(dt, records as IEnumerable<CampaignDefinitionModel>);
        }
        else if (typeof(T) == typeof(GoalDefinitionModel))
        {
            AddDefinitionsToDataTable(dt, records as IEnumerable<GoalDefinitionModel>);
        }
        else if (typeof(T) == typeof(OutcomeDefinitionModel))
        {
            AddDefinitionsToDataTable(dt, records as IEnumerable<OutcomeDefinitionModel>);
        }
        else if (typeof(T) == typeof(CampaignModel))
        {
            AddCampaignsToDataTable(dt, records as IEnumerable<CampaignModel>);
        }
        else if (typeof(T) == typeof(DeviceModel))
        {
            AddDevicesToDataTable(dt, records as IEnumerable<DeviceModel>);
        }
        else if (typeof(T) == typeof(DownloadModel))
        {
            AddDownloadsToDataTable(dt, records as IEnumerable<DownloadModel>);
        }
        else if (typeof(T) == typeof(GeoNetworkModel))
        {
            AddGeoNetworksToDataTable(dt, records as IEnumerable<GeoNetworkModel>);
        }
        else if (typeof(T) == typeof(GoalModel))
        {
            AddGoalsToDataTable(dt, records as IEnumerable<GoalModel>);
        }
        else if (typeof(T) == typeof(OutcomeModel))
        {
            AddOutcomesToDataTable(dt, records as IEnumerable<OutcomeModel>);
        }
        else if (typeof(T) == typeof(PageViewModel))
        {
            AddPageViewsToDataTable(dt, records as IEnumerable<PageViewModel>);
        }
        else if (typeof(T) == typeof(SearchModel))
        {
            AddSearchesToDataTable(dt, records as IEnumerable<SearchModel>);
        }
        else if (typeof(T) == typeof(ChannelModel))
        {
            AddChannelsToDataTable(dt, records as IEnumerable<ChannelModel>);
        }
        return dt;
    }

    private static void AddInteractionsToDataTable(DataTable dt, IEnumerable<InteractionModel>? interactions)
    {
        if (interactions is null)
        {
            return;
        }
        dt.Columns.Add(new DataColumn("InteractionId", typeof(Guid)));
        dt.Columns.Add(new DataColumn("StartDateTime", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("EndDateTime", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("LastModified", typeof(DateTime)));
        dt.Columns.Add(new DataColumn("CampaignId", typeof(Guid)));
        dt.Columns.Add(new DataColumn("ChannelId", typeof(Guid)));
        dt.Columns.Add(new DataColumn("EngagementValue", typeof(int)));
        dt.Columns.Add(new DataColumn("Duration", typeof(TimeSpan)));
        dt.Columns.Add(new DataColumn("UserAgent", typeof(string)));
        dt.Columns.Add(new DataColumn("Bounces", typeof(int)));
        dt.Columns.Add(new DataColumn("Conversions", typeof(int)));
        dt.Columns.Add(new DataColumn("Converted", typeof(int)));
        dt.Columns.Add(new DataColumn("TimeOnSite", typeof(int)));
        dt.Columns.Add(new DataColumn("PageViews", typeof(int)));
        dt.Columns.Add(new DataColumn("OutcomeOccurrences", typeof(int)));
        dt.Columns.Add(new DataColumn("MonetaryValue", typeof(decimal)));

        foreach (var interaction in interactions)
        {
            var row = dt.NewRow();
            row["InteractionId"] = interaction?.InteractionId;
            row["StartDateTime"] = interaction?.StartDateTime is null ? DBNull.Value : interaction?.StartDateTime;
            row["EndDateTime"] = interaction?.EndDateTime is null ? DBNull.Value : interaction.EndDateTime;
            row["LastModified"] = interaction?.LastModified is null ? DBNull.Value : interaction.LastModified;
            row["CampaignId"] = interaction?.CampaignId is null ? DBNull.Value : interaction.CampaignId;
            row["ChannelId"] = interaction?.ChannelId is null ? DBNull.Value : interaction.ChannelId;
            row["EngagementValue"] = interaction?.EngagementValue is null ? DBNull.Value : interaction.EngagementValue;
            row["Duration"] = interaction?.Duration is null ? DBNull.Value : interaction.Duration;
            row["UserAgent"] = interaction?.UserAgent is null ? DBNull.Value : interaction.UserAgent;
            row["Bounces"] = interaction?.Bounces is null ? DBNull.Value : interaction.Bounces;
            row["Conversions"] = interaction?.Conversions is null ? DBNull.Value : interaction.Conversions;
            row["Converted"] = interaction?.Converted is null ? DBNull.Value : interaction.Converted;
            row["TimeOnSite"] = interaction?.TimeOnSite is null ? DBNull.Value : interaction.TimeOnSite;
            row["PageViews"] = interaction?.PageViews is null ? DBNull.Value : interaction.PageViews;
            row["OutcomeOccurrences"] = interaction?.OutcomeOccurrences is null ? DBNull.Value : interaction.OutcomeOccurrences;
            row["MonetaryValue"] = interaction?.MonetaryValue is null ? DBNull.Value : interaction.MonetaryValue;
            dt.Rows.Add(row);
        }
    }
    private static void AddDefinitionsToDataTable(DataTable dt, IEnumerable<DefinitionModel>? definitions)
    {
        if (definitions is null)
        {
            return;
        }
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("Alias", typeof(string));
        dt.Columns.Add("CreatedDate", typeof(DateTime));
        dt.Columns.Add("LastModifiedDate", typeof(DateTime));
        dt.Columns.Add("CreatedBy", typeof(string));
        dt.Columns.Add("LastModifiedBy", typeof(string));
        dt.Columns.Add("Culture", typeof(string));
        dt.Columns.Add("Description", typeof(string));

        foreach (var definition in definitions)
        {
            var row = dt.NewRow();
            row["Id"] = definition.Id;
            row["Alias"] = definition.Alias;
            row["CreatedDate"] = definition.CreatedDate;
            row["LastModifiedDate"] = definition.LastModifiedDate;
            row["CreatedBy"] = definition.CreatedBy;
            row["LastModifiedBy"] = definition.LastModifiedBy;
            row["Culture"] = definition.Culture;
            row["Description"] = definition.Description;
            dt.Rows.Add(row);
        }
    }
    private static void AddCampaignsToDataTable(DataTable dt, IEnumerable<CampaignModel>? campaigns)
    {
        if (campaigns is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));
        dt.Columns.Add("CampaignDefinitionId", typeof(Guid));

        foreach (var campaign in campaigns)
        {
            var row = dt.NewRow();
            row["CustomValues"] = campaign.CustomValues is null ? DBNull.Value : campaign.CustomValues;
            row["Data"] = campaign.Data is null ? DBNull.Value : campaign.Data;
            row["DataKey"] = campaign.DataKey is null ? DBNull.Value : campaign.DataKey;
            row["DefinitionId"] = campaign.DefinitionId is null ? DBNull.Value : campaign.DefinitionId;
            row["ItemId"] = campaign.ItemId is null ? DBNull.Value : campaign.ItemId;
            row["EngagementValue"] = campaign.EngagementValue is null ? DBNull.Value : campaign.EngagementValue;
            row["Id"] = campaign.Id;
            row["ParentEventId"] = campaign.ParentEventId is null ? DBNull.Value : campaign.ParentEventId;
            row["Text"] = campaign.Text is null ? DBNull.Value : campaign.Text;
            row["Timestamp"] = campaign.Timestamp is null ? DBNull.Value : campaign.Timestamp;
            row["Duration"] = campaign.Duration is null ? DBNull.Value : campaign.Duration;
            row["InteractionId"] = campaign.InteractionId is null ? DBNull.Value : campaign.InteractionId;
            row["CampaignDefinitionId"] = campaign.CampaignDefinitionId is null ? DBNull.Value : campaign.CampaignDefinitionId;
            dt.Rows.Add(row);
        }
    }
    private static void AddDevicesToDataTable(DataTable dt, IEnumerable<DeviceModel>? devices)
    {
        if (devices is null)
        {
            return;
        }
        dt.Columns.Add("BrowserVersion", typeof(string));
        dt.Columns.Add("BrowserMajorName", typeof(string));
        dt.Columns.Add("BrowserMinorName", typeof(string));
        dt.Columns.Add("DeviceCategory", typeof(string));
        dt.Columns.Add("ScreenSize", typeof(string));
        dt.Columns.Add("OperatingSystem", typeof(string));
        dt.Columns.Add("OperatingSystemVersion", typeof(string));
        dt.Columns.Add("Language", typeof(string));
        dt.Columns.Add("CanSupportTouchScreen", typeof(bool));
        dt.Columns.Add("DeviceVendor", typeof(string));
        dt.Columns.Add("DeviceVendorHardwareModel", typeof(string));
        dt.Columns.Add("InteractionId", typeof(Guid));

        foreach (var device in devices)
        {
            var row = dt.NewRow();
            row["BrowserVersion"] = device.BrowserVersion is null ? DBNull.Value : device.BrowserVersion;
            row["BrowserMajorName"] = device.BrowserMajorName is null ? DBNull.Value : device.BrowserMajorName;
            row["BrowserMinorName"] = device.BrowserMinorName is null ? DBNull.Value : device.BrowserMinorName;
            row["DeviceCategory"] = device.DeviceCategory is null ? DBNull.Value : device.DeviceCategory;
            row["ScreenSize"] = device.ScreenSize is null ? DBNull.Value : device.ScreenSize;
            row["OperatingSystem"] = device.OperatingSystem is null ? DBNull.Value : device.OperatingSystem;
            row["OperatingSystemVersion"] = device.OperatingSystemVersion is null ? DBNull.Value : device.OperatingSystemVersion;
            row["Language"] = device.Language is null ? DBNull.Value : device.Language;
            row["CanSupportTouchScreen"] = device.CanSupportTouchScreen is null ? DBNull.Value : device.CanSupportTouchScreen;
            row["DeviceVendor"] = device.DeviceVendor is null ? DBNull.Value : device.DeviceVendor;
            row["DeviceVendorHardwareModel"] = device.DeviceVendorHardwareModel is null ? DBNull.Value : device.DeviceVendorHardwareModel;
            row["InteractionId"] = device.InteractionId is null ? DBNull.Value : device.InteractionId;
            dt.Rows.Add(row);
        }
    }
    private static void AddDownloadsToDataTable(DataTable dt, IEnumerable<DownloadModel>? downloads)
    {
        if (downloads is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));
        dt.Columns.Add("EventDefinitionId", typeof(Guid));

        foreach (var download in downloads)
        {
            var row = dt.NewRow();
            row["CustomValues"] = download.CustomValues is null ? DBNull.Value : download.CustomValues;
            row["Data"] = download.Data is null ? DBNull.Value : download.Data;
            row["DataKey"] = download.DataKey is null ? DBNull.Value : download.DataKey;
            row["DefinitionId"] = download.DefinitionId is null ? DBNull.Value : download.DefinitionId;
            row["ItemId"] = download.ItemId is null ? DBNull.Value : download.ItemId;
            row["EngagementValue"] = download.EngagementValue is null ? DBNull.Value : download.EngagementValue;
            row["Id"] = download.Id;
            row["ParentEventId"] = download.ParentEventId is null ? DBNull.Value : download.ParentEventId;
            row["Text"] = download.Text is null ? DBNull.Value : download.Text;
            row["Timestamp"] = download.Timestamp is null ? DBNull.Value : download.Timestamp;
            row["Duration"] = download.Duration is null ? DBNull.Value : download.Duration;
            row["InteractionId"] = download.InteractionId is null ? DBNull.Value : download.InteractionId;
            row["EventDefinitionId"] = download.EventDefinitionId;
            dt.Rows.Add(row);
        }
    }
    private static void AddGeoNetworksToDataTable(DataTable dt, IEnumerable<GeoNetworkModel>? geoNetworks)
    {
        if (geoNetworks is null)
        {
            return;
        }
        dt.Columns.Add("Area", typeof(string));
        dt.Columns.Add("Country", typeof(string));
        dt.Columns.Add("Region", typeof(string));
        dt.Columns.Add("Metro", typeof(string));
        dt.Columns.Add("City", typeof(string));
        dt.Columns.Add("Latitude", typeof(double));
        dt.Columns.Add("Longitude", typeof(double));
        dt.Columns.Add("LocationId", typeof(Guid));
        dt.Columns.Add("PostalCode", typeof(string));
        dt.Columns.Add("IspName", typeof(string));
        dt.Columns.Add("BusinessName", typeof(string));
        dt.Columns.Add("InteractionId", typeof(Guid));

        foreach (var geoNetwork in geoNetworks)
        {
            var row = dt.NewRow();
            row["Area"] = geoNetwork.Area is null ? DBNull.Value : geoNetwork.Area;
            row["Country"] = geoNetwork.Country is null ? DBNull.Value : geoNetwork.Country;
            row["Region"] = geoNetwork.Region is null ? DBNull.Value : geoNetwork.Region;
            row["Metro"] = geoNetwork.Metro is null ? DBNull.Value : geoNetwork.Metro;
            row["City"] = geoNetwork.City is null ? DBNull.Value : geoNetwork.City;
            row["Latitude"] = geoNetwork.Latitude is null ? DBNull.Value : geoNetwork.Latitude;
            row["Longitude"] = geoNetwork.Longitude is null ? DBNull.Value : geoNetwork.Longitude;
            row["LocationId"] = geoNetwork.LocationId is null ? DBNull.Value : geoNetwork.LocationId;
            row["PostalCode"] = geoNetwork.PostalCode is null ? DBNull.Value : geoNetwork.PostalCode;
            row["IspName"] = geoNetwork.IspName is null ? DBNull.Value : geoNetwork.IspName;
            row["BusinessName"] = geoNetwork.BusinessName is null ? DBNull.Value : geoNetwork.BusinessName;
            row["InteractionId"] = geoNetwork.InteractionId is null ? DBNull.Value : geoNetwork.InteractionId;
            dt.Rows.Add(row);
        }
    }
    private static void AddGoalsToDataTable(DataTable dt, IEnumerable<GoalModel>? goals)
    {
        if (goals is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));

        foreach (var goal in goals)
        {
            var row = dt.NewRow();
            row["CustomValues"] = goal.CustomValues is null ? DBNull.Value : goal.CustomValues;
            row["Data"] = goal.Data is null ? DBNull.Value : goal.Data;
            row["DataKey"] = goal.DataKey is null ? DBNull.Value : goal.DataKey;
            row["DefinitionId"] = goal.DefinitionId is null ? DBNull.Value : goal.DefinitionId;
            row["ItemId"] = goal.ItemId is null ? DBNull.Value : goal.ItemId;
            row["EngagementValue"] = goal.EngagementValue is null ? DBNull.Value : goal.EngagementValue;
            row["Id"] = goal.Id;
            row["ParentEventId"] = goal.ParentEventId is null ? DBNull.Value : goal.ParentEventId;
            row["Text"] = goal.Text is null ? DBNull.Value : goal.Text;
            row["Timestamp"] = goal.Timestamp is null ? DBNull.Value : goal.Timestamp;
            row["Duration"] = goal.Duration is null ? DBNull.Value : goal.Duration;
            row["InteractionId"] = goal.InteractionId is null ? DBNull.Value : goal.InteractionId;
            dt.Rows.Add(row);
        }
    }
    private static void AddOutcomesToDataTable(DataTable dt, IEnumerable<OutcomeModel>? outcomes)
    {
        if (outcomes is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));
        dt.Columns.Add("CurrencyCode", typeof(string));
        dt.Columns.Add("MonetaryValue", typeof(decimal));

        foreach (var outcome in outcomes)
        {
            var row = dt.NewRow();
            row["CustomValues"] = outcome.CustomValues is null ? DBNull.Value : outcome.CustomValues;
            row["Data"] = outcome.Data is null ? DBNull.Value : outcome.Data;
            row["DataKey"] = outcome.DataKey is null ? DBNull.Value : outcome.DataKey;
            row["DefinitionId"] = outcome.DefinitionId is null ? DBNull.Value : outcome.DefinitionId;
            row["ItemId"] = outcome.ItemId is null ? DBNull.Value : outcome.ItemId;
            row["EngagementValue"] = outcome.EngagementValue is null ? DBNull.Value : outcome.EngagementValue;
            row["Id"] = outcome.Id;
            row["ParentEventId"] = outcome.ParentEventId is null ? DBNull.Value : outcome.ParentEventId;
            row["Text"] = outcome.Text is null ? DBNull.Value : outcome.Text;
            row["Timestamp"] = outcome.Timestamp is null ? DBNull.Value : outcome.Timestamp;
            row["Duration"] = outcome.Duration is null ? DBNull.Value : outcome.Duration;
            row["InteractionId"] = outcome.InteractionId is null ? DBNull.Value : outcome.InteractionId;
            row["CurrencyCode"] = outcome.CurrencyCode is null ? DBNull.Value : outcome.CurrencyCode;
            row["MonetaryValue"] = outcome.MonetaryValue;
            dt.Rows.Add(row);
        }
    }
    private static void AddPageViewsToDataTable(DataTable dt, IEnumerable<PageViewModel>? pageViews)
    {
        if (pageViews is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));
        dt.Columns.Add("EventDefinitionId", typeof(Guid));
        dt.Columns.Add("ItemLanguage", typeof(string));
        dt.Columns.Add("ItemVersion", typeof(int));
        dt.Columns.Add("Url", typeof(string));

        foreach (var pageView in pageViews)
        {
            var row = dt.NewRow();
            row["CustomValues"] = pageView.CustomValues is null ? DBNull.Value : pageView.CustomValues;
            row["Data"] = pageView.Data is null ? DBNull.Value : pageView.Data;
            row["DataKey"] = pageView.DataKey is null ? DBNull.Value : pageView.DataKey;
            row["DefinitionId"] = pageView.DefinitionId is null ? DBNull.Value : pageView.DefinitionId;
            row["ItemId"] = pageView.ItemId is null ? DBNull.Value : pageView.ItemId;
            row["EngagementValue"] = pageView.EngagementValue is null ? DBNull.Value : pageView.EngagementValue;
            row["Id"] = pageView.Id;
            row["ParentEventId"] = pageView.ParentEventId is null ? DBNull.Value : pageView.ParentEventId;
            row["Text"] = pageView.Text is null ? DBNull.Value : pageView.Text;
            row["Timestamp"] = pageView.Timestamp is null ? DBNull.Value : pageView.Timestamp;
            row["Duration"] = pageView.Duration is null ? DBNull.Value : pageView.Duration;
            row["InteractionId"] = pageView.InteractionId is null ? DBNull.Value : pageView.InteractionId;
            row["EventDefinitionId"] = pageView.EventDefinitionId;
            row["ItemLanguage"] = pageView.ItemLanguage is null ? DBNull.Value : pageView.ItemLanguage;
            row["ItemVersion"] = pageView.ItemVersion;
            row["Url"] = pageView.Url is null ? DBNull.Value : pageView.Url;
            dt.Rows.Add(row);
        }
    }
    private static void AddSearchesToDataTable(DataTable dt, IEnumerable<SearchModel>? searches)
    {
        if (searches is null)
        {
            return;
        }
        dt.Columns.Add("CustomValues", typeof(string));
        dt.Columns.Add("Data", typeof(string));
        dt.Columns.Add("DataKey", typeof(string));
        dt.Columns.Add("DefinitionId", typeof(Guid));
        dt.Columns.Add("ItemId", typeof(Guid));
        dt.Columns.Add("EngagementValue", typeof(int));
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("ParentEventId", typeof(Guid));
        dt.Columns.Add("Text", typeof(string));
        dt.Columns.Add("Timestamp", typeof(DateTime));
        dt.Columns.Add("Duration", typeof(TimeSpan));
        dt.Columns.Add("InteractionId", typeof(Guid));
        dt.Columns.Add("EventDefinitionId", typeof(Guid));
        dt.Columns.Add("Keywords", typeof(string));

        foreach (var search in searches)
        {
            var row = dt.NewRow();
            row["CustomValues"] = search.CustomValues is null ? DBNull.Value : search.CustomValues;
            row["Data"] = search.Data is null ? DBNull.Value : search.Data;
            row["DataKey"] = search.DataKey is null ? DBNull.Value : search.DataKey;
            row["DefinitionId"] = search.DefinitionId is null ? DBNull.Value : search.DefinitionId;
            row["ItemId"] = search.ItemId is null ? DBNull.Value : search.ItemId;
            row["EngagementValue"] = search.EngagementValue is null ? DBNull.Value : search.EngagementValue;
            row["Id"] = search.Id;
            row["ParentEventId"] = search.ParentEventId is null ? DBNull.Value : search.ParentEventId;
            row["Text"] = search.Text is null ? DBNull.Value : search.Text;
            row["Timestamp"] = search.Timestamp is null ? DBNull.Value : search.Timestamp;
            row["Duration"] = search.Duration is null ? DBNull.Value : search.Duration;
            row["InteractionId"] = search.InteractionId is null ? DBNull.Value : search.InteractionId;
            row["EventDefinitionId"] = search.EventDefinitionId;
            row["Keywords"] = search.Keywords is null ? DBNull.Value : search.Keywords;
            dt.Rows.Add(row);
        }
    }
    private static void AddChannelsToDataTable(DataTable dt, IEnumerable<ChannelModel>? channels)
    {
        if (channels is null)
        {
            return;
        }
        dt.Columns.Add("Id", typeof(Guid));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Description", typeof(string));
        dt.Columns.Add("Name", typeof(string));
        dt.Columns.Add("UriCulture", typeof(string));
        dt.Columns.Add("UriPath", typeof(string));
        dt.Columns.Add("Uri", typeof(string));

        foreach (var channel in channels)
        {
            var row = dt.NewRow();
            row["Id"] = channel.Id;
            row["Code"] = channel.Code is null ? DBNull.Value : channel.Code;
            row["Description"] = channel.Description is null ? DBNull.Value : channel.Description;
            row["Name"] = channel.Name is null ? DBNull.Value : channel.Name;
            row["UriCulture"] = channel.UriCulture is null ? DBNull.Value : channel.UriCulture;
            row["UriPath"] = channel.UriPath is null ? DBNull.Value : channel.UriPath;
            row["Uri"] = channel.Uri is null ? DBNull.Value : channel.Uri;
            dt.Rows.Add(row);
        }
    }

    private static string CreateMergeStatementBasedOnEntity(Type type)
    {
        string sql = string.Empty;
        if (type == typeof(InteractionModel))
        {
            sql = @"MERGE Interactions AS target
USING #temp AS source
ON target.InteractionId = source.InteractionId
WHEN MATCHED THEN
    UPDATE SET
        target.StartDateTime = source.StartDateTime,
        target.EndDateTime = source.EndDateTime,
        target.LastModified = source.LastModified,
        target.CampaignId = source.CampaignId,
        target.ChannelId = source.ChannelId,
        target.EngagementValue = source.EngagementValue,
        target.Duration = source.Duration,
        target.UserAgent = source.UserAgent,
        target.Bounces = source.Bounces,
        target.Conversions = source.Conversions,
        target.Converted = source.Converted,
        target.TimeOnSite = source.TimeOnSite,
        target.PageViews = source.PageViews,
        target.OutcomeOccurrences = source.OutcomeOccurrences,
        target.MonetaryValue = source.MonetaryValue
WHEN NOT MATCHED THEN
    INSERT (InteractionId, StartDateTime, EndDateTime, LastModified, CampaignId, ChannelId,
            EngagementValue, Duration, UserAgent, Bounces, Conversions, Converted,
            TimeOnSite, PageViews, OutcomeOccurrences, MonetaryValue)
    VALUES (source.InteractionId, source.StartDateTime, source.EndDateTime, source.LastModified,
            source.CampaignId, source.ChannelId, source.EngagementValue, source.Duration,
            source.UserAgent, source.Bounces, source.Conversions, source.Converted,
            source.TimeOnSite, source.PageViews, source.OutcomeOccurrences, source.MonetaryValue);";

        }
        else if (type == typeof(EventDefinitionModel))
        {
            sql = @"MERGE EventDefinitions AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Alias = source.Alias,
        target.CreatedDate = source.CreatedDate,
        target.LastModifiedDate = source.LastModifiedDate,
        target.CreatedBy = source.CreatedBy,
        target.LastModifiedBy = source.LastModifiedBy,
        target.Culture = source.Culture,
        target.Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (Id, Alias, CreatedDate, LastModifiedDate, CreatedBy, LastModifiedBy, Culture, Description)
    VALUES (source.Id, source.Alias, source.CreatedDate, source.LastModifiedDate,
            source.CreatedBy, source.LastModifiedBy, source.Culture, source.Description);";
        }
        else if (type == typeof(CampaignDefinitionModel))
        {
            sql = @"MERGE CampaignDefinitions AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Alias = source.Alias,
        target.CreatedDate = source.CreatedDate,
        target.LastModifiedDate = source.LastModifiedDate,
        target.CreatedBy = source.CreatedBy,
        target.LastModifiedBy = source.LastModifiedBy,
        target.Culture = source.Culture,
        target.Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (Id, Alias, CreatedDate, LastModifiedDate, CreatedBy, LastModifiedBy, Culture, Description)
    VALUES (source.Id, source.Alias, source.CreatedDate, source.LastModifiedDate,
            source.CreatedBy, source.LastModifiedBy, source.Culture, source.Description);";
        }
        else if (type == typeof(GoalDefinitionModel))
        {
            sql = @"MERGE GoalDefinitions AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Alias = source.Alias,
        target.CreatedDate = source.CreatedDate,
        target.LastModifiedDate = source.LastModifiedDate,
        target.CreatedBy = source.CreatedBy,
        target.LastModifiedBy = source.LastModifiedBy,
        target.Culture = source.Culture,
        target.Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (Id, Alias, CreatedDate, LastModifiedDate, CreatedBy, LastModifiedBy, Culture, Description)
    VALUES (source.Id, source.Alias, source.CreatedDate, source.LastModifiedDate,
            source.CreatedBy, source.LastModifiedBy, source.Culture, source.Description);";
        }
        else if (type == typeof(OutcomeDefinitionModel))
        {
            sql = @"MERGE OutcomeDefinitions AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Alias = source.Alias,
        target.CreatedDate = source.CreatedDate,
        target.LastModifiedDate = source.LastModifiedDate,
        target.CreatedBy = source.CreatedBy,
        target.LastModifiedBy = source.LastModifiedBy,
        target.Culture = source.Culture,
        target.Description = source.Description
WHEN NOT MATCHED THEN
    INSERT (Id, Alias, CreatedDate, LastModifiedDate, CreatedBy, LastModifiedBy, Culture, Description)
    VALUES (source.Id, source.Alias, source.CreatedDate, source.LastModifiedDate,
            source.CreatedBy, source.LastModifiedBy, source.Culture, source.Description);";
        }
        else if (type == typeof(CampaignModel))
        {
            sql = @"MERGE Campaigns AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration,
        target.CampaignDefinitionId = source.CampaignDefinitionId
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration, CampaignDefinitionId)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data, source.DataKey,
            source.DefinitionId, source.ItemId, source.EngagementValue, source.ParentEventId,
            source.Text, source.Timestamp, source.Duration, source.CampaignDefinitionId);";
        }
        else if (type == typeof(DeviceModel))
        {
            sql = @"MERGE Devices AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.BrowserVersion = source.BrowserVersion,
        target.BrowserMajorName = source.BrowserMajorName,
        target.BrowserMinorName = source.BrowserMinorName,
        target.DeviceCategory = source.DeviceCategory,
        target.ScreenSize = source.ScreenSize,
        target.OperatingSystem = source.OperatingSystem,
        target.OperatingSystemVersion = source.OperatingSystemVersion,
        target.Language = source.Language,
        target.CanSupportTouchScreen = source.CanSupportTouchScreen,
        target.DeviceVendor = source.DeviceVendor,
        target.DeviceVendorHardwareModel = source.DeviceVendorHardwareModel,
        target.InteractionId = source.InteractionId
WHEN NOT MATCHED THEN
    INSERT (BrowserVersion, BrowserMajorName, BrowserMinorName, DeviceCategory,
            ScreenSize, OperatingSystem, OperatingSystemVersion, Language,
            CanSupportTouchScreen, DeviceVendor, DeviceVendorHardwareModel, InteractionId)
    VALUES (source.BrowserVersion, source.BrowserMajorName, source.BrowserMinorName,
            source.DeviceCategory, source.ScreenSize, source.OperatingSystem,
            source.OperatingSystemVersion, source.Language, source.CanSupportTouchScreen,
            source.DeviceVendor, source.DeviceVendorHardwareModel, source.InteractionId);";
        }
        else if (type == typeof(DownloadModel))
        {
            sql = @"MERGE Downloads AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration,
        target.EventDefinitionId = source.EventDefinitionId
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration, EventDefinitionId)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data, source.DataKey,
            source.DefinitionId, source.ItemId, source.EngagementValue, source.ParentEventId,
            source.Text, source.Timestamp, source.Duration, source.EventDefinitionId);";
        }
        else if (type == typeof(GeoNetworkModel))
        {
            sql = @"MERGE GeoNetworks AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Area = source.Area,
        target.Country = source.Country,
        target.Region = source.Region,
        target.Metro = source.Metro,
        target.City = source.City,
        target.Latitude = source.Latitude,
        target.Longitude = source.Longitude,
        target.LocationId = source.LocationId,
        target.PostalCode = source.PostalCode,
        target.IspName = source.IspName,
        target.BusinessName = source.BusinessName,
        target.InteractionId = source.InteractionId
WHEN NOT MATCHED THEN
    INSERT (Area, Country, Region, Metro, City, Latitude, Longitude,
            LocationId, PostalCode, IspName, BusinessName, InteractionId)
    VALUES (source.Area, source.Country, source.Region, source.Metro,
            source.City, source.Latitude, source.Longitude, source.LocationId,
            source.PostalCode, source.IspName, source.BusinessName, source.InteractionId);";
        }
        else if (type == typeof(GoalModel))
        {
            sql = @"MERGE Goals AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data, source.DataKey,
            source.DefinitionId, source.ItemId, source.EngagementValue, source.ParentEventId,
            source.Text, source.Timestamp, source.Duration);";
        }
        else if (type == typeof(OutcomeModel))
        {
            sql = @"MERGE Outcomes AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration,
        target.CurrencyCode = source.CurrencyCode,
        target.MonetaryValue = source.MonetaryValue
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration,
            CurrencyCode, MonetaryValue)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data,
            source.DataKey, source.DefinitionId, source.ItemId, source.EngagementValue,
            source.ParentEventId, source.Text, source.Timestamp, source.Duration,
            source.CurrencyCode, source.MonetaryValue);
";
        }
        else if (type == typeof(PageViewModel))
        {
            sql = @"MERGE PageViews AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration,
        target.ItemLanguage = source.ItemLanguage,
        target.ItemVersion = source.ItemVersion,
        target.Url = source.Url,
        target.EventDefinitionId = source.EventDefinitionId
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration,
            ItemLanguage, ItemVersion, Url, EventDefinitionId)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data,
            source.DataKey, source.DefinitionId, source.ItemId, source.EngagementValue,
            source.ParentEventId, source.Text, source.Timestamp, source.Duration,
            source.ItemLanguage, source.ItemVersion, source.Url, source.EventDefinitionId);
";
        }
        else if (type == typeof(SearchModel))
        {
            sql = @"MERGE Searches AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.InteractionId = source.InteractionId,
        target.CustomValues = source.CustomValues,
        target.Data = source.Data,
        target.DataKey = source.DataKey,
        target.DefinitionId = source.DefinitionId,
        target.ItemId = source.ItemId,
        target.EngagementValue = source.EngagementValue,
        target.ParentEventId = source.ParentEventId,
        target.Text = source.Text,
        target.Timestamp = source.Timestamp,
        target.Duration = source.Duration,
        target.Keywords = source.Keywords,
        target.EventDefinitionId = source.EventDefinitionId
WHEN NOT MATCHED THEN
    INSERT (Id, InteractionId, CustomValues, Data, DataKey, DefinitionId,
            ItemId, EngagementValue, ParentEventId, Text, Timestamp, Duration,
            Keywords, EventDefinitionId)
    VALUES (source.Id, source.InteractionId, source.CustomValues, source.Data,
            source.DataKey, source.DefinitionId, source.ItemId, source.EngagementValue,
            source.ParentEventId, source.Text, source.Timestamp, source.Duration,
            source.Keywords, source.EventDefinitionId);
";
        }
        else if (type == typeof(ChannelModel))
        {
            sql = @"MERGE Channels AS target
USING #temp AS source
ON target.Id = source.Id
WHEN MATCHED THEN
    UPDATE SET
        target.Code = source.Code,
        target.Description = source.Description,
        target.Name = source.Name,
        target.UriCulture = source.UriCulture,
        target.UriPath = source.UriPath,
        target.Uri = source.Uri
WHEN NOT MATCHED THEN
    INSERT (Id, Code, Description, Name, UriCulture, UriPath, Uri)
    VALUES (source.Id, source.Code, source.Description, source.Name,
            source.UriCulture, source.UriPath, source.Uri);
";
        }
        return sql;
    }
}

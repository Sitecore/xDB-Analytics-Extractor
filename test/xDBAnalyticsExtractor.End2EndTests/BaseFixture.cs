using System.Data.SqlClient;
using Dapper;
using xDBAnalyticsExtractor.ExportSchema;
using Microsoft.Data.Sqlite;
using xDBAnalyticsExtractor.End2EndTests.Configuration;
using xDBAnalyticsExtractor.End2EndTests.Helpers;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;
using Sitecore.Common;

namespace xDBAnalyticsExtractor.End2EndTests;

public class BaseFixture
{
    protected Task<XConnectClient> XClient = null!;
    protected CsvFileHelper _csvFileHelper;
    protected ETLDatabaseHelper _etlDatabaseHelper;
    protected string _eTLfilesFolder;
    protected string _eTLDatabaseConnectionString;
    protected ExportRepository _exportRepository;
    private bool _isXConnectInitialized;
    private DbConnectionStringSettings _dbConnectionStringSettings;
    private TestInitializer _initialize;
    private InstanceInfoSettings _instanceInfoSettings;
    
    
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _initialize = new TestInitializer();
        _csvFileHelper = new CsvFileHelper();
        _etlDatabaseHelper = new ETLDatabaseHelper();
        _exportRepository = new ExportRepository();
        
        _dbConnectionStringSettings = _initialize.DbConnectionStringSettings;
        XClient = _initialize.InitializeXClient();
        _isXConnectInitialized = _initialize.IsInitialized;
        _eTLDatabaseConnectionString = _initialize.ETLDatabaseConnectionString;
        _eTLfilesFolder = _initialize.ETLfilesFolder;
        
        // DateTime types have tiny difference between xconnect client interaction submittion and collection DB timestamp.
        // In order to workaround this issue, we accept 10miliseconds tolerance.
        AssertionOptions.AssertEquivalencyUsing(options =>
            options.Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(10))).WhenTypeIs<DateTime>());

        //Clean Sitecore instance's XDB collection database from any interactions
        ClearXdbCollectionShards();

        //Clean ETLDatabase from any previous exports
        ClearETLDatabase();
        
        //Delete any exported .csv files in export folder
        CleanUpCsvFilesFolder();
        
        //Clear ETL Worker Sqlite DB
        ClearEtlWorkerDb();
        
        //Insert record with LastExported set to -12h
        SetLastExportedDateTimeUtcNowMinusHours(12);
    }

    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
    }    
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        if (_isXConnectInitialized)
        {
            XClient.Dispose();
        }
    }

    protected void ClearXdbCollectionShards()
    {
        //Shard0
        ClearXdbCollectionTables(
            $@"Data Source={_dbConnectionStringSettings.Server};Initial Catalog={_dbConnectionStringSettings.Database}.Shard0;User ID={_dbConnectionStringSettings.DbUser};Password={_dbConnectionStringSettings.DbPassword};");

        //Shard1
        ClearXdbCollectionTables(
            $@"Data Source={_dbConnectionStringSettings.Server};Initial Catalog={_dbConnectionStringSettings.Database}.Shard1;User ID={_dbConnectionStringSettings.DbUser};Password={_dbConnectionStringSettings.DbPassword};");
    }

    private void ClearXdbCollectionTables(string connectionString)
    {
        List<string> tableNames = new List<string>
        {
            "InteractionFacets",
            "InteractionFacets_Staging",
            "Interactions",
            "Interactions_Staging",
            "UnlockContactIdentifiersIndex_Staging",
            "CheckContacts_Staging",
            "ContactFacets",
            "ContactFacets_Staging",
            "ContactIdentifiers",
            "ContactIdentifiers_Staging",
            "ContactIdentifiersIndex",
            "ContactIdentifiersIndex_Staging",
            "Contacts",
            "Contacts_Staging",
            "DeviceProfileFacets",
            "DeviceProfileFacets_Staging",
            "DeviceProfiles",
            "DeviceProfiles_Staging",
            "GetContactIdsByIdentifiers_Staging",
            "GetContactsByIdentifiers_Staging"
        };

        using SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        foreach (string tableName in tableNames)
        {
            using (SqlCommand command = new SqlCommand($"DELETE FROM xdb_collection.{tableName}", connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"Deleted records from {tableName} table.");
            }
        }
    }
    
    protected void ClearETLDatabase()
    {
        List<string> tableNames = new List<string>
        {
            "Searches",
            "PageViews",
            "Outcomes",
            "Goals",
            "GeoNetworks",
            "Downloads",
            "Devices",
            "Campaigns",
            "OutcomeDefinitions",
            "GoalDefinitions",
            "CampaignDefinitions",
            "EventDefinitions",
            "Interactions"
        };

        using SqlConnection connection = new SqlConnection(_eTLDatabaseConnectionString);
        connection.Open();

        foreach (string tableName in tableNames)
        {
            using (SqlCommand command = new SqlCommand($"DELETE FROM dbo.{tableName}", connection))
            {
                command.ExecuteNonQuery();
                Console.WriteLine($"Deleted records from {tableName} table.");
            }
        }
    }

    protected void CleanUpCsvFilesFolder()
    {
        // Search for CSV files in the ETL export files folder
        string[] csvFiles = Directory.GetFiles(_eTLfilesFolder, "*.csv");

        // Delete each CSV file found
        foreach (string csvFile in csvFiles)
        {
            File.Delete(csvFile);
        }
    }
    
    protected void ClearEtlWorkerDb()
    {
        string sqliteDbPath = @"c:\ETLWorker\ETLWorker.db";
        string _connectionString = $"Data Source={sqliteDbPath};";
        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();
        string query = "DELETE FROM Exports;";
        connection.Execute(query);
        Console.WriteLine($"Deleted records from ETLWorker.db Exports table.");
        connection.Close();
    }
    
    protected async Task RunExtractorWorkerForCurrentData()
    {
        var testWorker = new TestWorker(new[] {"-csv", "-sqlServer", "-current"});
        await testWorker.TestWorkerService();
    }
    
    protected async Task RunExtractorWorkerForHistoricalData()
    {
        var testWorker = new TestWorker(new[] {"-csv", "-sqlServer", "-historical"});
        await testWorker.TestWorkerService();
    }

    protected void SetLastExportedDateTimeUtcNowMinusHours(double hours)
    {
        var lastExported = DateTime.UtcNow.AddHours(-hours);
        lastExported = lastExported.SpecifyKind(DateTimeKind.Utc);
        _exportRepository.InsertExport(new Export(lastExported));
    }
    
    protected Interaction SubmitInteraction(double hoursBefore)
    {
        InteractionBuilder interaction;
        var timestamp = DateTime.UtcNow.AddHours(hoursBefore);
        using (XClient)
        {
            var xConnectRootEntity = new XConnectEntityRoot(XClient);

            var contact = new ContactBuilder().AddPersonalInformation().AddContactIdentifiers();
            interaction = new InteractionBuilder(contact.Build(), InteractionInitiator.Brand,
                    InteractionBuilder.ChannelSelection.Telemarketing, "some Agent")
                .AddCampaignId(Guid.Parse("0C46AA2E-40DE-4DB2-803F-0D90B6935569"))
                .AddVenueId(Guid.NewGuid());
            var userAgentInfo = new UserAgentInfoBuilder(interaction.Build())
                .AddCanSupportTouchScreen(true)
                .AddDeviceType("android")
                .AddDeviceVendor("Google")
                .AddDeviceVendorHardwareModel("Pixel 20");
            var ipInfo = new IpInfoBuilder(interaction.Build(), "124.48.1.1")
                .AddCity("CityField").AddCountry("CountryField").AddDns("DnsField")
                .AddIsp("IspField").AddLatitude(74.2555454).AddLongitude(12.2555454)
                .AddRegion("RegionField").AddUrl("urlField").AddAreaCode("AreadCodeField")
                .AddBusinessName("BusinessNameField").AddLocationId(Guid.NewGuid())
                .AddMetroCode("MetroCodeField").AddPostalCode("PostalCodeField");
            var deviceProfile = new DeviceProfileBuilder(interaction.Build(), Guid.NewGuid())
                .AddLastKnownContact(contact.Build());
            var webVisit = new WebVisitBuilder(interaction.Build())
                .AddLanguage("en").AddReferrer("Facebook").AddBrowserData(new BrowserDataBuilder()
                    .AddBrowserVersion("Browser Version").AddBrowserMajorName("Major Version")
                    .AddBrowserMinorName("Minor Version").Build())
                .AddScreenData(new ScreenDataBuilder().AddScreenHeight(1000).AddScreenWidth(800).Build())
                .AddSearchKeywords("mitsos").AddSiteName("https://www.mitso-takis.com")
                .AddIsSelfReferrer(false)
                .AddOperatingSystemData(new OperatingSystemDataBuilder().AddName("Win 10").AddMajorVersion("102")
                    .AddMinorVersion("5").Build());
            var localeInfo = new LocaleInfoBuilder(interaction.Build())
                .AddGeoCoordinate(new GeoCoordinateBuilder(120.2555454, 5.2555454).Build())
                .AddTimeZoneOffset(TimeSpan.FromHours(+5));
            var pageViewEvent =
                new PageViewEventBuilder(interaction.Build(), timestamp, Guid.NewGuid(), 1, "en")
                    .AddData("dataString")
                    .AddDuration(TimeSpan.FromHours(1))
                    .AddDataKey("dataKeyString")
                    .AddText("textString")
                    .AddEngagementValue(20)
                    .AddUrl("https://www.mitso-takis.com")
                    .AddSitecoreRenderingDevice(Guid.NewGuid(), "GTR-9210");
            var searchEvent = new SearchEventBuilder(interaction.Build(), timestamp)
                .AddKeywords("mitsos")
                .AddData("some data")
                .AddDataKey("secretKey Data")
                .AddItemId(Guid.NewGuid())
                .AddEngagementValue(3)
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddText("Search Text")
                .AddDuration(TimeSpan.FromSeconds(3));
            var outcomeEvent = new OutcomeEventBuilder(interaction.Build(),
                    OutcomeEventBuilder.OutcomeDefinitionSelection.SalesLead, timestamp, "some outcome",
                    100.30m)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString Outcome")
                .AddText("textString Outcome")
                .AddEngagementValue(14)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var goalEvent = new GoalEventBuilder(interaction.Build(),
                    GoalEventBuilder.GoalDefinitionSelection.BrochuresRequest, timestamp)
                .AddData("dataString Goal")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString Goal")
                .AddText("textString Goal")
                .AddEngagementValue(27)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var downloadEvent = new DownloadEventBuilder(interaction.Build(), timestamp, Guid.NewGuid())
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var customEvent = new CustomEventBuilder(interaction.Build(), timestamp, Guid.NewGuid())
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddId(Guid.NewGuid())
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var customEventWithCustomValues =
                new CustomEventBuilder(interaction.Build(), timestamp, Guid.NewGuid())
                    .AddCustomValues("SomeData", "SomeDataKeys", Guid.NewGuid(), 1, Guid.NewGuid(), Guid.NewGuid(),
                        "custom",
                        TimeSpan.FromHours(1)).AddParentEventId(pageViewEvent.Build().Id);
            var profileScoresEvent =
                new ProfileScoresEventBuilder(interaction.Build(), contact.Build(), timestamp)
                    .AddProfileScores(ProfileScoresEventBuilder.ProfileDefinitionSelection.Persona, Guid.NewGuid(), 2,
                        7, true);
            var changeProfileScoresEvent =
                new ChangeProfileScoresEventBuilder(interaction.Build(), contact.Build(), timestamp)
                    .AddProfileScore(100)
                    .AddMatchedPatternId(Guid.NewGuid())
                    .AddProfileDefinitionId(ChangeProfileScoresEventBuilder.ProfileDefinitionSelection.Focus)
                    .AddProfileScoreCount(100)
                    .AddProfileScoreDelta(ChangeProfileScoresEventBuilder.ProfileDefinitionSelection.Focus, 10, true);
            var campaignEvent = new CampaignEventBuilder(interaction.Build(), timestamp,
                    CampaignEventBuilder.CampaignDefinitionSelection.BrandPromotion)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);

            xConnectRootEntity.Add(contact);
            xConnectRootEntity.Add(userAgentInfo);
            xConnectRootEntity.Add(ipInfo);
            xConnectRootEntity.Add(deviceProfile);
            xConnectRootEntity.Add(webVisit);
            xConnectRootEntity.Add(localeInfo);
            xConnectRootEntity.Add(searchEvent);
            xConnectRootEntity.Add(pageViewEvent);
            xConnectRootEntity.Add(outcomeEvent);
            xConnectRootEntity.Add(goalEvent);
            xConnectRootEntity.Add(downloadEvent);
            xConnectRootEntity.Add(customEvent);
            xConnectRootEntity.Add(customEventWithCustomValues);
            xConnectRootEntity.Add(profileScoresEvent);
            xConnectRootEntity.Add(changeProfileScoresEvent);
            xConnectRootEntity.Add(campaignEvent);
            xConnectRootEntity.Add(interaction); // important to place the interaction last in the list
            xConnectRootEntity.BuildInteraction();
        }

        return interaction.Build();
    }
}
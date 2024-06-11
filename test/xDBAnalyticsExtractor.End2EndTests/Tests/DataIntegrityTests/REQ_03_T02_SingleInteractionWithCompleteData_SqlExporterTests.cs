using xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;
using xDBAnalyticsExtractor.End2EndTests.Helpers;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

namespace xDBAnalyticsExtractor.End2EndTests.Tests.DataIntegrityTests;

public class REQ_03_T02_SingleInteractionWithCompleteData_SqlExporterTests : BaseFixture
{
    private List<Interaction> _interactionList;
    private TestWorker _testWorker;
    
    [OneTimeSetUp]
    public new async Task OneTimeSetUp()
    {
        _testWorker = new TestWorker(new[] {"-sqlServer", "-current"});
        _interactionList = new List<Interaction>();
        Interaction interactionResult;
        
        using (XClient)
        {
            var xConnectRootEntity = new XConnectEntityRoot(XClient);

            var contact = new ContactBuilder().AddPersonalInformation().AddContactIdentifiers();
            var interaction = new InteractionBuilder(contact.Build(), InteractionInitiator.Brand,
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
                .AddLanguage("en").AddReferrer("Facebook").AddBrowserData(new BrowserDataBuilder().AddBrowserVersion("Browser Version").AddBrowserMajorName("Major Version").AddBrowserMinorName("Minor Version").Build())
                .AddScreenData(new ScreenDataBuilder().AddScreenHeight(1000).AddScreenWidth(800).Build())
                .AddSearchKeywords("mitsos").AddSiteName("https://www.mitso-takis.com")
                .AddIsSelfReferrer(false)
                .AddOperatingSystemData(new OperatingSystemDataBuilder().AddName("Win 10").AddMajorVersion("102")
                    .AddMinorVersion("5").Build());
            var localeInfo = new LocaleInfoBuilder(interaction.Build())
                .AddGeoCoordinate(new GeoCoordinateBuilder(120.2555454, 5.2555454).Build())
                .AddTimeZoneOffset(TimeSpan.FromHours(+5));
            var pageViewEvent =
                new PageViewEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7), Guid.NewGuid(), 1, "en")
                    .AddData("dataString")
                    .AddDuration(TimeSpan.FromHours(1))
                    .AddDataKey("dataKeyString")
                    .AddText("textString")
                    .AddEngagementValue(20)
                    .AddUrl("https://www.mitso-takis.com")
                    .AddSitecoreRenderingDevice(Guid.NewGuid(), "GTR-9210")
                    .AddCustomValues();
            var searchEvent = new SearchEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7))
                .AddKeywords("mitsos")
                .AddData("some data")
                .AddDataKey("secretKey Data")
                .AddItemId(Guid.NewGuid())
                .AddEngagementValue(3)
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddText("Search Text")
                .AddDuration(TimeSpan.FromSeconds(3))
                .AddCustomValues();
            var outcomeEvent = new OutcomeEventBuilder(interaction.Build(),
                    OutcomeEventBuilder.OutcomeDefinitionSelection.SalesLead, DateTime.UtcNow.AddHours(-7), "EUR",
                    100.30m)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString Outcome")
                .AddText("textString Outcome")
                .AddEngagementValue(14)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            var goalEvent = new GoalEventBuilder(interaction.Build(),
                    GoalEventBuilder.GoalDefinitionSelection.BrochuresRequest, DateTime.UtcNow.AddHours(-7))
                .AddData("dataString Goal")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString Goal")
                .AddText("textString Goal")
                .AddEngagementValue(27)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            var downloadEvent =
                new DownloadEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7), Guid.NewGuid())
                    .AddData("dataString")
                    .AddDuration(TimeSpan.FromHours(1))
                    .AddDataKey("dataKeyString")
                    .AddText("textString")
                    .AddEngagementValue(20)
                    .AddItemId(Guid.NewGuid())
                    .AddId(Guid.NewGuid())
                    .AddParentEventId(pageViewEvent.Build().Id)
                    .AddCustomValues();
            var customEvent = new CustomEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7), Guid.NewGuid())
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddId(Guid.NewGuid())
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var customEventWithCustomValues =
                new CustomEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7), Guid.NewGuid())
                    .AddCustomValues("SomeData", "SomeDataKeys", Guid.NewGuid(), 1, Guid.NewGuid(), Guid.NewGuid(),
                        "custom",
                        TimeSpan.FromHours(1)).AddParentEventId(pageViewEvent.Build().Id);
            var profileScoresEvent =
                new ProfileScoresEventBuilder(interaction.Build(), contact.Build(), DateTime.UtcNow.AddHours(-7))
                    .AddProfileScores(ProfileScoresEventBuilder.ProfileDefinitionSelection.Persona, Guid.NewGuid(), 2,
                        7, true);
            var changeProfileScoresEvent =
                new ChangeProfileScoresEventBuilder(interaction.Build(), contact.Build(), DateTime.UtcNow.AddHours(-7))
                    .AddProfileScore(100)
                    .AddMatchedPatternId(Guid.NewGuid())
                    .AddProfileDefinitionId(ChangeProfileScoresEventBuilder.ProfileDefinitionSelection.Focus)
                    .AddProfileScoreCount(100)
                    .AddProfileScoreDelta(ChangeProfileScoresEventBuilder.ProfileDefinitionSelection.Focus, 10, true);
            var campaignEvent = new CampaignEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7),
                    CampaignEventBuilder.CampaignDefinitionSelection.BrandPromotion)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();

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

            interactionResult = interaction.Build();
        }

        _interactionList.Add(interactionResult);
        
        // Run the exporter
        await _testWorker.TestWorkerService();
    }
    
    [TearDown]
    public void TearDown()
    {
    }    
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _interactionList!.Clear();
    }
    
    [Test]
    public async Task InteractionWithCompleteData_SqlSearchesTableData()
    {
        // Arrange
        var mapper = new SqlSearchToDataTableMapper();
        
        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Searches");
        
        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }
    
    [Test]
    public async Task InteractionWithCompleteData_SqlPageViewsTableData()
    {
        // Arrange
        var mapper = new SqlPageViewToDataTableMapper();
        
        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "PageViews");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }    
    
    [Test]
    public async Task InteractionWithCompleteData_SqlOutcomesTableData()
    {
        // Arrange
        var mapper = new SqlOutcomeToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Outcomes");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }   
    
    [Test]
    public async Task InteractionWithCompleteData_SqlInteractionsTableData()
    {
        // Arrange
        var mapper = new SqlInteractionToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Interactions");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }
    
    [Test]
    public async Task InteractionWithCompleteData_SqlGoalsTableData()
    {
        // Arrange
        var mapper = new SqlGoalToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Goals");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }   
    
    [Test]
    public async Task InteractionWithCompleteData_SqlGeoNetworksTableData()
    {
        // Arrange
        var mapper = new SqlGeoNetworkToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "GeoNetworks");
        // remove Id column from assertion 
        dataTableResult.Columns.Remove("Id");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }   
    
    [Test]
    public async Task InteractionWithCompleteData_SqlDownloadsTableData()
    {
        // Arrange
        var mapper = new SqlDownloadToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Downloads");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }    
    
    [Test]
    public async Task InteractionWithCompleteData_SqlDevicesTableData()
    {
        // Arrange
        var mapper = new SqlDeviceToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Devices");
        // remove Id column from assertion 
        dataTableResult.Columns.Remove("Id");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }    
    
    [Test]
    public async Task InteractionWithCompleteData_SqlCampaignsTableData()
    {
        // Arrange
        var mapper = new SqlCampaignToDataTableMapper();

        // Act
        var dataTableResult = _etlDatabaseHelper.LoadDataTableFromETLDatabase(_eTLDatabaseConnectionString, "Campaigns");

        // Assert
        dataTableResult.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(_interactionList!));
    }
}
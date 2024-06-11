using ApprovalTests;
using ApprovalTests.Reporters;
using xDBAnalyticsExtractor.End2EndTests.Helpers;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

namespace xDBAnalyticsExtractor.End2EndTests.Tests;

[TestFixture]
[UseReporter(typeof(DiffReporter))]
public class SQLExporterApprovalTests : BaseFixture
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
                .AddCampaignId(Guid.Parse("D74879A8-99CD-0000-0000-06CFAAB89619"))
                .AddVenueId(Guid.NewGuid());
            var userAgentInfo = new UserAgentInfoBuilder(interaction.Build())
                .AddDeviceType("android")
                .AddDeviceVendor("Google")
                .AddCanSupportTouchScreen(true)
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
                .AddLanguage("en").AddReferrer("Facebook").AddBrowserData(new BrowserDataBuilder().Build())
                .AddScreenData(new ScreenDataBuilder().AddScreenHeight(1000).AddScreenWidth(800).Build())
                .AddSearchKeywords("mitso takis").AddSiteName("https://www.mitso-takis.com")
                .AddIsSelfReferrer(false)
                .AddOperatingSystemData(new OperatingSystemDataBuilder().AddName("Win 10").AddMajorVersion("102").AddMinorVersion("5").Build());
            var localeInfo = new LocaleInfoBuilder(interaction.Build())
                .AddGeoCoordinate(new GeoCoordinateBuilder(120.2555454, 5.2555454).Build())
                .AddTimeZoneOffset(TimeSpan.FromHours(+5));
            var pageViewEvent = new PageViewEventBuilder(interaction.Build(), DateTime.Now.AddHours(-1),
                    new Guid("00000002-0001-000c-0001-000000000006"), 1, "en")
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddUrl("https://www.mitso-takis.com")
                .AddSitecoreRenderingDevice(Guid.NewGuid(), "GTR-9210")
                .AddCustomValues();
            var outcomeEvent = new OutcomeEventBuilder(interaction.Build(),
                    OutcomeEventBuilder.OutcomeDefinitionSelection.SalesLead, DateTime.Now.AddHours(-1), "some outcome",
                    100.30m)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(new Guid("00000002-0001-000c-0001-000000000007"))
                .AddId(new Guid("00000002-0001-000c-0001-000000000008"))
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            var goalEvent = new GoalEventBuilder(interaction.Build(),
                    GoalEventBuilder.GoalDefinitionSelection.BrochuresRequest, DateTime.Now.AddHours(-1))
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(40)
                .AddItemId(new Guid("00000002-0001-000c-0001-000000000009"))
                .AddId(new Guid("00000002-0001-000c-0001-00000000000a"))
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            var downloadEvent = new DownloadEventBuilder(interaction.Build(), DateTime.Now.AddHours(-1),
                    new Guid("00000002-0001-000c-0001-00000000000b"))
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(60)
                .AddItemId(new Guid("00000002-0001-000c-0001-00000000000c"))
                .AddId(new Guid("00000002-0001-000c-0001-00000000000d"))
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            var searchEvent = new SearchEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-1))
                .AddKeywords("mitsos")
                .AddData("some data")
                .AddDataKey("secretKey Data")
                .AddItemId(new Guid("00000002-0001-000c-0001-00000000001c"))
                .AddEngagementValue(3)
                .AddId(new Guid("00000002-0001-000c-0001-00000000002c"))
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddText("Search Text")
                .AddDuration(TimeSpan.FromSeconds(3))
                .AddCustomValues();
            var campaignEvent = new CampaignEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-1),
                    CampaignEventBuilder.CampaignDefinitionSelection.BrandPromotion)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(new Guid("00000002-0001-000c-0001-00000000003c"))
                .AddId(new Guid("00000002-0001-000c-0001-00000000004c"))
                .AddParentEventId(pageViewEvent.Build().Id)
                .AddCustomValues();
            
            xConnectRootEntity.Add(contact);
            xConnectRootEntity.Add(userAgentInfo);
            xConnectRootEntity.Add(ipInfo);
            xConnectRootEntity.Add(deviceProfile);
            xConnectRootEntity.Add(webVisit);
            xConnectRootEntity.Add(localeInfo);
            xConnectRootEntity.Add(pageViewEvent);
            xConnectRootEntity.Add(outcomeEvent);
            xConnectRootEntity.Add(goalEvent);
            xConnectRootEntity.Add(downloadEvent);
            xConnectRootEntity.Add(searchEvent);
            xConnectRootEntity.Add(campaignEvent);
            xConnectRootEntity.Add(interaction); // important to place the interaction last in the list
            xConnectRootEntity.BuildInteraction();
            
            interactionResult = interaction.Build();
        }
        
        _interactionList.Add(interactionResult);
        
        // Run the exporter
        await _testWorker.TestWorkerService();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _interactionList!.Clear();
    }
    
    [TearDown]
    public async Task TearDown()
    {
    }
    
    [Test]
    public async Task SQLApprovalTest_Tables()
    {
        //Act
        string[] tableNames = _etlDatabaseHelper.GetTableNames(_eTLDatabaseConnectionString); 
        
        //Assert
        Approvals.VerifyAll("Table Names", tableNames, "Table");

    }
    
    [Test]
    public async Task SQLApprovalTest_CampaignDefinitionsData()
    {
        //Arrange
        string[] dynamicData = null;
        
        //Act
        var campaignDefinitionsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "CampaignDefinitions", dynamicData);
        
        //Assert
        Approvals.VerifyJson(campaignDefinitionsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_EventDefinitionsData()
    {
        //Arrange
        string[] dynamicData = null;
        
        //Act
        var eventDefinitionsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "EventDefinitions", dynamicData);
        
        //Assert
        Approvals.VerifyJson(eventDefinitionsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_GoalDefinitionsData()
    {
        //Arrange
        string[] dynamicData = null;
        
        //Act
        var goalDefinitionsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "GoalDefinitions", dynamicData);
        
        //Assert
        Approvals.VerifyJson(goalDefinitionsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_OutcomeDefinitionsData()
    {
        //Arrange
        string[] dynamicData = null;
        
        //Act
        var outcomeDefinitionsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "OutcomeDefinitions", dynamicData);
        
        //Assert
        Approvals.VerifyJson(outcomeDefinitionsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_PageViewEventData()
    {
        //Arrange
        var dynamicData = new[] { "Id", "InteractionId", "Timestamp" };
        
        //Act
        var pageViewsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "pageViews", dynamicData);

        //Approve
        Approvals.VerifyJson(pageViewsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_InteractionData()
    {
        //Arrange
        var dynamicData = new[] { "InteractionId", "StartDateTime", "EndDateTime", "LastModified", "Duration" };
        
        //Act
        var interactionsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "interactions", dynamicData);

        //Approve
        Approvals.VerifyJson(interactionsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_OutcomesData()
    {
        //Arrange
        var dynamicData = new[] { "ParentEventId", "InteractionId", "Timestamp" };
        
        //Act
        var outcomesTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "outcomes", dynamicData);

        //Approve
        Approvals.VerifyJson(outcomesTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_GoalsData()
    {
        //Arrange
        var dynamicData = new[] { "ParentEventId", "InteractionId", "Timestamp" };
        
        //Act
        var goalsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "goals", dynamicData);

        //Approve
        Approvals.VerifyJson(goalsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_DownloadsData()
    {
        //Arrange
        var dynamicData = new[] { "ParentEventId", "InteractionId", "Timestamp" };
        
        //Act
        var downloadsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "downloads", dynamicData);
        
        //Approve
        Approvals.VerifyJson(downloadsTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_SearchesData()
    {
        //Arrange
        var dynamicData = new[] { "ParentEventId", "InteractionId", "Timestamp" };
        
        //Act
        var searchesTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "searches", dynamicData);
        
        //Approve
        Approvals.VerifyJson(searchesTableContent);
    }
    
    [Test]
    public async Task SQLApprovalTest_CampaignsData()
    {
        //Arrange
        var dynamicData = new[] { "ParentEventId", "InteractionId", "Timestamp" };
        
        //Act
        var campaignsTableContent = _etlDatabaseHelper
            .LoadJsonFromETLDatabase(_eTLDatabaseConnectionString, "campaigns", dynamicData);
        
        //Approve
        Approvals.VerifyJson(campaignsTableContent);
    }
}
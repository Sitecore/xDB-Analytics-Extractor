using xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;
using xDBAnalyticsExtractor.End2EndTests.Helpers;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.LocaleInfoBuilders;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.WebVisitBuilders;

namespace xDBAnalyticsExtractor.End2EndTests.Tests.Samples;

[TestFixture]
public class CompositeDataGeneratorSampleTests : BaseFixture
{
    [SetUp]
    public new void SetUp()
    {
    }

    [Test]
    public async Task CompositeGeneratorExample_SecondVisitOnKnowContact()
    {
        using (XClient)
        {
            var compositeEntity = new XConnectEntityRoot(XClient);
            var contact = new ContactBuilder()
                .AddPersonalInformation(DateTime.Today.AddYears(-20), "Middle", "First", "Last", "Gender", "Job",
                    "Nick", "Sfx", "Title",
                    "en")
                .AddContactIdentifiers("Facebook", "sc_test@bb.aa",
                    "myAnyKeyword"); // Your known contact identifiers go here and is recognized based on the source and email
            var interaction = new InteractionBuilder(contact.Build(), InteractionInitiator.Brand,
                InteractionBuilder.ChannelSelection.Telemarketing, "Some Agent");
            var pageView = new PageViewEventBuilder(interaction.Build(), DateTime.Today.Add(new TimeSpan(-1)),
                Guid.NewGuid(), 1, "en");

            compositeEntity.Add(contact);
            compositeEntity.Add(pageView);
            compositeEntity.Add(interaction); // important to place the interaction last in the list
            compositeEntity.BuildInteraction();
        }
    }

    [Test]
    public async Task CompositeGeneratorExample_RandomContactWithCompleteData()
    {
        var interactionList = new List<Interaction>();
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
                    .AddSitecoreRenderingDevice(Guid.NewGuid(), "GTR-9210");
            var searchEvent = new SearchEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7))
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
                    OutcomeEventBuilder.OutcomeDefinitionSelection.SalesLead, DateTime.UtcNow.AddHours(-7), "some outcome",
                    100.30m)
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var goalEvent = new GoalEventBuilder(interaction.Build(),
                    GoalEventBuilder.GoalDefinitionSelection.BrochuresRequest, DateTime.UtcNow.AddHours(-7))
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
            var downloadEvent = new DownloadEventBuilder(interaction.Build(), DateTime.UtcNow.AddHours(-7), Guid.NewGuid())
                .AddData("dataString")
                .AddDuration(TimeSpan.FromHours(1))
                .AddDataKey("dataKeyString")
                .AddText("textString")
                .AddEngagementValue(20)
                .AddItemId(Guid.NewGuid())
                .AddId(Guid.NewGuid())
                .AddParentEventId(pageViewEvent.Build().Id);
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
                    CampaignEventBuilder.CampaignDefinitionSelection.IncreaseAwareness)
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

            interactionResult = interaction.Build();
        }

        interactionList.Add(interactionResult);

        var testWorker = new TestWorker(new[] {"-csv", "-current"});
        await testWorker.TestWorkerService();

        // Run the export service
        var dataTableHelper = new DataTableHelper();
        var fileTable = dataTableHelper.LoadDataTableFromCsv(CsvFileHelper.CsvExport.Interactions);
        var mapper = new CsvInteractionToDataTableMapper();
        
        // Assert data between interaction added and csv produced
        fileTable.Should().BeEquivalentTo(mapper.XConnectEntityToDataTable(interactionList));
    }
}
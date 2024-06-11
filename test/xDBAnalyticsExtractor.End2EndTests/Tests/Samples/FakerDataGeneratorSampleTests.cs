using System.Diagnostics;
using xDBAnalyticsExtractor.End2EndTests.Helpers;
using xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.FakerDataGenerator.Builders;

namespace xDBAnalyticsExtractor.End2EndTests.Tests.Samples;

[TestFixture]
public class FakerDataGeneratorSampleTests : BaseFixture
{
    [SetUp]
    public new void SetUp()
    {
    }

    [TearDown]
    public new void TearDown()
    {
    }

    [Test]
    public async Task FakerDataGenerator_SecondVisitOnKnowContact()
    {
        using (XClient)
        {
            var interactionBuilder = new InteractionModelBuilder(await XClient)
                .WithContact(firstName: "gaga", lastName: "bababa", middleName: "some", nickname: "nick",
                    contactEmail: "amanama@gmail", birthdate: DateTime.Today,
                    preferredLanguage: "En", title: "title", jobTitle: "prog", suffix: "title", gender: "Male",
                    source: "Twitter")
                .WithInteraction()
                .WithWebVisit()
                .WithIpInfo()
                .WithUserAgent()
                .WithLocaleInfo()
                .AddEvents()
                .WithPageViewEvent()
                .WithSearchEvent()
                .WithDownloadEvent()
                .WithCampaignEvent()
                .WithOutcomeEvent()
                .WithGoalEvent()
                .WithCustomEvent()
                .Build();

            var generatedData = interactionBuilder;
            Console.WriteLine(generatedData);
        }
    }

    [Test]
    public async Task FakerDataGenerator_RandomContactWithCompleteData()
    {
        using (XClient)
        {
            var interactionBuilder = new InteractionModelBuilder(await XClient)
                .WithContact()
                .WithInteraction()
                .WithWebVisit()
                .WithIpInfo()
                .WithUserAgent()
                .WithLocaleInfo()
                .WithDeviceProfile()
                .AddEvents()
                .WithProfileScores()
                .WithChangeProfileScoreEvent()
                .WithPageViewEvent()
                .WithSearchEvent()
                .WithDownloadEvent()
                .WithCampaignEvent()
                .WithOutcomeEvent()
                .WithGoalEvent()
                .WithCustomEvent()
                .Build();

            var generatedData = interactionBuilder;
            Console.WriteLine(generatedData);
        }

        Thread.Sleep(TimeSpan.FromSeconds(5));

        var testWorker = new TestWorker(new[] {"-csv", "-current"});
        await testWorker.TestWorkerService();
    }

    [Test]
    public async Task FakerDataGenerator_InteractionWithMultipleRandomEvents()
    {
        List<Interaction> interactionsList = new List<Interaction>();
        using (XClient)
        {
            for (int i = 0; i < 10; i++)
            {
                var interactionBuilder = new InteractionModelBuilder(await XClient)
                    .WithContact()
                    .WithInteraction()
                    .WithWebVisit()
                    .WithIpInfo()
                    .WithUserAgent()
                    .WithLocaleInfo()
                    .AddEvents()
                    .WithPageViewEvent()
                    .WithANumberOfRandomEvents(50).Build();

                interactionsList.Add(interactionBuilder.Interaction);
            }
        }
    }
}
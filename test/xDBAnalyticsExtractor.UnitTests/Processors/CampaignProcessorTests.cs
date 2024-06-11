using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class CampaignProcessorTests
{
    private readonly Guid _eventDefinitionId = new("F358D040-256F-4FC6-B2A1-739ACA2B2983");
    private List<CampaignEvent>? _campaignEvents;
    private IEnumerable<CampaignModel> _expectedCampaignModels = Enumerable.Empty<CampaignModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        
        _campaignEvents = new List<CampaignEvent>(1)
        {
            new CampaignEvent(_eventDefinitionId, timestamp)
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                Text = string.Empty,
                CampaignDefinitionId = Guid.Empty,
                ParentEventId = Guid.Empty,
                DataKey = string.Empty,
                ItemId = Guid.Empty,
                Data = string.Empty,
                XObject = { }
            }
        };

        _expectedCampaignModels = new List<CampaignModel>()
        {
            new CampaignModel()
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                Text = string.Empty,
                CampaignDefinitionId = Guid.Empty,
                ParentEventId = Guid.Empty,
                DataKey = string.Empty,
                ItemId = Guid.Empty,
                Data = string.Empty,
                DefinitionId = _eventDefinitionId,
                InteractionId = _interactionId,
                Timestamp = timestamp
            }
        };
    }

    [Test]
    public void Process_WhenProvidedInteractionWithCampaigns_ReturnsCampaignModels()
    {
        var actualCampaignModels = CampaignProcessor.Process(_campaignEvents!, _interactionId).ToList();

        actualCampaignModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedCampaignModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoCampaigns_ReturnsEmptyCollection()
    {
        var actualCampaignModels = CampaignProcessor.Process(Enumerable.Empty<CampaignEvent>(), _interactionId);

        actualCampaignModels.Should().BeEmpty();
    }
}
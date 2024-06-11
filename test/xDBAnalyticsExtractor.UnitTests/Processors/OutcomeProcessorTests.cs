using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class OutcomeProcessorTests
{
    private readonly Guid _eventDefinitionId = new("9326CB1E-CEC8-48F2-9A3E-91C7DBB21DDD");
    private List<Outcome>? _outcomes;
    private Guid _itemId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222db");
    private IEnumerable<OutcomeModel> _expectedOutcomeModels = Enumerable.Empty<OutcomeModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        _outcomes = new List<Outcome>(1)
        {
            new(_eventDefinitionId, timestamp, string.Empty, 15.2m)
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                Text = string.Empty,
                ParentEventId = Guid.Empty,
                DataKey = string.Empty,
                Data = string.Empty,
                XObject = { },
                ItemId = _itemId,
                CurrencyCode = "test",
                MonetaryValue = 15.2m
            }
        };
        
        _expectedOutcomeModels = new List<OutcomeModel>()
        {
            new()
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                ItemId = _itemId,
                Text = string.Empty,
                ParentEventId = Guid.Empty,
                DataKey = string.Empty,
                Data = string.Empty,
                DefinitionId = _eventDefinitionId,
                CurrencyCode = "test",
                MonetaryValue = 15.2m,
                InteractionId = _interactionId,
                Timestamp = timestamp
            }
        };
    }

    [Test]
    public void Process_WhenProvidedInteractionWithOutcomes_ReturnsOutcomeModels()
    {
        var actualOutcomeModels = OutcomeProcessor.Process(_outcomes!, _interactionId).ToList();

        actualOutcomeModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedOutcomeModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoOutcomes_ReturnsEmptyCollection()
    {
        var actualOutcomeModels = OutcomeProcessor.Process(Enumerable.Empty<Outcome>(), _interactionId);

        actualOutcomeModels.Should().BeEmpty();
    }
}
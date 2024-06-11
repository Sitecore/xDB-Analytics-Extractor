using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class GoalProcessorTests
{
    private readonly Guid _eventDefinitionId = new("F358D040-256F-4FC6-B2A1-739ACA2B2442");
    private List<Goal>? _goals;
    private IEnumerable<GoalModel> _expectedGoalModels = Enumerable.Empty<GoalModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        _goals = new List<Goal>(1)
        {
            new(_eventDefinitionId, timestamp)
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                Text = string.Empty,
                ParentEventId = Guid.Empty,
                DataKey = string.Empty,
                ItemId = Guid.Empty,
                Data = string.Empty,
                XObject = { },
            }
        };
        
        _expectedGoalModels = new List<GoalModel>()
        {
            new ()
            {
                Id = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
                EngagementValue = 15,
                Duration = new TimeSpan(0, 1, 15),
                CustomValues = { },
                Text = string.Empty,
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
    public void Process_WhenProvidedInteractionWithGoals_ReturnsGoalModels()
    {
        var actualGoalModels = GoalProcessor.Process(_goals!, _interactionId).ToList();

        actualGoalModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedGoalModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoGoals_ReturnsEmptyCollection()
    {
        var actualGoalModels = GoalProcessor.Process(Enumerable.Empty<Goal>(), _interactionId);

        actualGoalModels.Should().BeEmpty();
    }
}
using System.Globalization;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Goals;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class GoalDefinitionProcessorTests
{
    private ResultSet<DefinitionResult<IGoalDefinition>>? _goalDefinitionsWithRecords;
    private ResultSet<DefinitionResult<IGoalDefinition>>? _goalDefinitionsEmpty;

    private List<GoalDefinitionModel> _expectedGoalDefinitionModelsWithRecords = new();

    [OneTimeSetUp]
    public void Setup()
    {
        var goalDefinition = new GoalDefinition(
            Guid.Parse("6657795b-9ea1-4b8b-beb0-a58df403cc90"), string.Empty,
            CultureInfo.CurrentCulture, "goal", DateTime.UtcNow, string.Empty);

        var definitionResult = new DefinitionResult<IGoalDefinition>(goalDefinition, true);
        _goalDefinitionsWithRecords =
            new ResultSet<DefinitionResult<IGoalDefinition>>(
                new List<DefinitionResult<IGoalDefinition>>() { definitionResult });

        _goalDefinitionsEmpty =
            new ResultSet<DefinitionResult<IGoalDefinition>>(
                new List<DefinitionResult<IGoalDefinition>>());

        var goalDefinitionModel = new GoalDefinitionModel()
        {
            Id = goalDefinition.Id,
            Alias = goalDefinition.Alias,
            CreatedBy = goalDefinition.CreatedBy,
            CreatedDate = goalDefinition.CreatedDate,
            Culture = goalDefinition.Culture.Name,
            Description = goalDefinition.Description,
            LastModifiedBy = goalDefinition.LastModifiedBy,
            LastModifiedDate = goalDefinition.LastModifiedDate
        };
        _expectedGoalDefinitionModelsWithRecords.Add(goalDefinitionModel);
    }

    [Test]
    public void Process_WhenProvidedCollectionWithRecords_ReturnsCollectionWithModels()
    {
        var actualGoalDefinitionCollection =
            GoalDefinitionProcessor.Process(_goalDefinitionsWithRecords!).ToList();

        actualGoalDefinitionCollection.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedGoalDefinitionModelsWithRecords);
    }
    
    [Test]
    public void Process_WhenProvidedEmptyCollection_ReturnsEmptyCollection()
    {
        var actualGoalDefinitionCollection =
            GoalDefinitionProcessor.Process(_goalDefinitionsEmpty!);

        actualGoalDefinitionCollection.Should().BeEmpty();
    }
}
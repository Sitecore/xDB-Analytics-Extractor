using System.Globalization;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions;

using Sitecore.Marketing.Definitions.Outcomes.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class OutcomeDefinitionProcessorTests
{
    private ResultSet<DefinitionResult<IOutcomeDefinition>>? _outcomeDefinitionsWithRecords;
    private ResultSet<DefinitionResult<IOutcomeDefinition>>? _outcomeDefinitionsEmpty;

    private List<OutcomeDefinitionModel> _expectedOutcomeDefinitionModelsWithRecords = new();

    [OneTimeSetUp]
    public void Setup()
    {
        var outcomeDefinition = new OutcomeDefinition(
            Guid.Parse("6657795b-9ea1-4b8b-beb0-a58df403cc90"), string.Empty,
            CultureInfo.CurrentCulture, "outcome", DateTime.UtcNow, string.Empty);

        var definitionResult = new DefinitionResult<IOutcomeDefinition>(outcomeDefinition, true);
        _outcomeDefinitionsWithRecords =
            new ResultSet<DefinitionResult<IOutcomeDefinition>>(
                new List<DefinitionResult<IOutcomeDefinition>>() { definitionResult });

        _outcomeDefinitionsEmpty =
            new ResultSet<DefinitionResult<IOutcomeDefinition>>(
                new List<DefinitionResult<IOutcomeDefinition>>());

        var outcomeDefinitionModel = new OutcomeDefinitionModel()
        {
            Id = outcomeDefinition.Id,
            Alias = outcomeDefinition.Alias,
            CreatedBy = outcomeDefinition.CreatedBy,
            CreatedDate = outcomeDefinition.CreatedDate,
            Culture = outcomeDefinition.Culture.Name,
            Description = outcomeDefinition.Description,
            LastModifiedBy = outcomeDefinition.LastModifiedBy,
            LastModifiedDate = outcomeDefinition.LastModifiedDate
        };
        _expectedOutcomeDefinitionModelsWithRecords.Add(outcomeDefinitionModel);
    }

    [Test]
    public void Process_WhenProvidedCollectionWithRecords_ReturnsCollectionWithModels()
    {
        var actualOutcomeDefinitionCollection =
            OutcomeDefinitionProcessor.Process(_outcomeDefinitionsWithRecords!).ToList();

        actualOutcomeDefinitionCollection.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedOutcomeDefinitionModelsWithRecords);
    }
    
    [Test]
    public void Process_WhenProvidedEmptyCollection_ReturnsEmptyCollection()
    {
        var actualOutcomeDefinitionCollection =
            OutcomeDefinitionProcessor.Process(_outcomeDefinitionsEmpty!);

        actualOutcomeDefinitionCollection.Should().BeEmpty();
    }
}
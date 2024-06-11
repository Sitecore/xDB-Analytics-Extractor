using System.Globalization;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Events;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class EventDefinitionProcessorTests
{
    private ResultSet<DefinitionResult<IEventDefinition>>? _eventDefinitionsWithRecords;
    private ResultSet<DefinitionResult<IEventDefinition>>? _eventDefinitionsEmpty;

    private List<EventDefinitionModel> _expectedEventDefinitionModelsWithRecords = new();

    [OneTimeSetUp]
    public void Setup()
    {
        var eventDefinition = new EventDefinition(
            Guid.Parse("6657795b-9ea1-4b8b-beb0-a58df403cc90"), string.Empty,
            CultureInfo.CurrentCulture, "event", DateTime.UtcNow, string.Empty);

        var definitionResult = new DefinitionResult<IEventDefinition>(eventDefinition, true);
        _eventDefinitionsWithRecords =
            new ResultSet<DefinitionResult<IEventDefinition>>(
                new List<DefinitionResult<IEventDefinition>>() { definitionResult });

        _eventDefinitionsEmpty =
            new ResultSet<DefinitionResult<IEventDefinition>>(
                new List<DefinitionResult<IEventDefinition>>());

        var eventDefinitionModel = new EventDefinitionModel()
        {
            Id = eventDefinition.Id,
            Alias = eventDefinition.Alias,
            CreatedBy = eventDefinition.CreatedBy,
            CreatedDate = eventDefinition.CreatedDate,
            Culture = eventDefinition.Culture.Name,
            Description = eventDefinition.Description,
            LastModifiedBy = eventDefinition.LastModifiedBy,
            LastModifiedDate = eventDefinition.LastModifiedDate
        };
        _expectedEventDefinitionModelsWithRecords.Add(eventDefinitionModel);
    }

    [Test]
    public void Process_WhenProvidedCollectionWithRecords_ReturnsCollectionWithModels()
    {
        var actualEventDefinitionCollection =
            EventDefinitionProcessor.Process(_eventDefinitionsWithRecords!).ToList();

        actualEventDefinitionCollection.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedEventDefinitionModelsWithRecords);
    }
    
    [Test]
    public void Process_WhenProvidedEmptyCollection_ReturnsEmptyCollection()
    {
        var eventDefinitionCollection =
            EventDefinitionProcessor.Process(_eventDefinitionsEmpty!);

        eventDefinitionCollection.Should().BeEmpty();
    }
}
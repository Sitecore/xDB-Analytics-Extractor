using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class SearchProcessorTests
{
    private readonly Guid _eventDefinitionId = new("0C179613-2073-41AB-992E-027D03D523BF");
    private List<SearchEvent>? _searchEvents;
    private Guid _itemId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222db");
    private IEnumerable<SearchModel> _expectedSearchModels = Enumerable.Empty<SearchModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        _searchEvents = new List<SearchEvent>(1)
        {
            new(timestamp)
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
                Keywords = "test"
            }
        };
        
        _expectedSearchModels = new List<SearchModel>()
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
                Keywords = "test",
                InteractionId = _interactionId,
                Timestamp = timestamp
            }
        };
    }

    [Test]
    public void Process_WhenProvidedInteractionWithSearches_ReturnsSearchModels()
    {
        var actualSearchModels = SearchProcessor.Process(_searchEvents!, _interactionId).ToList();

        actualSearchModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedSearchModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoSearches_ReturnsEmptyCollection()
    {
        var actualSearchModels = SearchProcessor.Process(Enumerable.Empty<SearchEvent>(), _interactionId);

        actualSearchModels.Should().BeEmpty();
    }
}
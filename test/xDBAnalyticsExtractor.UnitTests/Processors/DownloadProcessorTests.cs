using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class DownloadProcessorTests
{
    private readonly Guid _eventDefinitionId = new("FA72E131-3CFD-481C-8E15-04496E9586DC");
    private List<DownloadEvent>? _downloadEvents;
    private Guid _itemId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222db");
    private IEnumerable<DownloadModel> _expectedDownloadModels = Enumerable.Empty<DownloadModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        _downloadEvents = new List<DownloadEvent>(1)
        {
            new(timestamp, _itemId)
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
            }
        };
        
        _expectedDownloadModels = new List<DownloadModel>()
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
                InteractionId = _interactionId,
                Timestamp = timestamp,
            }
        };
    }

    [Test]
    public void Process_WhenProvidedInteractionWithDownloads_ReturnsDownloadModels()
    {
        var actualDownloadModels = DownloadProcessor.Process(_downloadEvents!, _interactionId).ToList();
        

        actualDownloadModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedDownloadModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoDownloads_ReturnsEmptyCollection()
    {
        var actualDownloadEvents = DownloadProcessor.Process(Enumerable.Empty<DownloadEvent>(), _interactionId);

        actualDownloadEvents.Should().BeEmpty();
    }
}
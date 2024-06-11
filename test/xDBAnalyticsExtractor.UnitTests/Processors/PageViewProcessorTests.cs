using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class PageViewProcessorTests
{
    private readonly Guid _eventDefinitionId = new("9326CB1E-CEC8-48F2-9A3E-91C7DBB2166C");
    private List<PageViewEvent>? _pageViewEvents;
    private Guid _itemId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222db");
    private IEnumerable<PageViewModel> _expectedPageViewModels = Enumerable.Empty<PageViewModel>();
    private readonly Guid _interactionId = Guid.Parse("4e0ce51f-51ff-419c-8dbc-bd26e44b56ed");

    [OneTimeSetUp]
    public void Setup()
    {
        var timestamp = new DateTime(2023, 10, 02);
        _pageViewEvents = new List<PageViewEvent>(1)
        {
            new(timestamp, _itemId, 1, "test")
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
                ItemLanguage = "test",
                ItemVersion = 1,
                Url = "test",
                SitecoreRenderingDevice = null
            }
        };

        _expectedPageViewModels = new List<PageViewModel>()
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
                ItemLanguage = "test",
                ItemVersion = 1,
                Url = "test",
                InteractionId = _interactionId,
                Timestamp = timestamp
            }
        };
    }

    [Test]
    public void Process_WhenProvidedInteractionWithPageViews_ReturnsPageViewModels()
    {
        var actualPageViewModels = PageViewProcessor.Process(_pageViewEvents!, _interactionId).ToList();

        actualPageViewModels.Should().NotBeNull()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedPageViewModels, options => options.Excluding(o => o.CustomValues));
    }

    [Test]
    public void Process_WhenProvidedInteractionWithNoPageViews_ReturnsEmptyCollection()
    {
        var actualPageViewModels = PageViewProcessor.Process(Enumerable.Empty<PageViewEvent>(), _interactionId);

        actualPageViewModels.Should().BeEmpty();
    }
}
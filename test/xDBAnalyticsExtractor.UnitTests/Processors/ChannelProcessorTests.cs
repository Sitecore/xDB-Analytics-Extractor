using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.Marketing.Taxonomy.Model;
using Sitecore.Marketing.Taxonomy.Model.Channel;
using System.Globalization;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class ChannelProcessorTests
{
    private List<Channel> _channelsWithRecords = new();
    private List<ChannelModel> _expectedChannelModelsWithRecords = new();
    private IEnumerable<Channel> _channelsEmpty = Enumerable.Empty<Channel>();
    public static CultureInfo TestLanguage
    {
        get
        {
            return new CultureInfo("en");
        }
    }

    [OneTimeSetUp]
    public void Setup()
    {
        var channel = new Channel(Guid.Parse("bb7ab59e-928e-11ee-b9d1-0242ac120002"))
        {
            Code = "Code",
            Description = "Description",
            Name = "Name",
            Uri = new TaxonUri(TestLanguage, Guid.Empty)
        };
        _channelsWithRecords.Add(channel);

        var channelModel = new ChannelModel()
        {
            Id = Guid.Parse("bb7ab59e-928e-11ee-b9d1-0242ac120002"),
            Code = "Code",
            Description = "Description",
            Name = "Name",
            Uri = "/{00000000-0000-0000-0000-000000000000}?lang=en",
            UriCulture = "en",
            UriPath = "/{00000000-0000-0000-0000-000000000000}"
        };
        _expectedChannelModelsWithRecords.Add(channelModel);
    }

    [Test]
    public void Process_WhenProvidedCollectionWithRecords_ReturnsCollectionWithModels()
    {
        var actualChannelCollection =
            ChannelProcessor.Process(_channelsWithRecords!).ToList();

        actualChannelCollection.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedChannelModelsWithRecords);
    }

    [Test]
    public void Process_WhenProvidedEmptyCollection_ReturnsEmptyCollection()
    {
        var actualChannelCollection =
            ChannelProcessor.Process(_channelsEmpty!);
    }
}

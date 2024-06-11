using xDBAnalyticsExtractor.Models;
using Sitecore.Marketing.Taxonomy.Model.Channel;

namespace xDBAnalyticsExtractor.Processors;

public static class ChannelProcessor
{
    public static IEnumerable<ChannelModel> Process(IEnumerable<Channel> channels)
    {
        if (!channels.Any())
            return Enumerable.Empty<ChannelModel>();

        return channels.Select(channel => new ChannelModel()
        {
            Id = channel.Id,
            Code = channel.Code,
            Description = channel.Description,
            Name = channel.Name,
            Uri = channel.Uri.Uri,
            UriCulture = channel.Uri.Culture.ToString(),
            UriPath = channel.Uri.Path
        });
    }
}

using Sitecore.XConnect.Collection.Model;
using xDBAnalyticsExtractor.Models;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class DownloadProcessor
{
    public static IEnumerable<DownloadModel> Process(IEnumerable<DownloadEvent> downloads, Guid? interactionId)
    {
        var downloadEvents = downloads as DownloadEvent[] ?? downloads.ToArray();
        
        if(!downloadEvents.Any()) 
            return Enumerable.Empty<DownloadModel>();
        
        return downloadEvents.Select(download => new DownloadModel()
        {
            Data = download.Data,
            Duration = download.Duration,
            Id = download.Id,
            Text = download.Text,
            CustomValues = JsonSerializer.Serialize(download.CustomValues),
            DataKey = download.DataKey,
            DefinitionId = download.DefinitionId,
            EngagementValue = download.EngagementValue,
            ItemId = download.ItemId,
            ParentEventId = download.ParentEventId,
            InteractionId = interactionId,
            Timestamp = download.Timestamp,
        });
    }
}
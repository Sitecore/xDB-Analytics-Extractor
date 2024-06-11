using Sitecore.XConnect.Collection.Model;
using xDBAnalyticsExtractor.Models;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class SearchProcessor
{
    public static IEnumerable<SearchModel> Process(IEnumerable<SearchEvent> searches, Guid? interactionId)
    {
        var searchEvents = searches as SearchEvent[] ?? searches.ToArray();
        
        if(!searchEvents.Any()) 
            return Enumerable.Empty<SearchModel>();

        return searchEvents.Select(search => new SearchModel()
        {
            Data = search.Data,
            Duration = search.Duration,
            Id = search.Id,
            Text = search.Text,
            CustomValues = JsonSerializer.Serialize(search.CustomValues),
            DataKey = search.DataKey,
            DefinitionId = search.DefinitionId,
            EngagementValue = search.EngagementValue,
            ItemId = search.ItemId,
            ParentEventId = search.ParentEventId,
            Keywords = search.Keywords,
            InteractionId = interactionId,
            Timestamp = search.Timestamp,
        });
    }
}
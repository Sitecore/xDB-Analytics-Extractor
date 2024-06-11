using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect.Collection.Model;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class PageViewProcessor
{
    public static IEnumerable<PageViewModel> Process(IEnumerable<PageViewEvent> pageViews, Guid? interactionId)
    {
        var pageViewEvents = pageViews as PageViewEvent[] ?? pageViews.ToArray();
        
        if(!pageViewEvents.Any()) 
            return Enumerable.Empty<PageViewModel>();

        return pageViewEvents.Select(pageView => new PageViewModel()
        {
            Data = pageView.Data,
            Duration = pageView.Duration,
            Id = pageView.Id,
            Text = pageView.Text,
            CustomValues = JsonSerializer.Serialize(pageView.CustomValues),
            DataKey = pageView.DataKey,
            DefinitionId = pageView.DefinitionId,
            EngagementValue = pageView.EngagementValue,
            ItemId = pageView.ItemId,
            ParentEventId = pageView.ParentEventId,
            Url = pageView.Url,
            ItemLanguage = pageView.ItemLanguage,
            ItemVersion = pageView.ItemVersion,
            InteractionId = interactionId,
            Timestamp = pageView.Timestamp,
        });
    }
}
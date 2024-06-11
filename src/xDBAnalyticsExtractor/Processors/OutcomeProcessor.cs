using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class OutcomeProcessor
{
    public static IEnumerable<OutcomeModel> Process(IEnumerable<Outcome> outcomes, Guid? interactionId)
    {
        var outcomeEvents = outcomes as Outcome[] ?? outcomes.ToArray();
        
        if(!outcomeEvents.Any()) 
            return Enumerable.Empty<OutcomeModel>();

        return outcomeEvents.Select(outcome => new OutcomeModel()
        {
            Data = outcome.Data,
            Duration = outcome.Duration,
            Id = outcome.Id,
            Text = outcome.Text,
            CustomValues = JsonSerializer.Serialize(outcome.CustomValues),
            DataKey = outcome.DataKey,
            DefinitionId = outcome.DefinitionId,
            EngagementValue = outcome.EngagementValue,
            ItemId = outcome.ItemId,
            ParentEventId = outcome.ParentEventId,
            CurrencyCode = outcome.CurrencyCode,
            MonetaryValue = outcome.MonetaryValue,
            InteractionId = interactionId,
            Timestamp = outcome.Timestamp,
        });
    }
}
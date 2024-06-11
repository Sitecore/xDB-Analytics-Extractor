using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class GoalProcessor
{
    public static IEnumerable<GoalModel> Process(IEnumerable<Goal> goals, Guid? interactionId)
    {
        var goalEvents = goals as Goal[] ?? goals.ToArray();
        
        if(!goalEvents.Any()) 
            return Enumerable.Empty<GoalModel>();
        
        return goalEvents.Select(goal => new GoalModel()
        {
            Data = goal.Data,
            Duration = goal.Duration,
            Id = goal.Id,
            Text = goal.Text,
            CustomValues = JsonSerializer.Serialize(goal.CustomValues),
            DataKey = goal.DataKey,
            DefinitionId = goal.DefinitionId,
            EngagementValue = goal.EngagementValue,
            ItemId = goal.ItemId,
            ParentEventId = goal.ParentEventId,
            InteractionId = interactionId,
            Timestamp = goal.Timestamp,
        });
    }
}
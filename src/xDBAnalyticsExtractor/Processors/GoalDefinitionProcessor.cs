using Sitecore.Marketing.Definitions.Goals;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Core;
using xDBAnalyticsExtractor.Models;

namespace xDBAnalyticsExtractor.Processors;

public static class GoalDefinitionProcessor
{
    public static IEnumerable<GoalDefinitionModel> Process(ResultSet<DefinitionResult<IGoalDefinition>> goalDefinitions)
    {
        if (goalDefinitions.Count == 0) 
            return Enumerable.Empty<GoalDefinitionModel>();
        
        return goalDefinitions.Select(goalDefinition => new GoalDefinitionModel()
        {
            Id = goalDefinition.Data.Id,
            Alias = goalDefinition.Data.Alias,
            CreatedBy = goalDefinition.Data.CreatedBy,
            CreatedDate = goalDefinition.Data.CreatedDate,
            Culture = goalDefinition.Data.Culture.Name,
            Description = goalDefinition.Data.Description,
            LastModifiedBy = goalDefinition.Data.LastModifiedBy,
            LastModifiedDate = goalDefinition.Data.LastModifiedDate
        });
    }
}
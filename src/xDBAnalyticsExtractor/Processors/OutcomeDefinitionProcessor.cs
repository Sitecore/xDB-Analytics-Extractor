using xDBAnalyticsExtractor.Models;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions.Outcomes.Model;

namespace xDBAnalyticsExtractor.Processors;

public static class OutcomeDefinitionProcessor
{
    public static IEnumerable<OutcomeDefinitionModel> Process(ResultSet<DefinitionResult<IOutcomeDefinition>> outcomeDefinitions)
    {
        if (outcomeDefinitions.Count == 0) 
            return Enumerable.Empty<OutcomeDefinitionModel>();
        
        return outcomeDefinitions.Select(outcomeDefinition => new OutcomeDefinitionModel()
        {
            Id = outcomeDefinition.Data.Id,
            Alias = outcomeDefinition.Data.Alias,
            CreatedBy = outcomeDefinition.Data.CreatedBy,
            CreatedDate = outcomeDefinition.Data.CreatedDate,
            Culture = outcomeDefinition.Data.Culture.Name,
            Description = outcomeDefinition.Data.Description,
            LastModifiedBy = outcomeDefinition.Data.LastModifiedBy,
            LastModifiedDate = outcomeDefinition.Data.LastModifiedDate            
        });
    }
}
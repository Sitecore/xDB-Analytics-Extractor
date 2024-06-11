using xDBAnalyticsExtractor.Models;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions.Events;

namespace xDBAnalyticsExtractor.Processors;

public static class EventDefinitionProcessor
{
    public static IEnumerable<EventDefinitionModel> Process(ResultSet<DefinitionResult<IEventDefinition>> eventDefinitions)
    {
        if (eventDefinitions.Count == 0) 
            return Enumerable.Empty<EventDefinitionModel>();
        
        return eventDefinitions.Select(eventDefinition => new EventDefinitionModel()
        {
            Id = eventDefinition.Data.Id,
            Alias = eventDefinition.Data.Alias,
            CreatedBy = eventDefinition.Data.CreatedBy,
            CreatedDate = eventDefinition.Data.CreatedDate,
            Culture = eventDefinition.Data.Culture.Name,
            Description = eventDefinition.Data.Description,
            LastModifiedBy = eventDefinition.Data.LastModifiedBy,
            LastModifiedDate = eventDefinition.Data.LastModifiedDate
        });
    }
}

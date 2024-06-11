using xDBAnalyticsExtractor.Models;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Campaigns;
using Sitecore.Marketing.Core;

namespace xDBAnalyticsExtractor.Processors;

public static class CampaignDefinitionProcessor
{
    public static IEnumerable<CampaignDefinitionModel> Process(ResultSet<DefinitionResult<ICampaignActivityDefinition>> campaignDefinitions)
    {
        if (campaignDefinitions.Count == 0) 
            return Enumerable.Empty<CampaignDefinitionModel>();
        
        return campaignDefinitions.Select(campaignDefinition => new CampaignDefinitionModel()
        {
            Id = campaignDefinition.Data.Id,
            Alias = campaignDefinition.Data.Alias,
            CreatedBy = campaignDefinition.Data.CreatedBy,
            CreatedDate = campaignDefinition.Data.CreatedDate,
            Culture = campaignDefinition.Data.Culture.Name,
            Description = campaignDefinition.Data.Description,
            LastModifiedBy = campaignDefinition.Data.LastModifiedBy,
            LastModifiedDate = campaignDefinition.Data.LastModifiedDate            
        });
    }
}

using xDBAnalyticsExtractor.Models;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using System.Text.Json;

namespace xDBAnalyticsExtractor.Processors;

public static class CampaignProcessor
{
    public static IEnumerable<CampaignModel> Process(IEnumerable<CampaignEvent> campaigns, Guid? interactionId)
    {
        var campaignEvents = campaigns as CampaignEvent[] ?? campaigns.ToArray();
        
        if(!campaignEvents.Any()) 
            return Enumerable.Empty<CampaignModel>();
        
        return campaignEvents.Select(campaign => new CampaignModel()
        {
            Data = campaign.Data,
            Duration = campaign.Duration,
            Id = campaign.Id,
            Text = campaign.Text,
            CustomValues = JsonSerializer.Serialize(campaign.CustomValues),
            DataKey = campaign.DataKey,
            DefinitionId = campaign.DefinitionId,
            EngagementValue = campaign.EngagementValue,
            ItemId = campaign.ItemId,
            ParentEventId = campaign.ParentEventId,
            CampaignDefinitionId = campaign.CampaignDefinitionId,
            InteractionId = interactionId,
            Timestamp = campaign.Timestamp,
        });
    }
}
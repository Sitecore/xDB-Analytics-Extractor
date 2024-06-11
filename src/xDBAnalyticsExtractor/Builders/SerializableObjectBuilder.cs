using xDBAnalyticsExtractor.Dto;
using xDBAnalyticsExtractor.Processors;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.Builders;

public static class SerializableObjectBuilder
{
    public static InteractionDto BuildInteractionDto(Interaction interaction)
    {
        var campaigns = interaction.Events.OfType<CampaignEvent>();
        var downloads = interaction.Events.OfType<DownloadEvent>();
        var geoNetwork = interaction.GetFacet<IpInfo>();
        var goals = interaction.Events.OfType<Goal>();
        var outcomes = interaction.Events.OfType<Outcome>();
        var pageViews = interaction.Events.OfType<PageViewEvent>();
        var searches = interaction.Events.OfType<SearchEvent>();
        
        var dto = new InteractionDto
        {
            InteractionModel = InteractionProcessor.Process(interaction),
            DeviceModel = DeviceProcessor.Process(interaction.WebVisit(), interaction.UserAgentInfo(), interaction.Id),
            CampaignModels = CampaignProcessor.Process(campaigns, interaction.Id).ToList(),
            DownloadModels = DownloadProcessor.Process(downloads, interaction.Id).ToList(),
            GeoNetworkModel = GeoNetworkProcessor.Process(geoNetwork, interaction.Id),
            GoalModels = GoalProcessor.Process(goals, interaction.Id).ToList(),
            OutcomeModels = OutcomeProcessor.Process(outcomes, interaction.Id).ToList(),
            PageViewModels = PageViewProcessor.Process(pageViews, interaction.Id).ToList(),
            SearchModels = SearchProcessor.Process(searches, interaction.Id).ToList()
        };
        return dto;
    }
}
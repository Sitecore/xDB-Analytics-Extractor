using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.XConnectConfiguration;
using Sitecore.XConnect;

namespace xDBAnalyticsExtractor.Processors;

public static class InteractionProcessor
{
    public static InteractionModel? Process(Interaction? interaction)
    {
        if (interaction is null)
            return null;
        
        var metrics = InteractionCalculator.Calculate(interaction);
        return new InteractionModel()
        {
            InteractionId = interaction.Id,
            Duration = interaction.Duration,
            CampaignId = interaction.CampaignId,
            ChannelId = interaction.ChannelId,
            StartDateTime = interaction.StartDateTime,
            EndDateTime = interaction.EndDateTime,
            LastModified = interaction.LastModified,
            EngagementValue = interaction.EngagementValue,
            UserAgent = interaction.UserAgent,
            Bounces = metrics.Bounces,
            Conversions = metrics.Conversions,
            Converted = metrics.Converted,
            PageViews = metrics.PageViews,
            TimeOnSite = metrics.TimeOnSite,
            MonetaryValue = metrics.MonetaryValue,
            OutcomeOccurrences = metrics.OutcomeOccurrences
        };
    }
}
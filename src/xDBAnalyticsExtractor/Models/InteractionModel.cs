using xDBAnalyticsExtractor.Interfaces;
using Sitecore.XConnect.Collection.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Interactions")]
public class InteractionModel : IModel
{
    public Guid? InteractionId { get; set; }
    public DateTime? StartDateTime { get; set; }
    public DateTime? EndDateTime { get; set; }
    public DateTime? LastModified { get; set; }
    public Guid? CampaignId { get; set; }
    public Guid ChannelId { get; set; }
    public int EngagementValue { get; set; }
    public TimeSpan Duration { get; set; }
    public string? UserAgent { get; set; }
    public int Bounces { get; set; }
    public int Conversions { get; set; }
    public int Converted { get; set; }
    public int TimeOnSite { get; set; }
    public int PageViews { get; set; }
    public int OutcomeOccurrences { get; set; }
    public decimal MonetaryValue { get; set; }
}
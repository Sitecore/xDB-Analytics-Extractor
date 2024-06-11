using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Campaigns")]
public class CampaignModel : EventModel
{
    public Guid EventDefinitionId { get; } = new Guid("F358D040-256F-4FC6-B2A1-739ACA2B2983");
    public Guid? CampaignDefinitionId { get; set; }
}
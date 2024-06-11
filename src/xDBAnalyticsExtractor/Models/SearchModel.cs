using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Searches")]
public class SearchModel : EventModel
{
    public Guid EventDefinitionId { get; } = new Guid("0C179613-2073-41AB-992E-027D03D523BF");

    public string? Keywords { get; set; }
}
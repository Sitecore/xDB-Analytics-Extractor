using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Outcomes")]
public class OutcomeModel : EventModel
{
    public string? CurrencyCode { get; set; }
    public decimal MonetaryValue { get; set; }
}
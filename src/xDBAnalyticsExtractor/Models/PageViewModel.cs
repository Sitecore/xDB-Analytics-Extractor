using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("PageViews")]
public class PageViewModel : EventModel
{
    public Guid EventDefinitionId { get; } = new Guid("9326CB1E-CEC8-48F2-9A3E-91C7DBB2166C");
    public string? ItemLanguage { get; set; }
    public int ItemVersion { get; set; }
    public string? Url { get; set; }
}
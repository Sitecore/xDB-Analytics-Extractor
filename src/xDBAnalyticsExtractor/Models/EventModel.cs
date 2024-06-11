using xDBAnalyticsExtractor.Interfaces;

namespace xDBAnalyticsExtractor.Models;

public class EventModel : IModel
{
    public string? CustomValues { get; set; }
    public string? Data { get; set; }
    public string? DataKey { get; set; }
    public Guid? DefinitionId { get; set; }
    public Guid? ItemId { get; set; }
    public int? EngagementValue { get; set; }
    public Guid? Id { get; set; }
    public Guid? ParentEventId { get; set; }
    public string? Text { get; set; }
    public DateTime? Timestamp { get; set; }
    public TimeSpan? Duration { get; set; }
    public Guid? InteractionId { get; set; }
}
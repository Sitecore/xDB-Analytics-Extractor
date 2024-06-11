namespace xDBAnalyticsExtractor.Models;

public class DefinitionModel
{
    public Guid Id { get; set; }
    public string? Alias { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public string? Culture { get; set; }
    public string? Description { get; set; }
}

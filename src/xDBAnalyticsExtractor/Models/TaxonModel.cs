namespace xDBAnalyticsExtractor.Models;

public class TaxonModel
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string UriCulture{ get; set; } = string.Empty;
    public string UriPath { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
}

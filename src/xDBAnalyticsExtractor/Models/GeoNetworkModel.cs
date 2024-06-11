using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("GeoNetworks")]
public class GeoNetworkModel : IModel
{
    public long Id { get; }
    public string? Area { get; set; }
    public string? Country { get; set; }
    public string? Region { get; set; }
    public string? Metro { get; set; }
    public string? City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public Guid? LocationId { get; set; }
    public string? PostalCode { get; set; }
    public string? IspName { get; set; }
    public string? BusinessName { get; set; }
    public Guid? InteractionId { get; set; }
}
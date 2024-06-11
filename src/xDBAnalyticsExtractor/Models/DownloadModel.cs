using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("Downloads")]
public class DownloadModel : EventModel
{
    public Guid EventDefinitionId { get; } = new Guid("FA72E131-3CFD-481C-8E15-04496E9586DC");
}
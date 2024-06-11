using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("EventDefinitions")]
public class EventDefinitionModel : DefinitionModel, IModel
{
}

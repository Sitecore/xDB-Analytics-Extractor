using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("OutcomeDefinitions")]
public class OutcomeDefinitionModel : DefinitionModel, IModel
{ 
}

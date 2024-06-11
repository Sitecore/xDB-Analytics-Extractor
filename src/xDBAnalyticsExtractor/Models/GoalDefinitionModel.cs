using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("GoalDefinitions")]
public class GoalDefinitionModel : DefinitionModel, IModel
{   
}
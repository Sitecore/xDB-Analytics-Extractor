using xDBAnalyticsExtractor.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace xDBAnalyticsExtractor.Models;

[Table("CampaignDefinitions")]
public class CampaignDefinitionModel : DefinitionModel, IModel
{ 
}

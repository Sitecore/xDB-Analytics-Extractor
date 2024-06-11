using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;

public class SqlCampaignToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable campaignsDataTable = new DataTable();

        DataColumn campaignDefinitionId = new DataColumn("CampaignDefinitionId", typeof(Guid));
        campaignsDataTable.Columns.Add(campaignDefinitionId);        
        
        DataColumn data = new DataColumn("Data", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        campaignsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        campaignsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(Guid));
        campaignsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(Guid));
        campaignsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(int));
        campaignsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(Guid))
        {
            AllowDBNull = false
        };
        campaignsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(Guid));
        campaignsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        campaignsDataTable.Columns.Add(text);

        DataColumn timestamp = new DataColumn("Timestamp", typeof(DateTime));
        campaignsDataTable.Columns.Add(timestamp);

        DataColumn duration = new DataColumn("Duration", typeof(TimeSpan));
        campaignsDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(Guid));
        campaignsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        campaignsDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            campaignsDataTable.Rows.Add(
                interaction.Events.OfType<CampaignEvent>().Select(x => x.CampaignDefinitionId).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Timestamp).FirstOrDefault()
                    .ToString("MM/dd/yyyy HH:mm:ss.fff"),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<CampaignEvent>().Select(x => x.CustomValues).FirstOrDefault())
            ).AcceptChanges();
        }

        return campaignsDataTable;
    }
}
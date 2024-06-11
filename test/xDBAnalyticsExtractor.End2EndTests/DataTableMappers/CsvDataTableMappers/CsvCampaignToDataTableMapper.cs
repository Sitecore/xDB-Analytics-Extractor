using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvCampaignToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable campaignsDataTable = new DataTable();

        DataColumn eventDefinitionId = new DataColumn("EventDefinitionId", typeof(string));
        campaignsDataTable.Columns.Add(eventDefinitionId);
        
        DataColumn campaignDefinitionId = new DataColumn("CampaignDefinitionId", typeof(string));
        campaignsDataTable.Columns.Add(campaignDefinitionId);        
        
        DataColumn data = new DataColumn("Data", typeof(string));
        campaignsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        campaignsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        campaignsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        campaignsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        campaignsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        campaignsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        campaignsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        campaignsDataTable.Columns.Add(text);

        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        campaignsDataTable.Columns.Add(timestamp);

        DataColumn duration = new DataColumn("Duration", typeof(string));
        campaignsDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        campaignsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        campaignsDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            campaignsDataTable.Rows.Add(
                "f358d040-256f-4fc6-b2a1-739aca2b2983", // The id of Campaign event item found in path:/sitecore/system/Settings/Analytics/Page Events/Campaign
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
                    .ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<CampaignEvent>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<CampaignEvent>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return campaignsDataTable;
    }
}
using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvDownloadToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable downloadsDataTable = new DataTable();

        DataColumn eventDefinitionId = new DataColumn("EventDefinitionId", typeof(string));
        downloadsDataTable.Columns.Add(eventDefinitionId);
        
        DataColumn data = new DataColumn("Data", typeof(string));
        downloadsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        downloadsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        downloadsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        downloadsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        downloadsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        downloadsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        downloadsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        downloadsDataTable.Columns.Add(text);        
        
        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        downloadsDataTable.Columns.Add(timestamp);       
        
        DataColumn duration = new DataColumn("Duration", typeof(string));
        downloadsDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        downloadsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        downloadsDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            downloadsDataTable.Rows.Add(
                "fa72e131-3cfd-481c-8e15-04496e9586dc", // The id of Downloads event item found in path:/sitecore/system/Settings/Analytics/Page Events/Download
                interaction.Events.OfType<DownloadEvent>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<DownloadEvent>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<DownloadEvent>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return downloadsDataTable;
    }
}
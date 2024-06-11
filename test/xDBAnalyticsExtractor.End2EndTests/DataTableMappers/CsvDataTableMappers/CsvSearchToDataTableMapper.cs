using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvSearchToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable searchesDataTable = new DataTable();

        DataColumn eventDefinitionId = new DataColumn("EventDefinitionId", typeof(string));
        searchesDataTable.Columns.Add(eventDefinitionId);
        
        DataColumn keywords = new DataColumn("Keywords", typeof(string));
        searchesDataTable.Columns.Add(keywords);

        DataColumn data = new DataColumn("Data", typeof(string));
        searchesDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        searchesDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        searchesDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        searchesDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        searchesDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        searchesDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        searchesDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        searchesDataTable.Columns.Add(text);

        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        searchesDataTable.Columns.Add(timestamp);

        DataColumn duration = new DataColumn("Duration", typeof(string));
        searchesDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        searchesDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        searchesDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            searchesDataTable.Rows.Add(
                "0c179613-2073-41ab-992e-027d03d523bf", // The id of Search event item found in path:/sitecore/system/Settings/Analytics/Page Events/Search
                interaction.Events.OfType<SearchEvent>().Select(x => x.Keywords).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<SearchEvent>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<SearchEvent>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<SearchEvent>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return searchesDataTable;
    }
}
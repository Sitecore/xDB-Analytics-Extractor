using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvGoalToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable goalsDataTable = new DataTable();

        DataColumn data = new DataColumn("Data", typeof(string));
        goalsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        goalsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        goalsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        goalsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        goalsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        goalsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        goalsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        goalsDataTable.Columns.Add(text);        
        
        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        goalsDataTable.Columns.Add(timestamp);        
        
        DataColumn duration = new DataColumn("Duration", typeof(string));
        goalsDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        goalsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        goalsDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            goalsDataTable.Rows.Add(
                interaction.Events.OfType<Goal>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<Goal>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<Goal>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<Goal>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return goalsDataTable;
    }
}
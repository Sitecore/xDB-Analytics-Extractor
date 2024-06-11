using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvOutcomeToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable outcomesDataTable = new DataTable();

        DataColumn currencyCode = new DataColumn("CurrencyCode", typeof(string));
        outcomesDataTable.Columns.Add(currencyCode);

        DataColumn monetaryValue = new DataColumn("MonetaryValue", typeof(string));
        outcomesDataTable.Columns.Add(monetaryValue);

        DataColumn data = new DataColumn("Data", typeof(string));
        outcomesDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        outcomesDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        outcomesDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        outcomesDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        outcomesDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        outcomesDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        outcomesDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        outcomesDataTable.Columns.Add(text);        
        
        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        outcomesDataTable.Columns.Add(timestamp);        
        
        DataColumn duration = new DataColumn("Duration", typeof(string));
        outcomesDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        outcomesDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        outcomesDataTable.Columns.Add(customValues);

        foreach (var interaction in interactions)
        {
            outcomesDataTable.Rows.Add(
                interaction.Events.OfType<Outcome>().Select(x => x.CurrencyCode).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.MonetaryValue).FirstOrDefault().ToString("0.0"),
                interaction.Events.OfType<Outcome>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<Outcome>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<Outcome>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<Outcome>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return outcomesDataTable;
    }
}
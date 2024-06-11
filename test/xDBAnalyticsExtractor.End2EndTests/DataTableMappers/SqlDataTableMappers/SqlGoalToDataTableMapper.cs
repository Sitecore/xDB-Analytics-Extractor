using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;

public class SqlGoalToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable goalsDataTable = new DataTable();

        DataColumn data = new DataColumn("Data", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        goalsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        goalsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(Guid));
        goalsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(Guid));
        goalsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(int));
        goalsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(Guid))
        {
            AllowDBNull = false
        };
        goalsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(Guid));
        goalsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        goalsDataTable.Columns.Add(text);        
        
        DataColumn timestamp = new DataColumn("Timestamp", typeof(DateTime));
        goalsDataTable.Columns.Add(timestamp);        
        
        DataColumn duration = new DataColumn("Duration", typeof(TimeSpan));
        goalsDataTable.Columns.Add(duration);
        
        DataColumn interactionId = new DataColumn("InteractionId", typeof(Guid));
        goalsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
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
                interaction.Events.OfType<Goal>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss.fff"),
                interaction.Events.OfType<Goal>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<Goal>().Select(x => x.CustomValues).FirstOrDefault())
            ).AcceptChanges();
        }

        return goalsDataTable;
    }
}
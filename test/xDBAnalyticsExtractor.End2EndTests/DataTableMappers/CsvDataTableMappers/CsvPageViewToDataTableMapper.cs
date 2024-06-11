using System.Data;
using System.Text.Json;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvPageViewToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable pageViewsDataTable = new DataTable();

        DataColumn eventDefinitionId = new DataColumn("EventDefinitionId", typeof(string));
        pageViewsDataTable.Columns.Add(eventDefinitionId);
        
        DataColumn itemLanguage = new DataColumn("ItemLanguage", typeof(string));
        pageViewsDataTable.Columns.Add(itemLanguage);

        DataColumn itemVersion = new DataColumn("ItemVersion", typeof(string));
        pageViewsDataTable.Columns.Add(itemVersion);

        DataColumn url = new DataColumn("Url", typeof(string));
        pageViewsDataTable.Columns.Add(url);

        DataColumn data = new DataColumn("Data", typeof(string));
        pageViewsDataTable.Columns.Add(data);

        DataColumn dataKey = new DataColumn("DataKey", typeof(string));
        pageViewsDataTable.Columns.Add(dataKey);

        DataColumn definitionId = new DataColumn("DefinitionId", typeof(string));
        pageViewsDataTable.Columns.Add(definitionId);

        DataColumn itemId = new DataColumn("ItemId", typeof(string));
        pageViewsDataTable.Columns.Add(itemId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        pageViewsDataTable.Columns.Add(engagementValue);

        DataColumn id = new DataColumn("Id", typeof(string));
        pageViewsDataTable.Columns.Add(id);

        DataColumn parentEventId = new DataColumn("ParentEventId", typeof(string));
        pageViewsDataTable.Columns.Add(parentEventId);

        DataColumn text = new DataColumn("Text", typeof(string));
        pageViewsDataTable.Columns.Add(text);        
        
        DataColumn timestamp = new DataColumn("Timestamp", typeof(string));
        pageViewsDataTable.Columns.Add(timestamp);        
        
        DataColumn duration = new DataColumn("Duration", typeof(string));
        pageViewsDataTable.Columns.Add(duration);

        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        pageViewsDataTable.Columns.Add(interactionId);
        
        DataColumn customValues = new DataColumn("CustomValues", typeof(string));
        pageViewsDataTable.Columns.Add(customValues);
        
        foreach (var interaction in interactions)
        {
            pageViewsDataTable.Rows.Add(
                "9326cb1e-cec8-48f2-9a3e-91c7dbb2166c", // The id of Page View event item found in path:/sitecore/system/Marketing Control Panel/Events/Page View
                interaction.Events.OfType<PageViewEvent>().Select(x => x.ItemLanguage).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.ItemVersion).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Url).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Data).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.DataKey).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.DefinitionId).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.ItemId).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.EngagementValue).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Id).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.ParentEventId).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Text).FirstOrDefault(),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Timestamp).FirstOrDefault().ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.Events.OfType<PageViewEvent>().Select(x => x.Duration).FirstOrDefault(),
                interaction.Id,
                JsonSerializer.Serialize(interaction.Events.OfType<PageViewEvent>().Select(x => x.CustomValues).FirstOrDefault())
            );
        }

        return pageViewsDataTable;
    }
}
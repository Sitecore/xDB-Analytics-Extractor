using System.Data;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;
using DateTime = System.DateTime;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.SqlDataTableMappers;

public class SqlInteractionToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable interactionsDataTable = new DataTable();

        DataColumn interactionId = new DataColumn("InteractionId", typeof(Guid))
        {
            AllowDBNull = false
        };
        interactionsDataTable.Columns.Add(interactionId);

        DataColumn startDateTime = new DataColumn("StartDateTime", typeof(DateTime));
        interactionsDataTable.Columns.Add(startDateTime);

        DataColumn endDateTime = new DataColumn("EndDateTime", typeof(DateTime));
        interactionsDataTable.Columns.Add(endDateTime);

        DataColumn lastModified = new DataColumn("LastModified", typeof(DateTime));
        interactionsDataTable.Columns.Add(lastModified);

        DataColumn campaignId = new DataColumn("CampaignId", typeof(Guid));
        interactionsDataTable.Columns.Add(campaignId);

        DataColumn channelId = new DataColumn("ChannelId", typeof(Guid))
        {
            AllowDBNull = false
        };
        interactionsDataTable.Columns.Add(channelId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(int));
        interactionsDataTable.Columns.Add(engagementValue);

        DataColumn duration = new DataColumn("Duration", typeof(TimeSpan));
        interactionsDataTable.Columns.Add(duration);

        DataColumn userAgent = new DataColumn("UserAgent", typeof(string))
        {
            MaxLength = Int32.MaxValue
        };
        interactionsDataTable.Columns.Add(userAgent);

        DataColumn bounces = new DataColumn("Bounces", typeof(int));
        interactionsDataTable.Columns.Add(bounces);

        DataColumn conversions = new DataColumn("Conversions", typeof(int));
        interactionsDataTable.Columns.Add(conversions);

        DataColumn converted = new DataColumn("Converted", typeof(int));
        interactionsDataTable.Columns.Add(converted);

        DataColumn timeOnSite = new DataColumn("TimeOnSite", typeof(int));
        interactionsDataTable.Columns.Add(timeOnSite);

        DataColumn pageViews = new DataColumn("PageViews", typeof(int));
        interactionsDataTable.Columns.Add(pageViews);

        DataColumn outcomeOccurrences = new DataColumn("OutcomeOccurrences", typeof(int));
        interactionsDataTable.Columns.Add(outcomeOccurrences);

        DataColumn monetaryValue = new DataColumn("MonetaryValue", typeof(decimal));
        interactionsDataTable.Columns.Add(monetaryValue);

        foreach (var interaction in interactions)
        {
            interactionsDataTable.Rows.Add(
                interaction.Id,
                interaction.StartDateTime.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                interaction.EndDateTime.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                interaction.LastModified?.ToString("MM/dd/yyyy HH:mm:ss.fff"),
                interaction.CampaignId,
                interaction.ChannelId,
                interaction.EngagementValue,
                interaction.Duration,
                interaction.UserAgent,
                interaction.Events.OfType<PageViewEvent>().ToList().Count() == 1 ? 1 : 0, // As Bounces
                interaction.Events.OfType<Goal>().ToList().Count, // As Conversions
                interaction.Events.OfType<Goal>().ToList().Any() ? 1 : 0, // As Converted
                interaction.Events.OfType<PageViewEvent>().ToList().Sum(evt =>
                    evt.Duration.TotalMilliseconds > 0
                        ? (int)Math.Round(evt.Duration.TotalMilliseconds / 1000)
                        : 0), // As TimeOnSite
                interaction.Events.OfType<PageViewEvent>().ToList().Count(), // As PageViews
                interaction.Events.OfType<Outcome>().ToList().Count(), // As OutcomeOccurrences
                interaction.Events.OfType<Outcome>().ToList().Sum(evt => decimal.Parse(evt.MonetaryValue.ToString("0.0"))) // As MonetaryValue
                ).AcceptChanges(); 
        }

        return interactionsDataTable;
    }
}
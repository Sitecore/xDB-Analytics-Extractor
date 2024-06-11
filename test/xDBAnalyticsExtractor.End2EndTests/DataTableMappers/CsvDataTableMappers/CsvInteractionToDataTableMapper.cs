using System.Data;
using xDBAnalyticsExtractor.End2EndTests.Interfaces;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.DataTableMappers.CsvDataTableMappers;

public class CsvInteractionToDataTableMapper : IDataTableMapper
{
    public DataTable XConnectEntityToDataTable(IEnumerable<Interaction> interactions)
    {
        DataTable interactionsDataTable = new DataTable();

        DataColumn interactionId = new DataColumn("InteractionId", typeof(string));
        interactionsDataTable.Columns.Add(interactionId);

        DataColumn startDateTime = new DataColumn("StartDateTime", typeof(string));
        interactionsDataTable.Columns.Add(startDateTime);

        DataColumn endDateTime = new DataColumn("EndDateTime", typeof(string));
        interactionsDataTable.Columns.Add(endDateTime);

        DataColumn lastModified = new DataColumn("LastModified", typeof(string));
        interactionsDataTable.Columns.Add(lastModified);

        DataColumn campaignId = new DataColumn("CampaignId", typeof(string));
        interactionsDataTable.Columns.Add(campaignId);

        DataColumn channelId = new DataColumn("ChannelId", typeof(string));
        interactionsDataTable.Columns.Add(channelId);

        DataColumn engagementValue = new DataColumn("EngagementValue", typeof(string));
        interactionsDataTable.Columns.Add(engagementValue);

        DataColumn duration = new DataColumn("Duration", typeof(string));
        interactionsDataTable.Columns.Add(duration);

        DataColumn userAgent = new DataColumn("UserAgent", typeof(string));
        interactionsDataTable.Columns.Add(userAgent);

        DataColumn bounces = new DataColumn("Bounces", typeof(string));
        interactionsDataTable.Columns.Add(bounces);

        DataColumn conversions = new DataColumn("Conversions", typeof(string));
        interactionsDataTable.Columns.Add(conversions);

        DataColumn converted = new DataColumn("Converted", typeof(string));
        interactionsDataTable.Columns.Add(converted);

        DataColumn timeOnSite = new DataColumn("TimeOnSite", typeof(string));
        interactionsDataTable.Columns.Add(timeOnSite);

        DataColumn pageViews = new DataColumn("PageViews", typeof(string));
        interactionsDataTable.Columns.Add(pageViews);

        DataColumn outcomeOccurrences = new DataColumn("OutcomeOccurrences", typeof(string));
        interactionsDataTable.Columns.Add(outcomeOccurrences);

        DataColumn monetaryValue = new DataColumn("MonetaryValue", typeof(string));
        interactionsDataTable.Columns.Add(monetaryValue);

        foreach (var interaction in interactions)
        {
            interactionsDataTable.Rows.Add(
                interaction.Id,
                interaction.StartDateTime.ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.EndDateTime.ToString("MM/dd/yyyy HH:mm:ss"),
                interaction.LastModified?.ToString("MM/dd/yyyy HH:mm:ss"),
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
                ); 
        }

        return interactionsDataTable;
    }
}
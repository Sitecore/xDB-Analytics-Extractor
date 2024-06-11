using System.Collections.ObjectModel;
using xDBAnalyticsExtractor.ExportSchema;
using Serilog;
using Sitecore.Common;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;

namespace xDBAnalyticsExtractor.XConnectConfiguration
{
    public static class Retriever
    {
        private static readonly ExportRepository _exportRepository = new();
        public static async Task<IReadOnlyCollection<Interaction>> GetCurrentDataAsync(XConnectClientConfiguration clientConfiguration)
        {
            Log.Information($"{nameof(GetCurrentDataAsync)} started at : {DateTime.Now}");
            using var client = new XConnectClient(clientConfiguration);
            try
            {
                var export = _exportRepository.GetLatestExport();
                Log.Information($"Retrieved latest export with LastExported: {export?.LastExported}");
                var exportDate = export is null ? DateTime.MinValue : export.LastExported;
                exportDate = exportDate.SpecifyKind(DateTimeKind.Utc);
                Log.Information($"Export date is {exportDate}");

                var instanceHasRecords = await client.Interactions.Any();

                if (exportDate == DateTime.MinValue && instanceHasRecords)
                {
                    Log.Information($"Export date was equal to DateTime.MinValue, meaning that this is the first run.");
                    exportDate = await client.Interactions.Max(interaction => interaction.EndDateTime);
                    _exportRepository.InsertExport(new Export(exportDate));
                    Log.Information($"Export inserted with LastExported: {exportDate.Date}");
                    Log.Information("Since this is the first run, no records will be processed in this run.");
                    return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
                }
                else if (exportDate == DateTime.MinValue && !instanceHasRecords)
                {
                    Log.Information($"Export date was equal to DateTime.MinValue, meaning that this is the first run.");
                    var newExport = new Export(DateTime.UtcNow);
                    _exportRepository.InsertExport(newExport);
                    Log.Information($"Export inserted with LastExported: {newExport.LastExported}");
                    Log.Information("Since this is the first run, no records will be processed in this run.");
                    return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
                }

                var interactionFacets = client.Model.Facets.Where(c => c.Target == EntityType.Interaction)
                    .Select(x => x.Name).ToArray();
                var contactFacets = client.Model.Facets.Where(c => c.Target == EntityType.Contact).Select(x => x.Name).ToArray();
                var interactionEnumerator = await client.Interactions.Where(interaction => interaction.EndDateTime > exportDate)
                    .WithExpandOptions(new InteractionExpandOptions(interactionFacets)
                    {
                        Contact = new RelatedContactExpandOptions(contactFacets)
                    }).GetBatchEnumerator(200);

                IEnumerable<Interaction> interactions = new List<Interaction>();
                while (await interactionEnumerator.MoveNextAsync())
                {
                    interactions = interactions.Concat(interactionEnumerator.Current);
                }
                Log.Information($"{nameof(GetCurrentDataAsync)} finished at : {DateTime.Now}");
                return new ReadOnlyCollection<Interaction>(interactions.ToList());
            }
            catch (XdbExecutionException xdbExecutionException)
            {
                Log.Error(xdbExecutionException.Message, "Error.");
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, "Error.");
            }
            return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
        }


        public static async Task<IReadOnlyCollection<Interaction>> GetHistoricalDataAsync(XConnectClientConfiguration clientConfiguration, int numberOfDaysForHistorical)
        {
            Log.Information($"{nameof(GetHistoricalDataAsync)} started at : {DateTime.Now}");
            using var client = new XConnectClient(clientConfiguration);
            try
            {
                var export = _exportRepository.GetEarliestExport();
                Log.Information($"Retrieved earliest export with LastExported: {export?.LastExported}");
                var exportDate = export is null ? DateTime.MinValue : export.LastExported;
                exportDate = exportDate.SpecifyKind(DateTimeKind.Utc);
                var instanceHasRecords = await client.Interactions.Any();
                Log.Information($"Export date is {exportDate}");

                if (exportDate == DateTime.MinValue && instanceHasRecords)
                {
                    Log.Information($"Export date was equal to DateTime.MinValue, meaning that this is the first run.");
                    exportDate = await client.Interactions.Max(interaction => interaction.EndDateTime);
                    _exportRepository.InsertExport(new Export(exportDate));
                    Log.Information($"Export inserted: {exportDate}");
                    Log.Information("Since this is the first run, no records will be processed in this run.");
                    return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
                }
                else if (exportDate == DateTime.MinValue && !instanceHasRecords)
                {
                    Log.Information($"Export date was equal to DateTime.MinValue, meaning that this is the first run.");
                    _exportRepository.InsertExport(new Export(DateTime.UtcNow));
                    Log.Information($"Export inserted: {exportDate}");
                    Log.Information("Since this is the first run, no records will be processed in this run.");
                    return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
                }

                var interactionFacets = client.Model.Facets.Where(c => c.Target == EntityType.Interaction)
                    .Select(x => x.Name).ToArray();
                var contactFacets = client.Model.Facets.Where(c => c.Target == EntityType.Contact).Select(x => x.Name).ToArray();
                var interactionEnumerator = await client.Interactions.Where(interaction => interaction.EndDateTime < exportDate && interaction.EndDateTime >= DateTime.UtcNow.AddDays(-numberOfDaysForHistorical))
                    .WithExpandOptions(new InteractionExpandOptions(interactionFacets)
                    {
                        Contact = new RelatedContactExpandOptions(contactFacets)
                    }).GetBatchEnumerator(200);

                IEnumerable<Interaction> interactions = new List<Interaction>();
                while (await interactionEnumerator.MoveNextAsync())
                {
                    interactions = interactions.Concat(interactionEnumerator.Current);
                }
                Log.Information($"{nameof(GetHistoricalDataAsync)} finished at : {DateTime.Now}");
                return new ReadOnlyCollection<Interaction>(interactions.ToList());
            }
            catch (XdbExecutionException xdbExecutionException)
            {
                Log.Error(xdbExecutionException.Message, "Error.");
            }
            catch (Exception exception)
            {
                Log.Error(exception.Message, "Error.");
            }
            return Enumerable.Empty<Interaction>().ToList().AsReadOnly();
        }
    }
}

using System.Globalization;
using CsvHelper;
using xDBAnalyticsExtractor.Dto;
using xDBAnalyticsExtractor.Interfaces;
using xDBAnalyticsExtractor.Utils;
using Serilog;

namespace xDBAnalyticsExtractor.Exporters;

public class CSVExporter : IFileExporter
{
    private string directory = string.Empty;

    public CSVExporter(IConfiguration configuration)
    {
        var path = configuration.GetValue<string>("CSVExportPath") ?? string.Empty;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        directory = path;
    }

    private void Export<T>(IEnumerable<T> records, string fileName) where T : IModel?
    {
        try
        {
            Log.Information($"CsvExporter: Exporting {typeof(T).Name} records. Count: {records?.Count()}");
            var enumerable = records as T[] ?? records?.ToArray();
            if (enumerable is null || !enumerable.Any()) return;
            var combinedFilePath = Path.Combine(directory, $"{fileName}-export.locked");
            var combinedMovedFilePath = Path.Combine(directory, $"{fileName}-export-{DateTime.UtcNow.ToFileTimeUtc()}.csv");

            File.Create(combinedFilePath).Close();

            IOUtils.AssertFileWritable(combinedFilePath);

            using (var writer = new StreamWriter(combinedFilePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(enumerable);
            }
            IOUtils.RenameFile(combinedFilePath, combinedMovedFilePath);
            Log.Information($"CsvExporter: file exported succesfully at {combinedMovedFilePath}");
        }
        catch (CsvHelperException ex)
        {
            Log.Error(ex, "Error.");
        }
    }

    public void ExportToFile(IEnumerable<InteractionDto> records)
    {
        var (interactionModels, campaignModels, deviceModels, downloadModels,
            geoNetworkModels, goalModels, outcomeModels, pageViewModels, searchModels) = InteractionDtoUtils.SplitEntities(records);

        var (goalDefinitionModels, campaignDefinitionModels,
            outcomeDefinitionModels, eventDefinitionModels) = InteractionDtoUtils.RetrieveDefinitions();

        var channels = InteractionDtoUtils.GetTaxonModels(records);

        Export(channels, "channels");
        Export(interactionModels, "interactions");
        Export(campaignModels, "campaigns");
        Export(deviceModels, "devices");
        Export(downloadModels, "downloads");
        Export(geoNetworkModels, "geoNetworks");
        Export(goalModels, "goals");
        Export(outcomeModels, "outcomes");
        Export(pageViewModels, "pageViews");
        Export(searchModels, "searches");
        Export(goalDefinitionModels, "goalDefinitions");
        Export(campaignDefinitionModels, "campaignDefinitions");
        Export(outcomeDefinitionModels, "outcomeDefinitions");
        Export(eventDefinitionModels, "eventDefinitions");

    }
}
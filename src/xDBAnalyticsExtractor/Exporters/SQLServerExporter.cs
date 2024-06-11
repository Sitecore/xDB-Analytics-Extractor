using xDBAnalyticsExtractor.Dto;
using xDBAnalyticsExtractor.Interfaces;
using xDBAnalyticsExtractor.Utils;
using Serilog;
using System.Data.SqlClient;
using xDBAnalyticsExtractor.Extentions;

namespace xDBAnalyticsExtractor.Exporters;

public sealed class SQLServerExporter : ISQLExporter
{
    private readonly string _connectionString = string.Empty;
    public SQLServerExporter(IConfiguration configuration)
    {
        _connectionString = configuration.GetSection("SqlServer").GetValue<string>("ConnectionString") ?? string.Empty;
    }
    private void Export<T>(IEnumerable<T>? records, SqlConnection connection) where T : IModel?
    {
        try
        {
            connection.BulkMerge(records);
            Log.Information($"SQLServerExporter: BulkMerge Executed.");
            Log.Information($"SQLServerExporter: Export of {typeof(T).Name} records is finished.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error.");
        }

    }

    public void ExportToDatabase(IEnumerable<InteractionDto> records)
    {
        var (interactionModels, campaignModels, deviceModels, downloadModels, geoNetworkModels,
            goalModels, outcomeModels, pageViewModels, searchModels) = InteractionDtoUtils.SplitEntities(records);

        var (goalDefinitionModels, campaignDefinitionModels,
            outcomeDefinitionModels, eventDefinitionModels) = InteractionDtoUtils.RetrieveDefinitions();
        var channels = InteractionDtoUtils.GetTaxonModels(records);

        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        Log.Information($"SQLServerExporter: Connection opened.");

        Export(channels, connection);
        Export(goalDefinitionModels, connection);
        Export(campaignDefinitionModels, connection);
        Export(outcomeDefinitionModels, connection);
        Export(eventDefinitionModels, connection);
        Export(interactionModels, connection);
        Export(campaignModels, connection);
        Export(deviceModels, connection);
        Export(downloadModels, connection);
        Export(geoNetworkModels, connection);
        Export(goalModels, connection);
        Export(outcomeModels, connection);
        Export(pageViewModels, connection);
        Export(searchModels, connection);

        connection.Close();
        Log.Information($"SQLServerExporter: Connection closed.");
        
    }
}

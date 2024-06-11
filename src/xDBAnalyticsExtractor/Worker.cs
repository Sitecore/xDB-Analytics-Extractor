using xDBAnalyticsExtractor.Builders;
using xDBAnalyticsExtractor.ExportSchema;
using xDBAnalyticsExtractor.Interfaces;
using xDBAnalyticsExtractor.XConnectConfiguration;
using Serilog;

namespace xDBAnalyticsExtractor
{
    public class Worker : BackgroundService
    {
        private readonly XConnectClientSettings? _xConnectClientSettings;
        private readonly IFileExporter _csvExporter;
        private readonly ISQLExporter _sqlExporter;
        private readonly string _certificate;
        private readonly string _collectionClientUrl;
        private readonly string _searchClientUrl;
        private readonly string _configurationClientUrl;
        private readonly ExportRepository _exportRepository;
        private readonly CommandLineArgs _commandArgs;
        private readonly int numberOfDaysForHistorical;
        private readonly IHost _host;

        public Worker(IConfiguration configuration, IFileExporter exporter, ISQLExporter sqlExporter,
            ExportRepository exportRepository, CommandLineArgs commandArgs, IHost host)
        {
            _xConnectClientSettings = configuration.GetSection("XConnectClient").Get<XConnectClientSettings>();
            DefinitionManagerExternal.Certificate = _xConnectClientSettings?.Certificate is null ? string.Empty : _xConnectClientSettings.Certificate!;
            DefinitionManagerExternal.XConnectClientUrl = _xConnectClientSettings?.ClientUrl is null ? string.Empty : _xConnectClientSettings.ClientUrl!;
            _csvExporter = exporter;
            _sqlExporter = sqlExporter;
            _certificate = _xConnectClientSettings?.Certificate is null ? string.Empty : _xConnectClientSettings.Certificate!;
            _collectionClientUrl = _xConnectClientSettings?.CollectionClientUrl is null ? string.Empty : _xConnectClientSettings.CollectionClientUrl!;
            _searchClientUrl = _xConnectClientSettings?.SearchClientUrl is null ? string.Empty : _xConnectClientSettings.SearchClientUrl!;
            _configurationClientUrl = _xConnectClientSettings?.ConfigurationClientUrl is null ? string.Empty : _xConnectClientSettings.ConfigurationClientUrl!;
            _exportRepository = exportRepository;
            _commandArgs = commandArgs;
            numberOfDaysForHistorical = configuration.GetValue<int>("HistoricalDaysExport");
            _host = host;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Log.Information($"Export started at: {DateTime.Now}");
                var config = Configurator.Set(_certificate, _collectionClientUrl, _searchClientUrl, _configurationClientUrl);
                var configIsInitialized = await Initializer.InitializeAsync(config);
                Log.Information("XConnect config initialized: {ConfigIsInitialized}", configIsInitialized);
                if (configIsInitialized is false) return;

                if (_commandArgs.Args.Contains(CommandLineArgs.CURRENT_DATA_ARGUMENT) && !_commandArgs.Args.Contains(CommandLineArgs.HISTORICAL_DATA_ARGUMENT))
                {
                    Log.Information("Running service for current data.");

                    var interactions = await Retriever.GetCurrentDataAsync(config);
                    Log.Information($"Retrieved {interactions.Count} interactions.");

                    var dtos = interactions.Select(SerializableObjectBuilder.BuildInteractionDto)
                    .Where(dto => dto.InteractionModel is not null);

                    if (_commandArgs.Args.Contains(CommandLineArgs.SQL_ARGUMENT))
                    {
                        Log.Information($"Exporting data to SQL Server using SqlServerExporter.");
                        _sqlExporter.ExportToDatabase(dtos);
                        Log.Information("Export operation completed succesfully.");
                    }

                    if (_commandArgs.Args.Contains(CommandLineArgs.FILE_ARGUMENT))
                    {
                        Log.Information($"Exporting data to CSV file using CsvExporter.");
                        _csvExporter.ExportToFile(dtos);
                        Log.Information("Export operation completed succesfully.");
                    }

                    if (interactions.Any())
                    {
                        Log.Information($"Creating a new export based on the maximum end date-time from interactions.");
                        var dateTime = interactions.Max(interaction => interaction.EndDateTime);
                        Log.Information($"Max EndDateTime: {dateTime}");
                        var export = new Export(dateTime);
                        Log.Information($"Export created: {export}");
                        _exportRepository.InsertExport(export);
                        Log.Information($"Inserted export into sql-lite db.");
                    }
                }
                else if (_commandArgs.Args.Contains(CommandLineArgs.HISTORICAL_DATA_ARGUMENT) && !_commandArgs.Args.Contains(CommandLineArgs.CURRENT_DATA_ARGUMENT))
                {
                    Log.Information("Running service for historical data.");
                    var interactions = await Retriever.GetHistoricalDataAsync(config, numberOfDaysForHistorical);
                    Log.Information($"Retrieved {interactions.Count} interactions.");
                    var dtos = interactions.Select(SerializableObjectBuilder.BuildInteractionDto)
                    .Where(dto => dto.InteractionModel is not null);

                    if (_commandArgs.Args.Contains(CommandLineArgs.SQL_ARGUMENT) && dtos.Any())
                    {
                        Log.Information($"Exporting data to SQL Server using SqlServerExporter.");
                        _sqlExporter.ExportToDatabase(dtos);
                        Log.Information("Export operation completed succesfully.");
                    }

                    if (_commandArgs.Args.Contains(CommandLineArgs.FILE_ARGUMENT) && dtos.Any())
                    {
                        Log.Information($"Exporting data to CSV file using CsvExporter.");
                        _csvExporter.ExportToFile(dtos);
                        Log.Information("Export operation completed succesfully.");
                    }

                    if (interactions.Any())
                    {
                        Log.Information($"Creating a new export based on the maximum end date-time from interactions.");
                        var dateTime = interactions.Min(interaction => interaction.EndDateTime);
                        Log.Information($"Min EndDateTime: {dateTime}");
                        var export = new Export(dateTime);
                        Log.Information($"Export created: {export}");
                        _exportRepository.InsertExport(export);
                        Log.Information($"Inserted export into sql-lite db.");
                    }
                }
                else
                {
                    Log.Error("Wrong configuration.");
                }

                Log.Information($"Export finished at: {DateTime.Now}");
                await _host.StopAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error.");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
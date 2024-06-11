using xDBAnalyticsExtractor;
using xDBAnalyticsExtractor.Exporters;
using xDBAnalyticsExtractor.ExportSchema;
using xDBAnalyticsExtractor.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace xDBAnalyticsExtractor.End2EndTests.Helpers;

public class TestWorker
{
    private readonly IHost _host;

    public TestWorker(string[] args)
    {
        _host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
                services.AddSingleton<ExportRepository>();
                services.AddSingleton<IFileExporter, CSVExporter>();
                services.AddSingleton<ISQLExporter, SQLServerExporter>();
                services.AddSingleton(new CommandLineArgs(args));
            })
            .Build(); 
    }

    public async Task TestWorkerService()
    {
        // Wait for instance to index interaction data added
        Thread.Sleep(TimeSpan.FromSeconds(5));
        
        // Start the service
        await _host.StartAsync();

        // Stop the service
        await _host.StopAsync();
    }
}
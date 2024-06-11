using xDBAnalyticsExtractor.Exporters;
using xDBAnalyticsExtractor.ExportSchema;
using xDBAnalyticsExtractor.Interfaces;
using Serilog;

namespace xDBAnalyticsExtractor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dt = DateTime.Now;
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File($"logs/etl-logs-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<ExportRepository>();
                    services.AddSingleton<IFileExporter, CSVExporter>();
                    services.AddSingleton<ISQLExporter, SQLServerExporter>();
                    services.AddSingleton(new CommandLineArgs(args));
                })
                .Build();           
            
            host.Run();
        }
    }
}
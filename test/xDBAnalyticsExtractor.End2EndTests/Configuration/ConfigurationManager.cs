using Microsoft.Extensions.Configuration;
using xDBAnalyticsExtractor.End2EndTests.Tests.Configuration;

namespace xDBAnalyticsExtractor.End2EndTests.Configuration;

public class ConfigurationManager
{
    public ExportedData LoadExportedData(string section)
    {
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("exporteddata.json")
            .Build();
        
        var config = new ExportedData();
        configurationBuilder.GetSection(section).Bind(config);
        
        return config;
    }
    
    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables() 
            .Build();
        return config;
    }
}
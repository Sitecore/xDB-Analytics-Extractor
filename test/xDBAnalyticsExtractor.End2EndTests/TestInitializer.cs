using xDBAnalyticsExtractor.XConnectConfiguration;
using Microsoft.Extensions.Configuration;
using xDBAnalyticsExtractor.End2EndTests.Configuration;
using ConfigurationManager = xDBAnalyticsExtractor.End2EndTests.Configuration.ConfigurationManager;

namespace xDBAnalyticsExtractor.End2EndTests;

public class TestInitializer
{
    private readonly XConnectClientSettings _xConnectClientSettings;
    public DbConnectionStringSettings DbConnectionStringSettings;
    public InstanceInfoSettings InstanceInfoSettings;
    public bool IsInitialized;
    public string ETLDatabaseConnectionString;
    public string ETLfilesFolder;

    public TestInitializer()
    {
        var config = ConfigurationManager.InitConfiguration();
        _xConnectClientSettings = config.GetSection("XConnectClient").Get<XConnectClientSettings>();
        DbConnectionStringSettings = config.GetSection("DbConnectionString").Get<DbConnectionStringSettings>();
        InstanceInfoSettings = config.GetSection("InstanceInfo").Get<InstanceInfoSettings>();
        ETLDatabaseConnectionString = config.GetSection("SqlServer").GetValue<string>("ConnectionString");
        ETLfilesFolder = config.GetValue<string>("CSVExportPath");
    }

    public async Task<XConnectClient> InitializeXClient()
    {
        try
        {
            var certificate = _xConnectClientSettings?.Certificate is null
                ? string.Empty
                : _xConnectClientSettings.Certificate!;
            var clientUrl = _xConnectClientSettings?.ClientUrl is null
                ? string.Empty
                : _xConnectClientSettings.ClientUrl!;
            var collectionClientUrl = _xConnectClientSettings?.CollectionClientUrl is null
                ? string.Empty
                : _xConnectClientSettings.CollectionClientUrl!;
            var searchClientUrl = _xConnectClientSettings?.SearchClientUrl is null
                ? string.Empty
                : _xConnectClientSettings.SearchClientUrl!;
            var configurationClientUrl = _xConnectClientSettings?.ConfigurationClientUrl is null
                ? string.Empty
                : _xConnectClientSettings.ConfigurationClientUrl!;
        
            var config = Configurator.Set(certificate, collectionClientUrl, searchClientUrl, configurationClientUrl);
            IsInitialized = await Initializer.InitializeAsync(config);
            return new XConnectClient(config);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
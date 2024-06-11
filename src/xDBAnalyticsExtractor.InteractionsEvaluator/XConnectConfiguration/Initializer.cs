using Sitecore.XConnect.Client;

namespace xDBAnalyticsExtractor.InteractionsEvaluator.XConnectConfiguration;

public static class Initializer
{
    /// <summary>
    /// Initializes the xConnect client configuration.
    /// </summary>
    /// <param name="configuration">The configured XConnectClientConfiguration object ready to be initialized.</param>
    /// <returns>True if the initialization completed successfully and false if something went wrong.</returns>
    public static async Task<bool> InitializeAsync(XConnectClientConfiguration configuration)
    {
        try
        {
            await configuration.InitializeAsync();
            return true;
        }
        catch (XdbModelConflictException conflictException)
        {
            Console.WriteLine("ERROR:" + conflictException.Message);
            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }
}

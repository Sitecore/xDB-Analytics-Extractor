using System.Diagnostics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Microsoft.Extensions.Logging;

namespace xDBAnalyticsExtractor.End2EndTests.Helpers;

public class WaitHelper
{
    private ILogger<WaitHelper> _logger = CreateLogger();

    public void WaitForContactInteractionCreated(XConnectClient client, string contactId)
    {
        bool contactCreated = false;
        var reference = new ContactReference(Guid.Parse(contactId));
        
        var stopWatch = new Stopwatch();
        stopWatch.Start();

        while (!contactCreated && stopWatch.Elapsed < new TimeSpan(0, 0, 10))
        {
            var contact = client.Get<Contact>(reference, new ContactExpandOptions() {Interactions = new RelatedInteractionsExpandOptions()}); 
            if (contact != null)
            {
                contactCreated = true;
            }
            Thread.Sleep(500);
            _logger.LogInformation("Contact created: {ContactCreated}, Elapsed Time: {ElapsedTime}", contactCreated, stopWatch.ElapsedMilliseconds);
        }
        stopWatch.Stop();
    }
    
    private static ILogger<WaitHelper> CreateLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        return loggerFactory.CreateLogger<WaitHelper>();
    }
    
}
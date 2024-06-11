using Sitecore.XConnect;
using Sitecore.XConnect.Client;

namespace xDBAnalyticsExtractor.InteractionsEvaluator.XConnectConfiguration;

public static class Retriever
{
    public static async Task<int> GetNumberOfBatchesProcessedAsync(XConnectClientConfiguration clientConfiguration, CancellationToken token)
    {
        using var client = new XConnectClient(clientConfiguration);
        int count = 0;
        try
        {

            var interactionFacets = client.Model.Facets.Where(c => c.Target == EntityType.Interaction)
                .Select(x => x.Name).ToArray();
            var contactFacets = client.Model.Facets.Where(c => c.Target == EntityType.Contact).Select(x => x.Name).ToArray();
            var interactionEnumerator = await client.Interactions
                .WithExpandOptions(new InteractionExpandOptions(interactionFacets)
                {
                    Contact = new RelatedContactExpandOptions(contactFacets)
                }).GetBatchEnumerator(200);
            IEnumerable<Interaction> interactions = new List<Interaction>();
            if (token.IsCancellationRequested)
            {
                return count;
            }

            while (await interactionEnumerator.MoveNextAsync())
            {
                if (token.IsCancellationRequested)
                {
                    return count;
                }
                count++;
            }
            return count;
        }
        catch (XdbExecutionException xdbExecutionException)
        {
            Console.WriteLine(xdbExecutionException.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
        return count;
                }

    public static async Task<int> GetNumberOfInteractionsForDaysSpecified(XConnectClientConfiguration clientConfiguration)
    {
        using var client = new XConnectClient(clientConfiguration);
        try
        {
            var interactionFacets = client.Model.Facets.Where(c => c.Target == EntityType.Interaction)
                .Select(x => x.Name).ToArray();
            var contactFacets = client.Model.Facets.Where(c => c.Target == EntityType.Contact).Select(x => x.Name).ToArray();
            var interactionEnumerator = await client.Interactions.Where(interaction => interaction.EndDateTime >= DateTime.Now.AddDays(-Constants.NUMBER_OF_DAYS))
                .WithExpandOptions(new InteractionExpandOptions(interactionFacets)
                {
                    Contact = new RelatedContactExpandOptions(contactFacets)
                }).GetBatchEnumerator(200);
            IEnumerable<Interaction> interactions = new List<Interaction>();
            while (await interactionEnumerator.MoveNextAsync())
            {
                interactions = interactions.Concat(interactionEnumerator.Current);                    
            }

            return interactions.Count();
        }
        catch (XdbExecutionException xdbExecutionException)
        {
            Console.WriteLine(xdbExecutionException.Message);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }

        return 0;
    }
}

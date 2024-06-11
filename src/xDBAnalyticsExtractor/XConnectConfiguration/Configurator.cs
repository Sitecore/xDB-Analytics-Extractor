using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;

namespace xDBAnalyticsExtractor.XConnectConfiguration
{
    public static class Configurator
    {
        /// <summary>
        /// Sets up the xConnect client configuration.
        /// </summary>
        /// <param name="certificate">The thumbprint of the client certificate found in the <code>ConnectionStrings.config</code> file.</param>
        /// <param name="collectionWebApiClientUri">The URI of the collection service endpoint. Depending on the topology, it can be found in the responsible role for the service in the <code>ConnectionStrings.config</code> file.</param>
        /// <param name="searchWebApiClientUri">The URI of the search service endpoint. Depending on the topology, it can be found in the responsible role for the service in the <code>ConnectionStrings.config</code> file.</param>
        /// <param name="configurationWebApiClientUri">The URI of the configuration service endpoint. Depending on the topology, it can be found in the responsible role for the service in the <code>ConnectionStrings.config</code> file.</param>
        /// <returns>The configured XConnectClientConfiguration object.</returns>
        public static XConnectClientConfiguration Set(string certificate, string collectionWebApiClientUri,
            string searchWebApiClientUri, string configurationWebApiClientUri)
        {
            var options = CertificateHttpClientHandlerModifierOptions.Parse(certificate);

            var certificateModifier = new CertificateHttpClientHandlerModifier(options);
            List<IHttpClientModifier> clientModifiers = new();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 10, 00));
            clientModifiers.Add(timeoutClientModifier);
            var collectionClient = new CollectionWebApiClient(new Uri(collectionWebApiClientUri), clientModifiers,
                new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri(searchWebApiClientUri), clientModifiers,
                new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri(configurationWebApiClientUri),
                clientModifiers, new[] { certificateModifier });
            return new XConnectClientConfiguration(new XdbRuntimeModel(CollectionModel.Model), collectionClient,
                searchClient, configurationClient);
        }
    }
}
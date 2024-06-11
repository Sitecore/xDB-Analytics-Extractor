using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Config;
using Sitecore.Marketing.ObservableFeed.Activation;
using Sitecore.Marketing.ObservableFeed.DeleteDefinition;
using Sitecore.Marketing.Search;
using Sitecore.Marketing.Taxonomy;
using Sitecore.Xdb.ReferenceData.Client;
using Sitecore.Xdb.ReferenceData.Core.Converter;
using Sitecore.Xdb.ReferenceData.Core.Results;
using Sitecore.Marketing.Core.ObservableFeed;
using Sitecore.Xdb.Common.Web;
using Sitecore.Marketing.Definitions.Goals;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Model.Definitions.Goals;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Service.Definitions.Goals;
using Sitecore.Marketing.Definitions.Campaigns;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Service.Definitions.Campaigns;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Model.Definitions.Campaigns;
using Sitecore.Marketing.Definitions.Outcomes.Model;
using Sitecore.Marketing.Definitions.Outcomes;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Service.Definitions.Outcomes;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Model.Definitions.Outcomes;
using Sitecore.Marketing.Definitions.Events;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Service.Definitions.Events;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Model.Definitions.Events;
using Sitecore.Marketing.Taxonomy.Mapping;
using Sitecore.Marketing.Taxonomy.Caching;
using Sitecore.Marketing.Taxonomy.Resolvers.ResolveUnknownTaxon;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Service.Taxonomy;
using Sitecore.Marketing.Operations.Xdb.ReferenceData.Model.Taxonomy;
using Sitecore.Marketing.Taxonomy.Mapping.Channel;

namespace xDBAnalyticsExtractor
{
    public static class DefinitionManagerExternal
    {
        public static string XConnectClientUrl = "";
        public static string Certificate = "";

        public static GoalDefinitionManager CreateGoalDefinitionManager()
        {
            var loggerFactory = new LoggerFactory();

            var options = CertificateHttpClientHandlerModifierOptions.Parse(Certificate);

            CertificateHttpClientHandlerModifier[] handlers = { new CertificateHttpClientHandlerModifier(options) };


            var refDataClient = new ReferenceDataHttpClient(
                new DefinitionEnvelopeJsonConverter(),
                new Uri(XConnectClientUrl),
                handlers,
                new Logger<ReferenceDataHttpClient>(loggerFactory)
            );

            var repo = new GoalDefinitionReferenceDataRepository(
                refDataClient,
                new GoalDataConverter(),
                new GuidMonikerConverter(),
                new DefinitionOperationResultDiagnostics()
            );

            var services = new ServiceCollection();

            services.AddSingleton<ITaxonomyManagerProvider, TaxonomyManagerProvider>();
            services.AddSingleton<ITaxonomyClassificationResolver<IGoalDefinition>, DefaultClassificationResolver<IGoalDefinition>>();
            services.AddSingleton<FieldTaxonomyMap<IGoalDefinition>>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var classificationResolver = serviceProvider.GetRequiredService<ITaxonomyClassificationResolver<IGoalDefinition>>();

            return new GoalDefinitionManager(
                repo,
                classificationResolver,
                new EmptySearchProvider<IGoalDefinition>(),
                new ActivationRetryingObservableFeed<IGoalDefinition>(new Logger<ActivationRetryingObservableFeed<IGoalDefinition>>(loggerFactory)),
                new DummyGoalDeleteDefinitionObservableFeed(),
                new DefaultDefinitionManagerSettings()
            );
        }

        public static CampaignDefinitionManager CreateCampaignDefinitionManager()
        {
            var loggerFactory = new LoggerFactory();

            var options = CertificateHttpClientHandlerModifierOptions.Parse(Certificate);

            CertificateHttpClientHandlerModifier[] handlers = { new CertificateHttpClientHandlerModifier(options) };


            var refDataClient = new ReferenceDataHttpClient(
                new DefinitionEnvelopeJsonConverter(),
                new Uri(XConnectClientUrl),
                handlers,
                new Logger<ReferenceDataHttpClient>(loggerFactory)
            );

            var repo = new CampaignDefinitionReferenceDataRepository(
                refDataClient,
                new CampaignDataConverter(),
                new GuidMonikerConverter(),
                new DefinitionOperationResultDiagnostics()
            );

            var services = new ServiceCollection();

            services.AddSingleton<ITaxonomyManagerProvider, TaxonomyManagerProvider>();
            services.AddSingleton<ITaxonomyClassificationResolver<ICampaignActivityDefinition>, DefaultClassificationResolver<ICampaignActivityDefinition>>();
            services.AddSingleton<FieldTaxonomyMap<ICampaignActivityDefinition>>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var classificationResolver = serviceProvider.GetRequiredService<ITaxonomyClassificationResolver<ICampaignActivityDefinition>>();

            return new CampaignDefinitionManager(
                repo,
                classificationResolver,
                new EmptySearchProvider<ICampaignActivityDefinition>(),
                new ActivationRetryingObservableFeed<ICampaignActivityDefinition>(new Logger<ActivationRetryingObservableFeed<ICampaignActivityDefinition>>(loggerFactory)),
                new DummyCampaignDeleteDefinitionObservableFeed(),
                new DefaultDefinitionManagerSettings()
            );
        }

        public static OutcomeDefinitionManager CreateOutcomeDefinitionManager()
        {
            var loggerFactory = new LoggerFactory();

            var options = CertificateHttpClientHandlerModifierOptions.Parse(Certificate);

            CertificateHttpClientHandlerModifier[] handlers = { new CertificateHttpClientHandlerModifier(options) };


            var refDataClient = new ReferenceDataHttpClient(
                new DefinitionEnvelopeJsonConverter(),
                new Uri(XConnectClientUrl),
                handlers,
                new Logger<ReferenceDataHttpClient>(loggerFactory)
            );

            var repo = new OutcomeDefinitionReferenceDataRepository(
                refDataClient,
                new OutcomeDataConverter(),
                new GuidMonikerConverter(),
                new DefinitionOperationResultDiagnostics()
            );

            var services = new ServiceCollection();

            services.AddSingleton<ITaxonomyManagerProvider, TaxonomyManagerProvider>();
            services.AddSingleton<ITaxonomyClassificationResolver<IOutcomeDefinition>, DefaultClassificationResolver<IOutcomeDefinition>>();
            services.AddSingleton<FieldTaxonomyMap<IOutcomeDefinition>>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var classificationResolver = serviceProvider.GetRequiredService<ITaxonomyClassificationResolver<IOutcomeDefinition>>();

            return new OutcomeDefinitionManager(
                repo,
                classificationResolver,
                new EmptySearchProvider<IOutcomeDefinition>(),
                new ActivationRetryingObservableFeed<IOutcomeDefinition>(new Logger<ActivationRetryingObservableFeed<IOutcomeDefinition>>(loggerFactory)),
                new DummyOutcomeDeleteDefinitionObservableFeed(),
                new DefaultDefinitionManagerSettings()
            );
        }

        public static EventDefinitionManager CreateEventDefinitionManager()
        {
            var loggerFactory = new LoggerFactory();

            var options = CertificateHttpClientHandlerModifierOptions.Parse(Certificate);

            CertificateHttpClientHandlerModifier[] handlers = { new CertificateHttpClientHandlerModifier(options) };


            var refDataClient = new ReferenceDataHttpClient(
                new DefinitionEnvelopeJsonConverter(),
                new Uri(XConnectClientUrl),
                handlers,
                new Logger<ReferenceDataHttpClient>(loggerFactory)
            );

            var repo = new EventDefinitionReferenceDataRepository(
                refDataClient,
                new EventDataConverter(),
                new GuidMonikerConverter(),
                new DefinitionOperationResultDiagnostics()
            );

            var services = new ServiceCollection();

            services.AddSingleton<ITaxonomyManagerProvider, TaxonomyManagerProvider>();
            services.AddSingleton<ITaxonomyClassificationResolver<IEventDefinition>, DefaultClassificationResolver<IEventDefinition>>();
            services.AddSingleton<FieldTaxonomyMap<IEventDefinition>>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var classificationResolver = serviceProvider.GetRequiredService<ITaxonomyClassificationResolver<IEventDefinition>>();

            return new EventDefinitionManager(
                repo,
                classificationResolver,
                new EmptySearchProvider<IEventDefinition>(),
                new ActivationRetryingObservableFeed<IEventDefinition>(new Logger<ActivationRetryingObservableFeed<IEventDefinition>>(loggerFactory)),
                new DummyEventDeleteDefinitionObservableFeed(),
                new DefaultDefinitionManagerSettings()
            );
        }

        public static ChannelTaxonomyManager CreateChannelTaxonomyManager()
        {
            var loggerFactory = new LoggerFactory();

            var options = CertificateHttpClientHandlerModifierOptions.Parse(Certificate);

            CertificateHttpClientHandlerModifier[] handlers = { new CertificateHttpClientHandlerModifier(options) };


            var refDataClient = new ReferenceDataHttpClient(
                new DefinitionEnvelopeJsonConverter(),
                new Uri(XConnectClientUrl),
                handlers,
                new Logger<ReferenceDataHttpClient>(loggerFactory)
            );

            var repo = new TaxonomyReferenceDataRepository(
                refDataClient,
                new TaxonDataConverter(),
                new GuidMonikerConverter(),
                new DefinitionOperationResultDiagnostics()
            );

            var services = new ServiceCollection();

            services.AddSingleton<ITaxonomyManagerProvider, TaxonomyManagerProvider>();
            services.AddSingleton<ITaxonomyClassificationResolver<ICampaignActivityDefinition>, DefaultClassificationResolver<ICampaignActivityDefinition>>();
            services.AddSingleton<FieldTaxonomyMap<ICampaignActivityDefinition>>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            var mappers = new List<IMapper>() { new ChannelMapper() };
            var mapper = new TaxonomyTypeMapper(mappers);
            var cache = new ChannelsTaxonomyCache(new NullCacheStorageFactory(), new DefaultTaxonomyCacheSettings());
            var taxonResolver = new DefaultUnknownTaxonResolver();
            var manager = new ChannelTaxonomyManager(repo, mapper, cache, new List<DefaultUnknownTaxonResolver>() { taxonResolver });

            return manager;
        }

        class DummyGoalDeleteDefinitionObservableFeed : IDeleteDefinitionObservableFeed<IGoalDefinition>
        {
            public void NotifyObservers(DeleteDefinitionArgs<IGoalDefinition> value)
            {

            }

            IList<Sitecore.Marketing.Core.ObservableFeed.IObserver<DeleteDefinitionArgs<IGoalDefinition>>> IObservableFeed<DeleteDefinitionArgs<IGoalDefinition>>.Observers
            {
                get;
            }
        }

        class DummyCampaignDeleteDefinitionObservableFeed : IDeleteDefinitionObservableFeed<ICampaignActivityDefinition>
        {
            public void NotifyObservers(DeleteDefinitionArgs<ICampaignActivityDefinition> value)
            {

            }

            IList<Sitecore.Marketing.Core.ObservableFeed.IObserver<DeleteDefinitionArgs<ICampaignActivityDefinition>>> IObservableFeed<DeleteDefinitionArgs<ICampaignActivityDefinition>>.Observers
            {
                get;
            }
        }

        class DummyOutcomeDeleteDefinitionObservableFeed : IDeleteDefinitionObservableFeed<IOutcomeDefinition>
        {
            public void NotifyObservers(DeleteDefinitionArgs<IOutcomeDefinition> value)
            {

            }

            IList<Sitecore.Marketing.Core.ObservableFeed.IObserver<DeleteDefinitionArgs<IOutcomeDefinition>>> IObservableFeed<DeleteDefinitionArgs<IOutcomeDefinition>>.Observers
            {
                get;
            }
        }

        class DummyEventDeleteDefinitionObservableFeed : IDeleteDefinitionObservableFeed<IEventDefinition>
        {
            public void NotifyObservers(DeleteDefinitionArgs<IEventDefinition> value)
            {

            }

            IList<Sitecore.Marketing.Core.ObservableFeed.IObserver<DeleteDefinitionArgs<IEventDefinition>>> IObservableFeed<DeleteDefinitionArgs<IEventDefinition>>.Observers
            {
                get;
            }
        }

    }
}

using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using xDBAnalyticsExtractor.XConnectConfiguration;
using FluentAssertions;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class InteractionProcessorTests
{
    private Interaction? _interaction = null;
    private InteractionModel? _expectedInteractionModel;
    [OneTimeSetUp]
    public void Setup()
    {
        var obj = RuntimeHelpers.GetUninitializedObject(typeof(Interaction));
        typeof(Interaction).GetProperty("LastModified")!.SetValue(obj,new DateTime(), null);
        typeof(Interaction).GetProperty("CampaignId")!.SetValue(obj,Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"), null);
        typeof(Interaction).GetProperty("Id")!.SetValue(obj,Guid.Parse("4cc9571d-490c-4639-ae84-3f53d69ee9c0"), null);
        typeof(Interaction).GetProperty("Events")!.SetValue(obj,new EventCollection(), null);
        typeof(Interaction).GetProperty("ChannelId")!.SetValue(obj,Guid.Parse("85ae5e23-a9b3-4795-8c61-401723f2e2db"), null);
        typeof(Interaction).GetProperty("UserAgent")!.SetValue(obj,"test", null);

        _interaction = obj as Interaction;
        
        var metrics = InteractionCalculator.Calculate(_interaction!);
        _expectedInteractionModel = new InteractionModel()
        {
            InteractionId = Guid.Parse("4cc9571d-490c-4639-ae84-3f53d69ee9c0"),
            Duration = new TimeSpan(0, 0, 0),
            CampaignId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
            ChannelId = Guid.Parse("85ae5e23-a9b3-4795-8c61-401723f2e2db"),
            StartDateTime = new DateTime(),
            EndDateTime = new DateTime(),
            LastModified = new DateTime(),
            EngagementValue = 0,
            UserAgent = "test",
            Bounces = metrics.Bounces,
            Conversions = metrics.Conversions,
            Converted = metrics.Converted,
            PageViews = metrics.PageViews,
            TimeOnSite = metrics.TimeOnSite,
            MonetaryValue = metrics.MonetaryValue,
            OutcomeOccurrences = metrics.OutcomeOccurrences
        };
    }

    [Test]
    public void Process_WhenProvidedValidInteraction_ReturnsInteractionModel()
    {
        var metrics = InteractionCalculator.Calculate(_interaction!);
        var expectedInteractionModel = new InteractionModel()
        {
            InteractionId = Guid.Parse("4cc9571d-490c-4639-ae84-3f53d69ee9c0"),
            Duration = new TimeSpan(0, 0, 0),
            CampaignId = Guid.Parse("c8c0e270-5bc4-4eaa-bebf-4e14ea1222bd"),
            ChannelId = Guid.Parse("85ae5e23-a9b3-4795-8c61-401723f2e2db"),
            StartDateTime = new DateTime(),
            EndDateTime = new DateTime(),
            LastModified = new DateTime(),
            EngagementValue = 0,
            UserAgent = "test",
            Bounces = metrics.Bounces,
            Conversions = metrics.Conversions,
            Converted = metrics.Converted,
            PageViews = metrics.PageViews,
            TimeOnSite = metrics.TimeOnSite,
            MonetaryValue = metrics.MonetaryValue,
            OutcomeOccurrences = metrics.OutcomeOccurrences
        };

        var actualInteractionModel = InteractionProcessor.Process(_interaction!);

        expectedInteractionModel.Should().BeEquivalentTo(actualInteractionModel);
    }

    [Test]
    public void Process_WhenProvidedNull_ReturnsNull()
    {
        var actualInteractionModel = InteractionProcessor.Process(null);

        actualInteractionModel.Should().BeNull();
    }
}
using System.Globalization;
using xDBAnalyticsExtractor.Models;
using xDBAnalyticsExtractor.Processors;
using FluentAssertions;
using Sitecore.Marketing.Core;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Campaigns;

namespace xDBAnalyticsExtractor.UnitTests.Processors;

public class CampaignDefinitionProcessorTests
{
    private ResultSet<DefinitionResult<ICampaignActivityDefinition>>? _campaignDefinitionsWithRecords;
    private ResultSet<DefinitionResult<ICampaignActivityDefinition>>? _campaignDefinitionsEmpty;

    private List<CampaignDefinitionModel> _expectedCampaignDefinitionModelsWithRecords = new();

    [OneTimeSetUp]
    public void Setup()
    {
        var campaignActivityDefinition = new CampaignActivityDefinition(
            Guid.Parse("6657795b-9ea1-4b8b-beb0-a58df403cc90"), string.Empty,
            CultureInfo.CurrentCulture, "campaign", DateTime.UtcNow, string.Empty);

        var definitionResult = new DefinitionResult<ICampaignActivityDefinition>(campaignActivityDefinition, true);
        _campaignDefinitionsWithRecords =
            new ResultSet<DefinitionResult<ICampaignActivityDefinition>>(
                new List<DefinitionResult<ICampaignActivityDefinition>>() { definitionResult });

        _campaignDefinitionsEmpty =
            new ResultSet<DefinitionResult<ICampaignActivityDefinition>>(
                new List<DefinitionResult<ICampaignActivityDefinition>>());

        var campaignDefinitionModel = new CampaignDefinitionModel()
        {
            Id = campaignActivityDefinition.Id,
            Alias = campaignActivityDefinition.Alias,
            CreatedBy = campaignActivityDefinition.CreatedBy,
            CreatedDate = campaignActivityDefinition.CreatedDate,
            Culture = campaignActivityDefinition.Culture.Name,
            Description = campaignActivityDefinition.Description,
            LastModifiedBy = campaignActivityDefinition.LastModifiedBy,
            LastModifiedDate = campaignActivityDefinition.LastModifiedDate
        };
        _expectedCampaignDefinitionModelsWithRecords.Add(campaignDefinitionModel);
    }

    [Test]
    public void Process_WhenProvidedCollectionWithRecords_ReturnsCollectionWithModels()
    {
        var actualCampaignDefinitionCollection =
            CampaignDefinitionProcessor.Process(_campaignDefinitionsWithRecords!).ToList();

        actualCampaignDefinitionCollection.Should().NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.BeEquivalentTo(_expectedCampaignDefinitionModelsWithRecords);
    }
    
    [Test]
    public void Process_WhenProvidedEmptyCollection_ReturnsEmptyCollection()
    {
        var actualCampaignDefinitionCollection =
            CampaignDefinitionProcessor.Process(_campaignDefinitionsEmpty!);

        actualCampaignDefinitionCollection.Should().BeEmpty();
    }
}
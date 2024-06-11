using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class CampaignEventBuilder : XConnectEntityNode
{
    //Campaigns available after Installing "Extractor Package.zip"
    //Install the package in Sitecore instance under test
    //and Deploying Marketing Definitions
    public enum CampaignDefinitionSelection
    {
        BrandPromotion,
        ExtractorTestCampaign,
        IncreaseAwareness
    }

    private readonly Guid _brandPromotion = Guid.Parse("F3EC0049-1D8A-4582-8DEE-BB1BF05DE843");
    private readonly Guid _extractorTestCampaign = Guid.Parse("34509012-D6CC-4668-9E06-50BD36A844F6");
    private readonly Guid _increaseAwareness = Guid.Parse("EA0D8753-9B64-4A50-98E2-00E8143E0B8F");
    
    private readonly CampaignEvent _campaignEvent;
    private readonly Interaction _interaction;

    public CampaignEventBuilder(Interaction interaction, DateTime timestamp,
        CampaignDefinitionSelection campaignDefinition)
    {
        _interaction = interaction;
        _campaignEvent = new CampaignEvent(SelectCampaign(campaignDefinition), timestamp);
    }
    
    private Guid SelectCampaign(CampaignDefinitionSelection definitionSelection)
    {
        switch (definitionSelection)
        {
            case CampaignDefinitionSelection.BrandPromotion:
                return _brandPromotion;
            case CampaignDefinitionSelection.ExtractorTestCampaign:
                return _extractorTestCampaign;
            case CampaignDefinitionSelection.IncreaseAwareness:
                return _increaseAwareness;
            default:
                throw new ArgumentException("Invalid campaign selection");
        }
    }

    public CampaignEventBuilder AddData(string data)
    {
        _campaignEvent.Data = data;
        return this;
    }

    public CampaignEventBuilder AddDataKey(string dataKey)
    {
        _campaignEvent.DataKey = dataKey;
        return this;
    }

    public CampaignEventBuilder AddItemId(Guid itemId)
    {
        _campaignEvent.ItemId = itemId;
        return this;
    }

    public CampaignEventBuilder AddEngagementValue(int engagementValue)
    {
        _campaignEvent.EngagementValue = engagementValue;
        return this;
    }

    public CampaignEventBuilder AddId(Guid id)
    {
        _campaignEvent.Id = id;
        return this;
    }

    public CampaignEventBuilder AddParentEventId(Guid parentEventId)
    {
        _campaignEvent.ParentEventId = parentEventId;
        return this;
    }

    public CampaignEventBuilder AddText(string text)
    {
        _campaignEvent.Text = text;
        return this;
    }

    public CampaignEventBuilder AddDuration(TimeSpan duration)
    {
        _campaignEvent.Duration = duration;
        return this;
    }
    public CampaignEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _campaignEvent.CustomValues.Add($"CampaignKey{i}", $"CampaignValue{i}");
        }
        return this;
    }
    

    public CampaignEvent Build()
    {
        return _campaignEvent;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_campaignEvent);
    }
}
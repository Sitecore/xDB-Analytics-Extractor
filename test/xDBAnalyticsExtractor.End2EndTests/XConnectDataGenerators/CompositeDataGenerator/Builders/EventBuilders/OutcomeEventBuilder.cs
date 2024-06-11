namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class OutcomeEventBuilder : XConnectEntityNode
{
    //System Outcomes
    public enum OutcomeDefinitionSelection
    {
        MarketingLead,
        SalesLead,
        Opportunity,
        CloseWon,
        CloseLost,
        CloseCancelled,
        ContactAcquisition,
        ProductPurchase
    }

    private readonly Guid _marketingLead = Guid.Parse("52054874-4767-47DC-8099-8C08BFA307AA");
    private readonly Guid _salesLead = Guid.Parse("C2D9DFBC-E465-45FD-BA21-0A06EBE942D6");
    private readonly Guid _opportunity = Guid.Parse("BF6B8EE3-9FFB-4C58-9CB4-301C1C710F89");
    private readonly Guid _closeWon = Guid.Parse("5646D20E-B10A-42BA-876B-2A3BB3CBC641");
    private readonly Guid _closeLost = Guid.Parse("B4D9C749-65E7-457D-B61D-4150B9E51424");
    private readonly Guid _closeCancelled = Guid.Parse("F4830B80-1BB1-4746-89C7-96EFE40DA572");
    private readonly Guid _contactAcquisition = Guid.Parse("75D53206-47B3-4391-BD48-75C42E5FC2CE");
    private readonly Guid _productPurchase = Guid.Parse("9016E456-95CB-42E9-AD58-997D6D77AE83");

    private readonly Outcome _outcome;
    private readonly Interaction _interaction;

    public OutcomeEventBuilder(Interaction interaction, OutcomeDefinitionSelection outcomeDefinitionItemDefinition,
        DateTime timestamp, string currencyCode,
        decimal monetaryValue)
    {
        _interaction = interaction;
        _outcome = new Outcome(SelectOutcome(outcomeDefinitionItemDefinition), timestamp, currencyCode, monetaryValue);
    }

    private Guid SelectOutcome(OutcomeDefinitionSelection definitionSelection)
    {
        switch (definitionSelection)
        {
            case OutcomeDefinitionSelection.MarketingLead:
                return _marketingLead;
            case OutcomeDefinitionSelection.SalesLead:
                return _salesLead;
            case OutcomeDefinitionSelection.Opportunity:
                return _opportunity;
            case OutcomeDefinitionSelection.CloseWon:
                return _closeWon;
            case OutcomeDefinitionSelection.CloseLost:
                return _closeLost;
            case OutcomeDefinitionSelection.CloseCancelled:
                return _closeCancelled;
            case OutcomeDefinitionSelection.ContactAcquisition:
                return _contactAcquisition;
            case OutcomeDefinitionSelection.ProductPurchase:
                return _productPurchase;
            default:
                throw new ArgumentException("Invalid outcome selection");
        }
    }

    public OutcomeEventBuilder AddData(string data)
    {
        _outcome.Data = data;
        return this;
    }

    public OutcomeEventBuilder AddDataKey(string dataKey)
    {
        _outcome.DataKey = dataKey;
        return this;
    }

    public OutcomeEventBuilder AddItemId(Guid itemId)
    {
        _outcome.ItemId = itemId;
        return this;
    }

    public OutcomeEventBuilder AddEngagementValue(int engagementValue)
    {
        _outcome.EngagementValue = engagementValue;
        return this;
    }

    public OutcomeEventBuilder AddId(Guid id)
    {
        _outcome.Id = id;
        return this;
    }

    public OutcomeEventBuilder AddParentEventId(Guid parentEventId)
    {
        _outcome.ParentEventId = parentEventId;
        return this;
    }

    public OutcomeEventBuilder AddText(string text)
    {
        _outcome.Text = text;
        return this;
    }

    public OutcomeEventBuilder AddDuration(TimeSpan duration)
    {
        _outcome.Duration = duration;
        return this;
    }
    
    public OutcomeEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _outcome.CustomValues.Add($"OutcomeKey{i}", $"OutcomeValue{i}");
        }
        return this;
    }

    public Outcome Build()
    {
        return _outcome;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_outcome);
    }
}
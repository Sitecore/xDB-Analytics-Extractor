namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class GoalEventBuilder : XConnectEntityNode
{
    //System Goals
    public enum GoalDefinitionSelection
    {
        BrochuresRequest,
        ClickEmailLink,
        InstantDemo,
        Login,
        NewsletterSignup,
        Register
    }

    private readonly Guid _brochuresRequest = Guid.Parse("968897F1-328A-489D-88E8-BE78F4370958");
    private readonly Guid _clickEmailLink = Guid.Parse("87431B9B-FA39-4780-BEB3-1047B9E61876");
    private readonly Guid _instantDemo = Guid.Parse("28A7C944-B8B6-45AD-A635-6F72E8F81F69");
    private readonly Guid _login = Guid.Parse("66722F52-2D13-4DCC-90FC-EA7117CF2298");
    private readonly Guid _newsletterSignup = Guid.Parse("1779CC42-EF7A-4C58-BF19-FA85D30755C9");
    private readonly Guid _register = Guid.Parse("8FFB183B-DA1A-4C74-8F3A-9729E9FCFF6A");
    private readonly Goal _goal;
    private readonly Interaction _interaction;

    public GoalEventBuilder(Interaction interaction,
        GoalDefinitionSelection goalDefinitionItemDefinition, DateTime timestamp)
    {
        _interaction = interaction;
        _goal = new Goal(SelectGoal(goalDefinitionItemDefinition), timestamp);
    }

    private Guid SelectGoal(GoalDefinitionSelection definitionSelection)
    {
        switch (definitionSelection)
        {
            case GoalDefinitionSelection.BrochuresRequest:
                return _brochuresRequest;
            case GoalDefinitionSelection.ClickEmailLink:
                return _clickEmailLink;
            case GoalDefinitionSelection.InstantDemo:
                return _instantDemo;
            case GoalDefinitionSelection.Login:
                return _login;
            case GoalDefinitionSelection.NewsletterSignup:
                return _newsletterSignup;
            case GoalDefinitionSelection.Register:
                return _register;
            default:
                throw new ArgumentException("Invalid goal selection");
        }
    }

    public GoalEventBuilder AddData(string data)
    {
        _goal.Data = data;
        return this;
    }

    public GoalEventBuilder AddDataKey(string dataKey)
    {
        _goal.DataKey = dataKey;
        return this;
    }

    public GoalEventBuilder AddItemId(Guid itemId)
    {
        _goal.ItemId = itemId;
        return this;
    }

    public GoalEventBuilder AddEngagementValue(int engagementValue)
    {
        _goal.EngagementValue = engagementValue;
        return this;
    }

    public GoalEventBuilder AddId(Guid id)
    {
        _goal.Id = id;
        return this;
    }

    public GoalEventBuilder AddParentEventId(Guid parentEventId)
    {
        _goal.ParentEventId = parentEventId;
        return this;
    }

    public GoalEventBuilder AddText(string text)
    {
        _goal.Text = text;
        return this;
    }

    public GoalEventBuilder AddDuration(TimeSpan duration)
    {
        _goal.Duration = duration;
        return this;
    }
    
    public GoalEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _goal.CustomValues.Add($"GoalKey{i}", $"GoalValue{i}");
        }
        return this;
    }

    public Goal Build()
    {
        return _goal;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_goal);
    }
}
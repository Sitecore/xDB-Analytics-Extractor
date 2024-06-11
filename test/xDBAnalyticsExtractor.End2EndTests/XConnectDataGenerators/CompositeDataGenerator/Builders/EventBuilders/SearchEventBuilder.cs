using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class SearchEventBuilder : XConnectEntityNode
{
    private readonly SearchEvent _searchEvent;
    private readonly Interaction _interaction;

    public SearchEventBuilder(Interaction interaction, DateTime timestamp)
    {
        _interaction = interaction;
        _searchEvent = new SearchEvent(timestamp);
    }

    public SearchEventBuilder AddData(string data)
    {
        _searchEvent.Data = data;
        return this;
    }

    public SearchEventBuilder AddDataKey(string dataKey)
    {
        _searchEvent.DataKey = dataKey;
        return this;
    }

    public SearchEventBuilder AddItemId(Guid itemId)
    {
        _searchEvent.ItemId = itemId;
        return this;
    }

    public SearchEventBuilder AddEngagementValue(int engagementValue)
    {
        _searchEvent.EngagementValue = engagementValue;
        return this;
    }

    public SearchEventBuilder AddId(Guid id)
    {
        _searchEvent.Id = id;
        return this;
    }

    public SearchEventBuilder AddParentEventId(Guid parentEventId)
    {
        _searchEvent.ParentEventId = parentEventId;
        return this;
    }

    public SearchEventBuilder AddText(string text)
    {
        _searchEvent.Text = text;
        return this;
    }

    public SearchEventBuilder AddDuration(TimeSpan duration)
    {
        _searchEvent.Duration = duration;
        return this;
    }

    public SearchEventBuilder AddKeywords(string keywords)
    {
        _searchEvent.Keywords = keywords;
        return this;
    }
    
    public SearchEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _searchEvent.CustomValues.Add($"SearchKey{i}", $"SearchValue{i}");
        }
        return this;
    }

    public SearchEvent Build()
    {
        return _searchEvent;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_searchEvent);
    }
}
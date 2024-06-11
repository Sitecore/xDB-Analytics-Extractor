using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class DownloadEventBuilder : XConnectEntityNode
{
    private readonly DownloadEvent _downloadEvent;
    private readonly Interaction _interaction;

    public DownloadEventBuilder(Interaction interaction, DateTime timestamp,
        Guid itemId)
    {
        _interaction = interaction;
        _downloadEvent = new DownloadEvent(timestamp, itemId);
    }

    public DownloadEventBuilder AddData(string data)
    {
        _downloadEvent.Data = data;
        return this;
    }

    public DownloadEventBuilder AddDataKey(string dataKey)
    {
        _downloadEvent.DataKey = dataKey;
        return this;
    }

    public DownloadEventBuilder AddItemId(Guid itemId)
    {
        _downloadEvent.ItemId = itemId;
        return this;
    }

    public DownloadEventBuilder AddEngagementValue(int engagementValue)
    {
        _downloadEvent.EngagementValue = engagementValue;
        return this;
    }

    public DownloadEventBuilder AddId(Guid id)
    {
        _downloadEvent.Id = id;
        return this;
    }

    public DownloadEventBuilder AddParentEventId(Guid parentEventId)
    {
        _downloadEvent.ParentEventId = parentEventId;
        return this;
    }

    public DownloadEventBuilder AddText(string text)
    {
        _downloadEvent.Text = text;
        return this;
    }

    public DownloadEventBuilder AddDuration(TimeSpan duration)
    {
        _downloadEvent.Duration = duration;
        return this;
    }
    
    public DownloadEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _downloadEvent.CustomValues.Add($"DownloadKey{i}", $"DownloadValue{i}");
        }
        return this;
    }

    public DownloadEvent Build()
    {
        return _downloadEvent;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_downloadEvent);
    }
}
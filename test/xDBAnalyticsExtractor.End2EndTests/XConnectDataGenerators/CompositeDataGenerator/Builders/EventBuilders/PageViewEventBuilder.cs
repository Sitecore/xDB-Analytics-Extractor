using Sitecore.XConnect.Collection.Model;

namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class PageViewEventBuilder : XConnectEntityNode
{
    private readonly PageViewEvent _pageViewEvent;
    private readonly Interaction _interaction;

    public PageViewEventBuilder(Interaction interaction, DateTime timestamp,
        Guid itemId, int itemVersion, string itemLanguage)
    {
        _interaction = interaction;
        new Dictionary<string, string>();
        _pageViewEvent = new PageViewEvent(timestamp, itemId, itemVersion, itemLanguage);
    }

    public PageViewEventBuilder AddData(string data)
    {
        _pageViewEvent.Data = data;
        return this;
    }

    public PageViewEventBuilder AddDataKey(string dataKey)
    {
        _pageViewEvent.DataKey = dataKey;
        return this;
    }

    public PageViewEventBuilder AddEngagementValue(int engagementValue)
    {
        _pageViewEvent.EngagementValue = engagementValue;
        return this;
    }

    public PageViewEventBuilder AddText(string text)
    {
        _pageViewEvent.Text = text;
        return this;
    }

    public PageViewEventBuilder AddDuration(TimeSpan duration)
    {
        _pageViewEvent.Duration = duration;
        return this;
    }

    public PageViewEventBuilder AddUrl(string url)
    {
        _pageViewEvent.Url = url;
        return this;
    }    
    
    public PageViewEventBuilder AddParentEventId(Guid parentEventId)
    {
        _pageViewEvent.ParentEventId = parentEventId;
        return this;
    }

    public PageViewEventBuilder AddSitecoreRenderingDevice(Guid itemRenderingDeviceId, string renderingDeviceName)
    {
        var sitecoreDeviceData =
            new SitecoreDeviceData(itemRenderingDeviceId,
                renderingDeviceName); // Guid of the item that describes the device and the device name, it can be fake or real items
        _pageViewEvent.SitecoreRenderingDevice = sitecoreDeviceData;
        return this;
    }
    
    public PageViewEventBuilder AddCustomValues()
    {
        for (int i = 0; i < 10; i++)
        {
            _pageViewEvent.CustomValues.Add($"PageViewKey{i}", $"PageViewValue{i}");
        }
        return this;
    }
    
    public PageViewEvent Build()
    {
        return _pageViewEvent;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_pageViewEvent);
    }
}
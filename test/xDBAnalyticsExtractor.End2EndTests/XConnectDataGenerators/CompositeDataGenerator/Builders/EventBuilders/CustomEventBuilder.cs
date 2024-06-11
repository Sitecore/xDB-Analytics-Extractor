namespace xDBAnalyticsExtractor.End2EndTests.XConnectDataGenerators.CompositeDataGenerator.Builders.EventBuilders;

public class CustomEventBuilder : XConnectEntityNode
{
    private Event _event;
    private readonly Interaction _interaction;

    public CustomEventBuilder(Interaction interaction, DateTime timestamp, Guid definitionId)
    {
        _interaction = interaction;
        _event = new Event(definitionId, timestamp);

    }

    public CustomEventBuilder AddData(string data)
    {
        _event.Data = data;
        return this;
    }

    public CustomEventBuilder AddDataKey(string dataKey)
    {
        _event.DataKey = dataKey;
        return this;
    }
    
    public CustomEventBuilder AddItemId(Guid itemId)
    {
        _event.ItemId = itemId;
        return this;
    }
    public CustomEventBuilder AddEngagementValue(int engagementValue)
    {
        _event.EngagementValue = engagementValue;
        return this;
    }
    public CustomEventBuilder AddId(Guid id)
    {
        _event.Id = id;
        return this;
    }
    public CustomEventBuilder AddParentEventId(Guid parentEventId)
    {
        _event.ParentEventId = parentEventId;
        return this;
    }
    public CustomEventBuilder AddText(string text)
    {
        _event.Text = text;
        return this;
    }
    public CustomEventBuilder AddDuration(TimeSpan duration)
    {
        _event.Duration = duration;
        return this;
    }
    
    public CustomEventBuilder AddCustomValues(string data, string dataKey, Guid itemId, int engagementValue, Guid id, Guid parentEventId, string text, TimeSpan duration)
    {
        _event = new Event(_event.DefinitionId, _event.Timestamp)
        {
            Data = data,
            DataKey = dataKey,
            ItemId = itemId,
            EngagementValue = engagementValue,
            Id = id,
            ParentEventId = parentEventId,
            Text = text,
            Duration = duration,
            CustomValues =  
            {
                {"Key0", "Value0"},
                {"Key1", "Value1"},
                {"Key2", "Value2"},
                {"Key3", "Value3"},
                {"Key4", "Value4"},
                {"Key5", "Value5"},
                {"Key6", "Value6"},
                {"Key7", "Value7"},
                {"Key8", "Value8"},
                {"Key9", "Value9"},
                {"Key10", "Value10"},
                {"Key11", "Value11"},
                {"Key12", "Value12"},
                {"Key13", "Value13"},
                {"Key14", "Value14"},
                {"Key15", "Value15"},
                {"Key16", "Value16"},
                {"Key17", "Value17"},
                {"Key18", "Value18"},
                {"Key19", "Value19"},
                {"Key20", "Value20"},
                {"Key21", "Value21"},
                {"Key22", "Value22"},
                {"Key23", "Value23"},
                {"Key24", "Value24"},
                {"Key25", "Value25"},
                {"Key26", "Value26"},
                {"Key27", "Value27"},
                {"Key28", "Value28"},
                {"Key29", "Value29"},
                {"Key30", "Value30"},
                {"Key31", "Value31"},
                {"Key32", "Value32"},
                {"Key33", "Value33"},
                {"Key34", "Value34"},
                {"Key35", "Value35"},
                {"Key36", "Value36"},
                {"Key37", "Value37"},
                {"Key38", "Value38"},
                {"Key39", "Value39"},
                {"Key40", "Value40"},
                {"Key41", "Value41"},
                {"Key42", "Value42"},
                {"Key43", "Value43"},
                {"Key44", "Value44"},
                {"Key45", "Value45"},
                {"Key46", "Value46"},
                {"Key47", "Value47"},
                {"Key48", "Value48"},
                {"Key49", "Value49"}
            }
        };
        return this;
    }

    public Event Build()
    {
        return _event;
    }

    public override void BuildInteraction()
    {
        _interaction.Events.Add(_event);
    }
}
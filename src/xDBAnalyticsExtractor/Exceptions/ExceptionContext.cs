namespace xDBAnalyticsExtractor.Exceptions;

public class ExceptionContext
{
    public Type ExceptionType { get; init; }
    public Guid InteractionId { get; set; }
    public string? ExceptionMessage { get; set; }
    public string? ExceptionStackTrace { get; set; }
    public DateTime Timestamp { get; set; }

    private Dictionary<string, string?> _customData = new();
    public Dictionary<string, string?> CustomData
    {
        get => _customData;
        set
        {
            if (value != null)
            {
                _customData = value;
            }

        }
    }

    public ExceptionContext(Type exceptionType)
    {
        ExceptionType = exceptionType;
    }
}
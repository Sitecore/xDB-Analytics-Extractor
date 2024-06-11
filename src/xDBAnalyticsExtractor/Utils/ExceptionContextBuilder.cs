using xDBAnalyticsExtractor.Exceptions;

namespace xDBAnalyticsExtractor.Utils;

public class ExceptionContextBuilder
{
    private ExceptionContext _context;

    public ExceptionContextBuilder(Type exceptionType)
    {
        _context = new ExceptionContext(exceptionType);
    }

    public ExceptionContext Build() => _context;

    public ExceptionContextBuilder WithInteractionId(Guid interactionId)
    {
        _context.InteractionId = interactionId;
        return this;
    }

    public ExceptionContextBuilder WithExceptionMessage(string message)
    {
        _context.ExceptionMessage = message;
        return this;
    }

    public ExceptionContextBuilder WithExceptionStackTrace(string stackTrace)
    {
        _context.ExceptionStackTrace = stackTrace;
        return this;
    }

    public ExceptionContextBuilder WithTimestamp(DateTime timestamp)
    {
        _context.Timestamp = timestamp;
        return this;
    }

    public ExceptionContextBuilder WithCustomDataValue(string key, string value)
    {
        _context.CustomData[key] = value;
        return this;
    }
}
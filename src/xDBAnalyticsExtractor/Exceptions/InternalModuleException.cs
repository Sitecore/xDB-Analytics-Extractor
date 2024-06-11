using System.Text;

namespace xDBAnalyticsExtractor.Exceptions;

public abstract class InternalModuleException : Exception
{
    public ExceptionContext Context { get; }

    protected InternalModuleException(ExceptionContext context)
    {
        Context = context;
    }
    public virtual string ToReadable()
    {
        StringBuilder sb = new();
        sb.AppendLine($"Exception Type: {Context.ExceptionType.FullName}");
        sb.AppendLine($"Interaction ID: {Context.InteractionId}");
        sb.AppendLine($"Exception Message: {Context.ExceptionMessage}");
        sb.AppendLine($"Exception Stack Trace: {Context.ExceptionStackTrace}");
        sb.AppendLine($"Timestamp: {Context.Timestamp}");

        if (Context.CustomData.Count <= 0) return sb.ToString();
        sb.AppendLine("Custom Data:");

        foreach (var customDataEntry in Context.CustomData)
        {
            sb.AppendLine($"{customDataEntry.Key}: {customDataEntry.Value}");
        }

        return sb.ToString();
    }
}

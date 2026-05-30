namespace LogInspector;

using Serilog.Events;

public class CachedLogEvent(LogEvent logEvent, string? exception, bool hasMessageTemplate = true)
{
    private static readonly CustomSerilogFormatter CustomSerilogFormatter = new();

    public DateTimeOffset Timestamp => logEvent.Timestamp;
    public LogEventLevel Level => logEvent.Level;
    public MessageTemplate MessageTemplate => logEvent.MessageTemplate;
    public bool HasMessageTemplate { get; } = hasMessageTemplate;
    public IReadOnlyDictionary<string, LogEventPropertyValue> Properties => logEvent.Properties;
    public string? Exception => exception;
    public string? ExceptionType => GetExceptionType(exception);
    public string LoggerName =>
        TryGetProperty("SourceContext")
        ?? TryGetProperty("LoggerName")
        ?? TryGetProperty("logger")
        ?? "";

    public LogEvent LogEvent => logEvent;

    public string Message
    {
        get
        {
            if (string.IsNullOrEmpty(message))
            {
                using var writer = new StringWriter();
                CustomSerilogFormatter.Format(logEvent, writer);
                message = writer.ToString();
            }
            return message;
        }
    }
    private string? message;

    private string? TryGetProperty(string name) =>
        logEvent.Properties.TryGetValue(name, out var value)
            ? ToString(value)
            : null;

    public static string ToString(LogEventPropertyValue? propertyValue)
    {
        if (propertyValue is not ScalarValue scalarValue)
        {
            return propertyValue?.ToString() ?? "";
        }
        if (scalarValue.Value is not string stringValue)
        {
            return scalarValue.ToString();
        }
        return stringValue;
    }

    private static string? GetExceptionType(string? exception)
    {
        if (string.IsNullOrWhiteSpace(exception))
        {
            return null;
        }

        var separatorIndex = exception.IndexOf(':');
        return separatorIndex > 0
            ? exception[..separatorIndex].Trim()
            : exception.Trim();
    }
}

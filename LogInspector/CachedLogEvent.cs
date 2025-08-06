namespace LogInspector
{
    using Serilog.Events;

    public class CachedLogEvent(LogEvent logEvent, string? exception)
    {
        private static readonly CustomSerilogFormatter CustomSerilogFormatter = new();

        public DateTimeOffset Timestamp => logEvent.Timestamp;
        public LogEventLevel Level => logEvent.Level;
        public MessageTemplate MessageTemplate => logEvent.MessageTemplate;
        public IReadOnlyDictionary<string, LogEventPropertyValue> Properties => logEvent.Properties;
        public string? Exception => exception;
        public string? ExceptionType => exception?[0..exception.IndexOf(':')].Trim();

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
    }
}

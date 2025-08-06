namespace LogInspector
{
    using System.Globalization;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Parsing;

    internal class CustomSerilogFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            RenderMessage(logEvent, output);
            output.WriteLine();
        }

        private static void RenderMessage(LogEvent logEvent, TextWriter output)
        {
            var messageTemplate = logEvent.MessageTemplate;
            var tokens = messageTemplate.Tokens;
            foreach (var token in tokens)
            {
                if (token is not PropertyToken pt)
                {
                    output.Write(token);
                    continue;
                }

                if (!logEvent.Properties.TryGetValue(
                    pt.PropertyName,
                    out var propertyValue))
                {
                    output.Write(pt.PropertyName);
                    continue;
                }

                RenderPropertyValue(propertyValue, output);
            }
        }

        private static void RenderPropertyValue(
            LogEventPropertyValue propertyValue,
            TextWriter output)
        {
            if (propertyValue is not ScalarValue scalarValue)
            {
                propertyValue.Render(
                        output,
                        formatProvider: CultureInfo.CurrentCulture);
                return;
            }
            if (scalarValue.Value is not string stringValue)
            {
                scalarValue.Render(
                        output,
                        formatProvider: CultureInfo.CurrentCulture);
                return;
            }
            output.Write(stringValue);
        }
    }
}

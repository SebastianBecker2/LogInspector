namespace LogInspector
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Serilog.Events;
    using Serilog.Parsing;

    public static class LogFileParser
    {
        public static IEnumerable<CachedLogEvent> ParseLogFile(string file)
        {
            var parser = new MessageTemplateParser();

            using var stream = new FileStream(
                file,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var trimmed = line.TrimStart();
                if (trimmed.StartsWith('{'))
                {
                    var logEvent = ParseJsonLogLine(trimmed, parser);
                    if (logEvent is not null)
                    {
                        yield return logEvent;
                    }
                }
                else if (trimmed.StartsWith('<'))
                {
                    var logEvent = ParseXmlLogLine(trimmed, parser);
                    if (logEvent is not null)
                    {
                        yield return logEvent;
                    }
                }
            }
        }

        private static CachedLogEvent? ParseJsonLogLine(
            string line,
            MessageTemplateParser parser)
        {
            var jObject = JObject.Parse(line);

            var timestamp = jObject["Timestamp"]?.ToObject<DateTimeOffset>()
                ?? throw new InvalidDataException("Missing Timestamp");
            var level = ParseLogLevel(jObject["Level"]?.ToString());

            var template = jObject["MessageTemplate"]?.ToString();
            var text = jObject["Message"]?.ToString();
            var rawMessage = template ?? text ?? string.Empty;
            var messageTemplate = parser.Parse(rawMessage);
            var exception = jObject["Exception"]?.ToString();
            var properties = ParseProperties(jObject);

            var logEvent = new LogEvent(
                timestamp,
                level,
                exception: null,
                messageTemplate,
                properties.ConvertAll(p => p.Value));

            return new CachedLogEvent(
                logEvent,
                exception,
                hasMessageTemplate: template is not null);
        }

        private static CachedLogEvent? ParseXmlLogLine(
            string line,
            MessageTemplateParser parser)
        {
            var element = XElement.Parse(line);
            if (!string.Equals(element.Name.LocalName, "logEvent", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var timestampText = element.Attribute("timestamp")?.Value
                ?? element.Element("timestamp")?.Value
                ?? throw new InvalidDataException("Missing timestamp attribute or element.");
            var timestamp = DateTimeOffset.Parse(
                timestampText,
                CultureInfo.InvariantCulture);
            var level = ParseLogLevel(element.Attribute("level")?.Value);
            var message = element.Attribute("message")?.Value
                ?? element.Element("message")?.Value
                ?? string.Empty;
            var exception = element.Attribute("exception")?.Value
                ?? element.Element("exception")?.Value;

            var properties = new List<KeyValuePair<string, LogEventProperty>>();
            foreach (var attribute in element.Attributes())
            {
                if (attribute.Name.LocalName is "timestamp" or "level" or "message" or "exception")
                {
                    continue;
                }

                properties.Add(new KeyValuePair<string, LogEventProperty>(
                    attribute.Name.LocalName,
                    new LogEventProperty(
                        attribute.Name.LocalName,
                        new ScalarValue(attribute.Value))));
            }

            foreach (var child in element.Elements())
            {
                if (child.Name.LocalName is "timestamp" or "level" or "message" or "exception")
                {
                    continue;
                }

                properties.Add(new KeyValuePair<string, LogEventProperty>(
                    child.Name.LocalName,
                    new LogEventProperty(
                        child.Name.LocalName,
                        new ScalarValue(child.Value))));
            }

            var messageTemplate = parser.Parse(message);
            var logEvent = new LogEvent(
                timestamp,
                level,
                exception: null,
                messageTemplate,
                properties.ConvertAll(p => p.Value));

            return new CachedLogEvent(
                logEvent,
                exception,
                hasMessageTemplate: false);
        }

        private static List<KeyValuePair<string, LogEventProperty>> ParseProperties(
            JObject jObject)
        {
            var properties = new List<KeyValuePair<string, LogEventProperty>>();
            if (jObject["Properties"] is JObject serilogProperties)
            {
                properties.AddRange([.. serilogProperties
                    .Children<JProperty>()
                    .Select(prop => new KeyValuePair<string, LogEventProperty>(
                        prop.Name,
                        new LogEventProperty(
                            prop.Name,
                            ToScalarValue(prop.Value))))]);
            }

            foreach (var prop in jObject.Properties())
            {
                if (prop.Name is "Timestamp" or "Level" or "MessageTemplate" or "Message" or "Exception" or "RenderedMessage" or "Properties")
                {
                    continue;
                }

                properties.Add(new KeyValuePair<string, LogEventProperty>(
                    prop.Name,
                    new LogEventProperty(prop.Name, ToScalarValue(prop.Value))));
            }

            return properties;
        }

        private static LogEventLevel ParseLogLevel(string? level)
        {
            if (string.IsNullOrWhiteSpace(level))
            {
                return LogEventLevel.Information;
            }

            var normalized = level.Trim();
            if (Enum.TryParse<LogEventLevel>(normalized, ignoreCase: true, out var parsed))
            {
                return parsed;
            }

            return normalized.ToLowerInvariant() switch
            {
                "info" or "information" => LogEventLevel.Information,
                "warn" or "warning" => LogEventLevel.Warning,
                "err" or "error" => LogEventLevel.Error,
                "fatal" => LogEventLevel.Fatal,
                "verbose" => LogEventLevel.Verbose,
                "trace" => LogEventLevel.Verbose,
                "debug" => LogEventLevel.Debug,
                _ => LogEventLevel.Information,
            };
        }

        private static ScalarValue ToScalarValue(JToken value) =>
            new ScalarValue(value.Type switch
            {
                JTokenType.Integer => value.ToObject<long>(),
                JTokenType.Float => value.ToObject<double>(),
                JTokenType.String => value.ToObject<string>(),
                JTokenType.Boolean => value.ToObject<bool>(),
                JTokenType.Date => value.ToObject<DateTimeOffset>(),
                JTokenType.Guid => value.ToObject<Guid>(),
                JTokenType.Uri => value.ToObject<Uri>(),
                JTokenType.TimeSpan => value.ToObject<TimeSpan>(),
                _ => value.ToString(Formatting.None)
            });
    }
}

namespace LogInspector;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

internal static class LogEventJson
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        Formatting = Formatting.Indented,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };

    public static string Serialize(CachedLogEvent logEvent) =>
        JsonConvert.SerializeObject(logEvent.LogEvent, SerializerSettings);

    public static string SerializeMany(IEnumerable<CachedLogEvent> logEvents) =>
        string.Join(
            Environment.NewLine + Environment.NewLine,
            logEvents.Select(Serialize));
}

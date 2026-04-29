namespace LogInspector.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using Serilog.Events;
    using Xunit;

    public class LogFileParserTests
    {
        [Fact]
        public void ParseXmlLogFile_MapsNLogShortLevelNames()
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "TestData", "nlog.xml");
            var logEvents = LogFileParser.ParseLogFile(xmlPath).ToList();

            Assert.Equal(2, logEvents.Count);
            Assert.Equal(LogEventLevel.Information, logEvents[0].Level);
            Assert.Equal("DebugGui.Program", CachedLogEvent.ToString(logEvents[0].Properties["logger"]));
            Assert.Contains("Starting DebugGui with version", logEvents[0].Message);
            Assert.False(logEvents[0].HasMessageTemplate);

            Assert.Equal(LogEventLevel.Warning, logEvents[1].Level);
            Assert.Equal("DatabaseMigration", CachedLogEvent.ToString(logEvents[1].Properties["logger"]));
        }
    }
}

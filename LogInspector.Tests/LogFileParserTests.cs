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

        [Fact]
        public void ParseSerilogLogFile_ParsesMessageTemplateAndProperties()
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "TestData", "serilog.log");
            var logEvents = LogFileParser.ParseLogFile(logPath).ToList();

            Assert.NotEmpty(logEvents);
            var first = logEvents[0];

            Assert.Equal(LogEventLevel.Information, first.Level);
            Assert.True(first.HasMessageTemplate);
            Assert.Equal("Application started. Version={Version} RunId={RunId} ProcessId={ProcessId} Machine={MachineName} OS={OSVersion} ExtendedLogging={ExtendedLogging} PreviousRunUnclean={PreviousRunUnclean}", first.MessageTemplate.Text);
            Assert.Equal("1.2.8-rc4+02ba2783135d01fd56fb939a8b4a5eaeb699da22", CachedLogEvent.ToString(first.Properties["Version"]));
            Assert.Equal("App.Started", CachedLogEvent.ToString(first.Properties["EventType"]));
        }
    }
}

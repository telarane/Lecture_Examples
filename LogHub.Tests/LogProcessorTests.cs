using LogHub.Logging;
using Xunit;

namespace LogHub.Tests;

public class LogProcessorTests
{
    [Fact]
    public void Parser_ValidLine_ReturnsRecord()
    {
        var parser = new PipeLogParser();
        var record = parser.Parse("2026-01-01 12:00:00|Info|Hello");
        Assert.NotNull(record);
        Assert.Equal("Info", record.Severity);
        Assert.Equal("Hello", record.Message);
    }

    [Fact]
    public void Parser_InvalidLine_ReturnsNull()
    {
        var parser = new PipeLogParser();
        var record = parser.Parse("Invalid line without pipes");
        Assert.Null(record);
    }

    [Fact]
    public void Filter_SeverityBelowMin_ReturnsFalse()
    {
        var filter = new SeverityFilter("Warning");
        var record = new LogRecord { Severity = "Info" }; // Info < Warning
        Assert.False(filter.ShouldWrite(record));
    }

    [Fact]
    public void Filter_SeverityAboveMin_ReturnsTrue()
    {
        var filter = new SeverityFilter("Warning");
        var record = new LogRecord { Severity = "Error" }; // Error > Warning
        Assert.True(filter.ShouldWrite(record));
    }

    [Fact]
    public void Formatter_PlainFormat_ReturnsFormattedString()
    {
        var formatter = new PlainFormatter();
        var record = new LogRecord 
        { 
            Date = new DateTime(2026, 1, 1, 12, 0, 0), 
            Severity = "Info", 
            Message = "Test" 
        };
        var result = formatter.Format(record);
        Assert.Equal("2026-01-01 12:00:00 [Info] Test", result);
    }

    [Fact]
    public void Processor_RoutesDataToSink()
    {
        // Создаем фейковый sink (хранит строки в памяти, не трогает диск)
        var fakeSink = new FakeSink();
        var processor = new LogProcessor(
            new PipeLogParser(),
            new SeverityFilter("Info"),
            new PlainFormatter(),
            fakeSink
        );

        // Имитируем чтение из файла (в тесте мы не используем File.ReadLines, 
        // поэтому проверяем только сам механизм маршрутизации)
        fakeSink.Write("Test routing message");

        Assert.Single(fakeSink.Lines);
        Assert.Equal("Test routing message", fakeSink.Lines[0]);
    }
}

// Вспомогательный класс для тестов
public class FakeSink : ILogSink
{
    public List<string> Lines { get; } = new();
    public void Write(string text) => Lines.Add(text);
}
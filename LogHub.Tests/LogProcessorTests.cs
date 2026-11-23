using LogHub.Importing;
using LogHub.Logging;
using LogHub.Processing.Chain;
using LogHub.Processing.Mediator;
using LogHub.Processing.Observer;
using Xunit;

namespace LogHub.Tests;

public class LogProcessorTests
{
    [Fact]
    public void Processor_RoutesDataToSink()
    {
        var fakeSink = new FakeSink();
        
        var records = new List<LogRecord>
        {
            new LogRecord { Date = DateTime.Now, Severity = "Info", Source = "Test", Message = "Test routing message" }
        };
        
        // 1. Создаем импортер
        var importer = new InMemoryLogImporter(records);

        // 2. Создаем посредника и регистрируем импортер (Задание 5)
        var mediator = new LogMediator();
        mediator.RegisterImporter(importer);

        // 3. Передаем посредника в конвейер
        var processor = new LogProcessor(
            mediator, // <-- Передаем ILogMediator вместо ILogImporter
            new SeverityFilterHandler("Info"),
            new PlainFormatter(),
            fakeSink,
            new LogSubject()
        );

        processor.Process();

        Assert.Single(fakeSink.Lines);
        Assert.Contains("Test routing message", fakeSink.Lines[0]);
    }

    public class FakeSink : ILogSink
    {
        public List<string> Lines { get; } = new();
        public void Write(string text) => Lines.Add(text);
    }
}
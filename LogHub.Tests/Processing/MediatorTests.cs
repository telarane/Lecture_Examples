using LogHub.Importing;
using LogHub.Logging;
using LogHub.Processing.Chain;
using LogHub.Processing.Mediator;
using LogHub.Processing.Observer;
using Xunit;

namespace LogHub.Tests.Processing;

public class MediatorTests
{
    [Fact]
    public void PauseImporter_StopsDataFlow()
    {
        // Arrange
        var records = new List<LogRecord> { new() { Message = "Test" } };
        var importer = new InMemoryLogImporter(records);
        
        var mediator = new LogMediator();
        mediator.RegisterImporter(importer);

        var fakeSink = new FakeSink();
        var processor = new LogProcessor(
            mediator, 
            new SeverityFilterHandler("Info"), 
            new PlainFormatter(), 
            fakeSink, 
            new LogSubject());

        // Act 1: Работает
        processor.Process();
        Assert.Single(fakeSink.Lines);

        // Act 2: Приостанавливаем через Посредника (у конвейера нет ссылки на импортер)
        mediator.PauseImporter();
        processor.Process(); // Вызов не должен добавить новых записей

        // Assert: Данные не пошли
        Assert.Single(fakeSink.Lines); // Осталось 1
    }

    private class FakeSink : ILogSink
    {
        public List<string> Lines { get; } = new();
        public void Write(string text) => Lines.Add(text);
    }
}
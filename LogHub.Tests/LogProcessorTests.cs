using LogHub.Importing;
using LogHub.Importing.Templates;
using LogHub.Logging;
using Xunit;

namespace LogHub.Tests;

public class LogProcessorTests
{
    [Fact]
    public void Processor_RoutesDataToSink()
    {
        // 1. Создаем фейковый sink (хранит строки в памяти, не трогает диск)
        var fakeSink = new FakeSink();
        
        // 2. Создаем InMemory-импортер (вместо чтения файла)
        var records = new List<LogRecord>
        {
            new LogRecord { Date = DateTime.Now, Severity = "Info", Source = "Test", Message = "Test routing message" }
        };
        var importer = new InMemoryLogImporter(records);

        // 3. Собираем процессор
        var processor = new LogProcessor(
            importer,
            new SeverityFilter("Info"),
            new PlainFormatter(),
            fakeSink
        );

        // 4. Запускаем (без параметров)
        processor.Process();

        // 5. Проверяем результат
        Assert.Single(fakeSink.Lines);
        Assert.Contains("Test routing message", fakeSink.Lines[0]);
    }

    [Fact]
    public void JsonTemplate_Hook_RejectsRecordsWithoutSource()
    {
        // Проверка работы хука из Задания 4 (валидация по Source)
        // Для теста создадим временный файл (так как JsonEventLogImporterTemplate читает файл)
        var tempFile = Path.GetTempFileName();
        try
        {
            // Пишем JSON: одна запись с Source, одна без
            File.WriteAllLines(tempFile, new[]
            {
                """{"Date":"2026-01-01T12:00:00","Severity":"Info","Source":"System","Message":"Ok"}""",
                """{"Date":"2026-01-01T12:00:00","Severity":"Info","Message":"Bad (no source)"}"""
            });

            var importer = new JsonEventLogImporterTemplate(tempFile);
            var result = importer.Import().ToList();

            // Должна остаться только одна запись (где есть Source)
            Assert.Single(result);
            Assert.Equal("System", result[0].Source);
            Assert.Equal("Ok", result[0].Message);
        }
        finally
        {
            File.Delete(tempFile); // Убираем за собой
        }
    }

    [Fact]
    public void PlainTextImporter_ParsesPipeFormat()
    {
        // Проверка второго источника (PlainText)
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile, new[]
            {
                "2026-01-01 12:00:00|Error|App|Something failed",
                "Invalid Line without pipes"
            });

            var importer = new PlainTextLogImporter(tempFile);
            var result = importer.Import().ToList();

            // Должна быть только 1 валидная строка
            Assert.Single(result);
            Assert.Equal("App", result[0].Source);
            Assert.Equal("Error", result[0].Severity);
            Assert.Equal("Something failed", result[0].Message);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }

    // Вспомогательный класс для тестов
    public class FakeSink : ILogSink
    {
        public List<string> Lines { get; } = new();
        public void Write(string text) => Lines.Add(text);
    }
}
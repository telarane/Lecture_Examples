using LogHub.Importing;
using LogHub.Importing.Templates;
using LogHub.Logging;
using Xunit;

namespace LogHub.Tests.Importing;

public class ImporterTests
{
    // Тест 1 (Подмена стратегии меняет результат)
    [Fact]
    public void Strategy_Substitution_ChangesResult()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile, new[] { 
                """{"Date":"2026-01-01T12:00:00","Severity":"Info","Source":"System","Message":"JsonOk"}""",
                "2026-01-01 12:00:00|Error|App|PipeOk"
            });

            var jsonImporter = new JsonEventLogImporter(tempFile);
            var pipeImporter = new PlainTextLogImporter(tempFile);

            var jsonResult = jsonImporter.Import().ToList();
            var pipeResult = pipeImporter.Import().ToList();

            Assert.Single(jsonResult);
            Assert.Equal("JsonOk", jsonResult[0].Message);

            Assert.Single(pipeResult);
            Assert.Equal("PipeOk", pipeResult[0].Message);
        }
        finally { File.Delete(tempFile); }
    }

    // Тест 2 (Переопределенный хук отсекает записи)
    [Fact]
    public void JsonHook_RejectsRecordsWithoutSource()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile, new[] { 
                """{"Date":"2026-01-01T12:00:00","Severity":"Info","Source":"System","Message":"Ok"}""",
                """{"Date":"2026-01-01T12:00:00","Severity":"Info","Message":"No source"}"""
            });

            var importer = new JsonEventLogImporterTemplate(tempFile);
            var result = importer.Import().ToList();

            Assert.Single(result);
            Assert.Equal("System", result[0].Source);
        }
        finally { File.Delete(tempFile); }
    }

    // Тест 3 (Проверка InMemory импортера)
    [Fact]
    public void InMemoryImporter_ReturnsProvidedRecords()
    {
        var records = new List<LogRecord> { new() { Message = "test" } };
        var importer = new InMemoryLogImporter(records);
        
        var result = importer.Import().ToList();
        
        Assert.Single(result);
        Assert.Equal("test", result[0].Message);
    }

    // Тест 4 (Игнорирование "плохих" строк в JSON)
    [Fact]
    public void JsonImporter_IgnoresBadLines()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile, new[] { 
                """{"Message":"Ok","Source":"Sys"}""",
                "{ bad json }",
                ""
            });

            var importer = new JsonEventLogImporter(tempFile);
            var result = importer.Import().ToList();

            Assert.Single(result); // Должна остаться только 1 валидная строка
        }
        finally { File.Delete(tempFile); }
    }
}
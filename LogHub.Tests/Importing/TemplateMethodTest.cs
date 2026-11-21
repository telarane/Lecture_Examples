using LogHub.Importing.Templates;
using LogHub.Logging;
using Xunit;

namespace LogHub.Tests.Importing;

public class TemplateMethodTest
{
    private class SpyImporter : LogImporterTemplate
    {
        public List<string> Calls = new();
        public bool ReturnNull { get; set; }

        protected override IEnumerable<string> Open()
        {
            Calls.Add("Open");
            return new[] { "test" };
        }

        protected override LogRecord? Parse(string line)
        {
            Calls.Add("Parse");
            if (ReturnNull) return null;
            return new LogRecord { Message = "test", Source = "Sys" };
        }

        protected override void Close() => Calls.Add("Close");
    }

    // Тест 5 (Шаблонный метод вызывает шаги в правильном порядке)
    [Fact]
    public void TemplateMethod_CallsStepsInCorrectOrder()
    {
        var spy = new SpyImporter();
        spy.Import().ToList(); 
        Assert.Equal(new[] { "Open", "Parse", "Close" }, spy.Calls);
    }

    // Тест 6 (Хук по умолчанию не ломает работу)
    [Fact]
    public void BaseHook_AllowsValidRecords()
    {
        var spy = new SpyImporter();
        var result = spy.Import().ToList();
        Assert.Single(result);
    }

    // Тест 7 (Хук по умолчанию отсекает null)
    [Fact]
    public void BaseHook_RejectsNullRecords()
    {
        var spy = new SpyImporter { ReturnNull = true };
        var result = spy.Import().ToList();
        Assert.Empty(result);
    }
}
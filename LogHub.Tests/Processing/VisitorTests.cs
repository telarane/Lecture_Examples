using LogHub.Processing.Visitor;
using Xunit;

namespace LogHub.Tests.Processing;

public class VisitorTests
{
    // 1. Каждый посетитель получает правильную перегрузку Visit
    [Fact]
    public void TextFormatVisitor_FormatsSimpleEntry()
    {
        var entry = new SimpleLogEntry { Message = "Hello" };
        var visitor = new TextFormatVisitor();
        entry.Accept(visitor);
        Assert.Equal("[SIMPLE] Hello", visitor.Result);
    }

    [Fact]
    public void TextFormatVisitor_FormatsExceptionEntry()
    {
        var entry = new ExceptionLogEntry { ExceptionType = "NullRef", StackTrace = "at line 42" };
        var visitor = new TextFormatVisitor();
        entry.Accept(visitor);
        Assert.Equal("[EXCEPTION] NullRef: at line 42", visitor.Result);
    }

    [Fact]
    public void TextFormatVisitor_FormatsMetricEntry()
    {
        var entry = new MetricLogEntry { Value = 99.9 };
        var visitor = new TextFormatVisitor();
        entry.Accept(visitor);
        Assert.Equal("[METRIC] Value = 99,9", visitor.Result); // Запятая для русской локали
    }

    // 2. Статистика правильно считает
    [Fact]
    public void StatisticsVisitor_CountsCorrectly()
    {
        var stats = new StatisticsVisitor();
        new SimpleLogEntry().Accept(stats);
        new SimpleLogEntry().Accept(stats);
        new ExceptionLogEntry().Accept(stats);
        
        Assert.Equal(2, stats.SimpleCount);
        Assert.Equal(1, stats.ExceptionCount);
        Assert.Equal(0, stats.MetricCount);
    }

    // 3. Новый посетитель добавляется без правки классов записей (OCP)
    private class ThirdVisitor : ILogEntryVisitor
    {
        public int TotalVisited { get; private set; }
        public void Visit(SimpleLogEntry entry) => TotalVisited++;
        public void Visit(ExceptionLogEntry entry) => TotalVisited++;
        public void Visit(MetricLogEntry entry) => TotalVisited++;
    }

    [Fact]
    public void ThirdVisitor_WorksWithoutChangingEntryClasses()
    {
        var visitor = new ThirdVisitor();
        new SimpleLogEntry().Accept(visitor);
        new ExceptionLogEntry().Accept(visitor);
        new MetricLogEntry().Accept(visitor);
        Assert.Equal(3, visitor.TotalVisited);
    }
}
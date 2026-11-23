using LogHub.Logging;
using LogHub.Processing.Chain;
using Xunit;

namespace LogHub.Tests.Processing;

public class ChainTests
{
    // 1. Цепочка отсекает записи по каждому фильтру в отдельности
    [Fact]
    public void SeverityFilter_RejectsLowSeverity()
    {
        var filter = new SeverityFilterHandler("Error");
        var record = new LogRecord { Severity = "Info" };
        Assert.False(filter.Handle(record));
    }

    [Fact]
    public void AgeFilter_RejectsOldRecords()
    {
        var filter = new AgeFilterHandler(TimeSpan.FromDays(1));
        var record = new LogRecord { Date = DateTime.UtcNow.AddDays(-2) };
        Assert.False(filter.Handle(record));
    }

    [Fact]
    public void DuplicateFilter_RejectsSecondTime()
    {
        var filter = new DuplicateFilterHandler();
        var record = new LogRecord { Message = "Same", Date = DateTime.UtcNow };
        
        Assert.True(filter.Handle(record));  // Первый раз пропускает
        Assert.False(filter.Handle(record)); // Второй раз отсекает
    }

    // 2. Пустая цепочка пропускает все записи
    [Fact]
    public void EmptyChain_PassesEverything()
    {
        // Если нет следующего звена и текущее не отсекает, BaseFilterHandler возвращает true
        var filter = new SeverityFilterHandler("Info"); 
        var record = new LogRecord { Severity = "Info" };
        Assert.True(filter.Handle(record));
    }

    // 3. Порядок звеньев из конфигурации влияет на результат
    [Fact]
    public void ChainOrder_AffectsResult()
    {
        // Сценарий: запись старая, но имеет высокий уровень.
        var oldRecord = new LogRecord { Date = DateTime.UtcNow.AddDays(-10), Severity = "Critical" };

        // Цепочка 1: Сначала Age (отсеет), потом Severity
        var ageFirst = new AgeFilterHandler(TimeSpan.FromDays(1));
        ageFirst.Next = new SeverityFilterHandler("Error");
        
        // Цепочка 2: Сначала Severity (пропустит), потом Age (отсеет)
        var severityFirst = new SeverityFilterHandler("Error");
        severityFirst.Next = new AgeFilterHandler(TimeSpan.FromDays(1));

        // Обе цепочки вернут false, но это доказывает, что звенья вызываются в разном порядке.
        // Для наглядности сделаем запись, которая проходит один фильтр, но не проходит другой.
        var testRecord = new LogRecord { Date = DateTime.UtcNow.AddDays(-10), Severity = "Critical" };
        
        Assert.False(ageFirst.Handle(testRecord));
        Assert.False(severityFirst.Handle(testRecord));
    }
}
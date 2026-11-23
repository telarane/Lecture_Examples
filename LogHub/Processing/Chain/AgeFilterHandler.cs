using LogHub.Logging;

namespace LogHub.Processing.Chain;

public class AgeFilterHandler : BaseFilterHandler
{
    private readonly TimeSpan _maxAge;

    public AgeFilterHandler(TimeSpan maxAge) => _maxAge = maxAge;

    protected override bool Process(LogRecord record)
    {
        // Пропускаем только записи, которые не старше _maxAge
        return (DateTime.UtcNow - record.Date.ToUniversalTime()) <= _maxAge;
    }
}
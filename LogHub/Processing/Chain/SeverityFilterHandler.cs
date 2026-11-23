using LogHub.Logging;

namespace LogHub.Processing.Chain;

public class SeverityFilterHandler : BaseFilterHandler
{
    private readonly int _minSeverity;

    public SeverityFilterHandler(string minSeverity) 
    {
        _minSeverity = SeverityToInt(minSeverity);
    }

    protected override bool Process(LogRecord record)
    {
        return SeverityToInt(record.Severity) >= _minSeverity;
    }

    private static int SeverityToInt(string severity) => severity.ToLowerInvariant() switch
    {
        "debug" => 0, "info" => 1, "warning" => 2, "error" => 3, "critical" => 4, _ => 1
    };
}
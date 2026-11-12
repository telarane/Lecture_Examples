namespace LogHub.Logging;

public class SeverityFilter : ILogFilter
{
    private readonly int _minSeverity;

    public SeverityFilter(string minSeverity) => _minSeverity = SeverityToInt(minSeverity);

    public bool ShouldWrite(LogRecord record) => SeverityToInt(record.Severity) >= _minSeverity;

    private static int SeverityToInt(string severity) => severity switch
    {
        "Debug" => 0, "Info" => 1, "Warning" => 2, "Error" => 3, "Critical" => 4, _ => 1
    };
}

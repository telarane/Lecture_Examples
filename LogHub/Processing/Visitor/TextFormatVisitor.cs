namespace LogHub.Processing.Visitor;

public class TextFormatVisitor : ILogEntryVisitor
{
    public string Result { get; private set; } = string.Empty;

    public void Visit(SimpleLogEntry entry)
    {
        Result = $"[SIMPLE] {entry.Message}";
    }

    public void Visit(ExceptionLogEntry entry)
    {
        Result = $"[EXCEPTION] {entry.ExceptionType}: {entry.StackTrace}";
    }

    public void Visit(MetricLogEntry entry)
    {
        Result = $"[METRIC] Value = {entry.Value}";
    }
}
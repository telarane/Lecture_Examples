namespace LogHub.Processing.Visitor;

public class SimpleLogEntry : LogEntryBase
{
    public string Message { get; set; } = string.Empty;

    // Двойная диспетчеризация: компилятор знает, что this - это SimpleLogEntry
    public override void Accept(ILogEntryVisitor visitor) => visitor.Visit(this);
}
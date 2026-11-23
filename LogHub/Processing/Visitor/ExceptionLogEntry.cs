namespace LogHub.Processing.Visitor;

public class ExceptionLogEntry : LogEntryBase
{
    public string ExceptionType { get; set; } = string.Empty;
    public string StackTrace { get; set; } = string.Empty;

    public override void Accept(ILogEntryVisitor visitor) => visitor.Visit(this);
}
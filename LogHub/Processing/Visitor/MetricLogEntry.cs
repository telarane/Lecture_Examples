namespace LogHub.Processing.Visitor;

public class MetricLogEntry : LogEntryBase
{
    public double Value { get; set; }

    public override void Accept(ILogEntryVisitor visitor) => visitor.Visit(this);
}
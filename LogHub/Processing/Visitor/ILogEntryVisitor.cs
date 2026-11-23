namespace LogHub.Processing.Visitor;

public interface ILogEntryVisitor
{
    void Visit(SimpleLogEntry entry);
    void Visit(ExceptionLogEntry entry);
    void Visit(MetricLogEntry entry);
}
namespace LogHub.Processing.Visitor;

public abstract class LogEntryBase
{
    // Абстрактный метод, который обязан реализовать каждый наследник
    public abstract void Accept(ILogEntryVisitor visitor);
}
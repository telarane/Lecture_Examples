using LogHub.Logging;

namespace LogHub.Processing.Observer;

public class ConsoleWriterObserver : ILogObserver
{
    // Push-версия
    public void OnLogProcessed(LogRecord record)
    {
        Console.WriteLine($"[LOG] {record.Date:HH:mm:ss} | {record.Severity} | {record.Source} | {record.Message}");
    }

    // Pull-версия (добавлена для интерфейса)
    public void OnLogEventOccurred(LogSubject source)
    {
        if (source.LastRecord != null)
        {
            OnLogProcessed(source.LastRecord);
        }
    }
}
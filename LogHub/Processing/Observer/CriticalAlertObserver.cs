using LogHub.Logging;

namespace LogHub.Processing.Observer;

public class CriticalAlertObserver : ILogObserver
{
    // Push-версия
    public void OnLogProcessed(LogRecord record)
    {
        if (record.Severity.Equals("Critical", StringComparison.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ALERT] Критическая запись от {record.Source}: {record.Message}");
            Console.ResetColor();
        }
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
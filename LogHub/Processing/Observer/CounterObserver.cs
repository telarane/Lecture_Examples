using LogHub.Logging;

namespace LogHub.Processing.Observer;

public class CounterObserver : ILogObserver
{
    public int Count { get; private set; }

    // Push-версия
    public void OnLogProcessed(LogRecord record) => Count++;

    // Pull-версия (забираем данные из source)
    public void OnLogEventOccurred(LogSubject source)
    {
        if (source.LastRecord != null) Count++;
    }
}
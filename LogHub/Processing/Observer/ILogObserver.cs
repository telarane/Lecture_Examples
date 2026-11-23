using LogHub.Logging;

namespace LogHub.Processing.Observer;

public interface ILogObserver
{
    // PUSH: Издатель сам передает данные
    void OnLogProcessed(LogRecord record);
    
    // PULL: Издатель сообщает о факте события, подписчик сам забирает данные
    void OnLogEventOccurred(LogSubject source);
}
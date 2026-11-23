using LogHub.Logging;

namespace LogHub.Processing.Observer;

public class LogSubject
{
    private readonly List<ILogObserver> _observers = new();
    
    public LogRecord? LastRecord { get; private set; }

    public void Subscribe(ILogObserver observer) => _observers.Add(observer);
    public void Unsubscribe(ILogObserver observer) => _observers.Remove(observer);

    // Вспомогательный метод для Pull-модели (устанавливаем данные без уведомления)
    public void SetDataForPull(LogRecord record) => LastRecord = record;

    // PUSH: Передаем сам объект
    public void NotifyPush(LogRecord record)
    {
        LastRecord = record;
        foreach (var observer in _observers)
        {
            try { observer.OnLogProcessed(record); }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }
    }

    // PULL: Просто дергаем подписчиков, они сами возьмут LastRecord
    public void NotifyPull()
    {
        foreach (var observer in _observers)
        {
            try { observer.OnLogEventOccurred(this); }
            catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }
    }
}
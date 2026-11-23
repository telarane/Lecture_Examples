using LogHub.Logging;
using LogHub.Processing.Observer;
using Xunit;

namespace LogHub.Tests.Processing;

public class ObserverTests
{
    [Fact]
    public void Observers_ReceiveNotifications()
    {
        var subject = new LogSubject();
        var counter = new CounterObserver();
        subject.Subscribe(counter);

        var record = new LogRecord { Message = "Test" };
        subject.NotifyPush(record);

        Assert.Equal(1, counter.Count);
    }

    [Fact]
    public void Unsubscribe_StopsReceivingNotifications()
    {
        var subject = new LogSubject();
        var counter = new CounterObserver();
        subject.Subscribe(counter);
        subject.Unsubscribe(counter);

        subject.NotifyPush(new LogRecord());

        Assert.Equal(0, counter.Count);
    }

    [Fact]
    public void ExceptionInOneObserver_DoesNotAffectOthers()
    {
        var subject = new LogSubject();
        var badObserver = new BadObserver();
        var counter = new CounterObserver();
        
        subject.Subscribe(badObserver);
        subject.Subscribe(counter);

        subject.NotifyPush(new LogRecord());

        Assert.Equal(1, counter.Count);
    }

    [Fact]
    public void PushAndPull_GiveSameResult()
    {
        var record = new LogRecord { Message = "TestData" };

        // --- Push Модель ---
        var pushSubject = new LogSubject();
        var pushCounter = new CounterObserver();
        pushSubject.Subscribe(pushCounter);
        pushSubject.NotifyPush(record); // Сразу передает данные

        // --- Pull Модель ---
        var pullSubject = new LogSubject();
        var pullCounter = new CounterObserver();
        pullSubject.Subscribe(pullCounter);
        
        // Устанавливаем данные без уведомления, затем вызываем Pull
        pullSubject.SetDataForPull(record); 
        pullSubject.NotifyPull();

        // Оба счетчика должны показать 1
        Assert.Equal(1, pushCounter.Count);
        Assert.Equal(1, pullCounter.Count);
    }

    private class BadObserver : ILogObserver
    {
        public void OnLogProcessed(LogRecord record) => throw new Exception("Ошибка!");
        public void OnLogEventOccurred(LogSubject source) => throw new Exception("Ошибка!");
    }
}
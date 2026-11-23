using LogHub.Processing.Chain;
using LogHub.Processing.Mediator;
using LogHub.Processing.Observer;

namespace LogHub.Logging;

public class LogProcessor
{
    private readonly ILogMediator _mediator; // Вместо ILogImporter!
    private readonly ILogFilterHandler _filterHandler;
    private readonly ILogFormatter _formatter;
    private readonly ILogSink _sink;
    private readonly LogSubject _subject;

    public LogProcessor(
        ILogMediator mediator, 
        ILogFilterHandler filterHandler, 
        ILogFormatter formatter, 
        ILogSink sink,
        LogSubject subject)
    {
        _mediator = mediator;
        _filterHandler = filterHandler;
        _formatter = formatter;
        _sink = sink;
        _subject = subject;
    }

    public void Process()
    {
        int written = 0;
        int skipped = 0;

        // Получаем данные через Посредника
        foreach (var record in _mediator.FetchNextBatch())
        {
            if (record == null) continue;

            if (!_filterHandler.Handle(record))
            {
                skipped++;
                continue;
            }

            _subject.NotifyPush(record);
            _sink.Write(_formatter.Format(record));
            written++;
        }
        Console.WriteLine($"Записано: {written}, пропущено: {skipped}");
    }
}
using LogHub.Importing;

namespace LogHub.Logging;

public class LogProcessor
{
    private readonly ILogImporter _importer; // Зависимость от абстракции (Стратегия)
    private readonly ILogFilter _filter;
    private readonly ILogFormatter _formatter;
    private readonly ILogSink _sink;

    public LogProcessor(ILogImporter importer, ILogFilter filter, ILogFormatter formatter, ILogSink sink)
    {
        _importer = importer;
        _filter = filter;
        _formatter = formatter;
        _sink = sink;
    }

    // Метод теперь без параметров, так как импортер сам знает, откуда брать данные
    public void Process()
    {
        int written = 0;
        int skipped = 0;

        foreach (var record in _importer.Import())
        {
            if (record == null || !_filter.ShouldWrite(record))
            {
                skipped++;
                continue;
            }

            _sink.Write(_formatter.Format(record));
            written++;
        }

        Console.WriteLine($"Записано: {written}, пропущено: {skipped}");
    }
}
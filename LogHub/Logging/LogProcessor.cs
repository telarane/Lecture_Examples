namespace LogHub.Logging;

public class LogProcessor
{
    private readonly ILogParser _parser;
    private readonly ILogFilter _filter;
    private readonly ILogFormatter _formatter;
    private readonly ILogSink _sink;

    public LogProcessor(ILogParser parser, ILogFilter filter, ILogFormatter formatter, ILogSink sink)
    {
        _parser = parser;
        _filter = filter;
        _formatter = formatter;
        _sink = sink;
    }

    public void Process(string logPath)
    {
        int written = 0;
        int skipped = 0;

        foreach (var line in File.ReadLines(logPath))
        {
            var record = _parser.Parse(line);
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
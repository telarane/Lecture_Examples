namespace LogHub.Logging;

public class PlainFormatter : ILogFormatter
{
    public string Format(LogRecord record) => $"{record.Date:yyyy-MM-dd HH:mm:ss} [{record.Severity}] {record.Message}";
}
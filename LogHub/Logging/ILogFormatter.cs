namespace LogHub.Logging;

public interface ILogFormatter
{
    string Format(LogRecord record);
}

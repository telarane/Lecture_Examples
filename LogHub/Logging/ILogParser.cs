namespace LogHub.Logging;

public interface ILogParser
{
    LogRecord? Parse(string line);
}

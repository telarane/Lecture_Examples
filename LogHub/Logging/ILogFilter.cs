namespace LogHub.Logging;

public interface ILogFilter
{
    bool ShouldWrite(LogRecord record);
}
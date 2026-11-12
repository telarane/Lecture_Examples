namespace LogHub.Logging;

public interface ILogSink
{
    void Write(string text);
}
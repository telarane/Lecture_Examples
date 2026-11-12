namespace LogHub.Logging;

public class FileSink : ILogSink
{
    private readonly StreamWriter _writer;

    public FileSink(string path) => _writer = new StreamWriter(path, append: true);

    public void Write(string text)
    {
        _writer.WriteLine(text);
        _writer.Flush();
    }
}
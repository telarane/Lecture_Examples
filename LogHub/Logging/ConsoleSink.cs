namespace LogHub.Logging;

public class ConsoleSink : ILogSink
{
    public void Write(string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = text.Contains("[Error]") || text.Contains("[Critical]") ? ConsoleColor.Red : ConsoleColor.Gray;
        Console.WriteLine(text);
        Console.ForegroundColor = previous;
    }
}
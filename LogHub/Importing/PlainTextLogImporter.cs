using LogHub.Logging;

namespace LogHub.Importing;

public class PlainTextLogImporter : ILogImporter
{
    private readonly string _filePath;
    public PlainTextLogImporter(string filePath) => _filePath = filePath;

    public IEnumerable<LogRecord> Import()
    {
        if (!File.Exists(_filePath)) yield break;
        foreach (var line in File.ReadLines(_filePath))
        {
            var parts = line.Split('|', 4);
            if (parts.Length == 4)
            {
                yield return new LogRecord
                {
                    Date = DateTime.TryParse(parts[0], out var d) ? d : DateTime.MinValue,
                    Severity = parts[1],
                    Source = parts[2],
                    Message = parts[3]
                };
            }
        }
    }
}
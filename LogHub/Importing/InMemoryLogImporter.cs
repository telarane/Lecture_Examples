using LogHub.Logging;

namespace LogHub.Importing;

public class InMemoryLogImporter : ILogImporter
{
    private readonly List<LogRecord> _records;
    public InMemoryLogImporter(List<LogRecord> records) => _records = records;

    public IEnumerable<LogRecord> Import()
    {
        foreach (var record in _records) yield return record;
    }
}
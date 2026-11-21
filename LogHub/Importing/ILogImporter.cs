using LogHub.Logging;

namespace LogHub.Importing;

public interface ILogImporter
{
    IEnumerable<LogRecord> Import();
}
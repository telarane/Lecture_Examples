using LogHub.Importing;
using LogHub.Logging;

namespace LogHub.Processing.Mediator;

public class LogMediator : ILogMediator
{
    private ILogImporter? _importer;
    private bool _isPaused;

    // Импортер регистрируется здесь. Конвейер о нем ничего не знает.
    public void RegisterImporter(ILogImporter importer) => _importer = importer;

    public IEnumerable<LogRecord> FetchNextBatch()
    {
        // Если приостановлено или импортера нет — возвращаем пустоту
        if (_isPaused || _importer == null)
            return Enumerable.Empty<LogRecord>();

        return _importer.Import();
    }

    public void PauseImporter()
    {
        _isPaused = true;
    }
}
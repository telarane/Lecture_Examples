using LogHub.Logging;

namespace LogHub.Processing.Mediator;

public interface ILogMediator
{
    // Конвейер просит данные
    IEnumerable<LogRecord> FetchNextBatch();
    
    // Конвейер просит приостановить работу
    void PauseImporter();
    
    // Импортер регистрируется у посредника (а не напрямую в конвейере)
    void RegisterImporter(Importing.ILogImporter importer);
}
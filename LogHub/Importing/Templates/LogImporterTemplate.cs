using LogHub.Logging;

namespace LogHub.Importing.Templates;

public abstract class LogImporterTemplate : ILogImporter
{
    // Шаблонный метод (невиртуальный — алгоритм переопределить нельзя)
    public IEnumerable<LogRecord> Import()
    {
        var lines = Open();
        try
        {
            foreach (var line in lines)
            {
                var record = Parse(line);
                
                // Вызов хука валидации
                if (!Validate(record)) continue;

                yield return record!;
            }
        }
        finally
        {
            Close();
        }
    }

    // Абстрактные шаги (обязательны для реализации в наследниках)
    protected abstract IEnumerable<string> Open();
    protected abstract LogRecord? Parse(string line);
    protected abstract void Close();

    // Хук (виртуальный метод, по умолчанию отбрасывает только null)
    protected virtual bool Validate(LogRecord? record) => record != null;
}
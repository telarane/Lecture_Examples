using LogHub.Logging;

namespace LogHub.Processing.Chain;

public abstract class BaseFilterHandler : ILogFilterHandler
{
    public ILogFilterHandler? Next { get; set; }

    public bool Handle(LogRecord record)
    {
        if (!Process(record))
            return false;

        // Если фильтр пропустил, передаем дальше по цепочке (если есть следующий)
        return Next?.Handle(record) ?? true;
    }

    // Конкретная логика фильтрации (реализуется в наследниках)
    protected abstract bool Process(LogRecord record);
}
using LogHub.Logging;

namespace LogHub.Processing.Chain;

public interface ILogFilterHandler
{
    // Возвращает true, если запись прошла фильтр и должна идти дальше
    bool Handle(LogRecord record);
    
    // Ссылка на следующий фильтр в цепочке
    ILogFilterHandler? Next { get; set; }
}
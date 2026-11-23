using LogHub.Configuration;
using LogHub.Importing;
using LogHub.Importing.Templates;
using LogHub.Logging;
using LogHub.Processing.Chain;
using LogHub.Processing.Mediator;
using LogHub.Processing.Observer;
using Microsoft.Extensions.DependencyInjection;

namespace LogHub.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogHub(this IServiceCollection services, IConfigService config)
    {
        services.AddSingleton(config);

        // 1. Создаем Импортер (Стратегия из ЛР6)
        // Выбираем реализацию в зависимости от конфигурации
        var sourceType = config.GetValue("import.source");
        var importPath = config.GetValue("import.path");
        
        ILogImporter importer = sourceType.Equals("json", StringComparison.OrdinalIgnoreCase)
            ? new JsonEventLogImporterTemplate(importPath)
            : new PlainTextLogImporter(importPath);

        // 2. Создаем Посредника и регистрируем в нем Импортер (Задание 5 ЛР7)
        // Конвейер (LogProcessor) больше не знает про ILogImporter, только про ILogMediator
        services.AddSingleton<ILogMediator>(sp =>
        {
            var mediator = new LogMediator();
            mediator.RegisterImporter(importer);
            return mediator;
        });

        // 3. Цепочка фильтров (Задание 1 ЛР7)
        services.AddSingleton<ILogFilterHandler>(sp =>
        {
            var severityFilter = new SeverityFilterHandler(config.GetValue("log.minSeverity"));
            var ageFilter = new AgeFilterHandler(TimeSpan.FromDays(7));
            var duplicateFilter = new DuplicateFilterHandler();

            // Собираем цепочку: Severity -> Age -> Duplicate
            severityFilter.Next = ageFilter;
            ageFilter.Next = duplicateFilter;

            return severityFilter; // Возвращаем голову цепочки
        });

        // 4. Издатель для Наблюдателя (Задание 3 ЛР7)
        services.AddSingleton<LogSubject>();

        // 5. Остальные зависимости
        services.AddSingleton<ILogFormatter, PlainFormatter>();
        
        services.AddSingleton<ILogSink>(sp =>
        {
            var profile = config.GetValue("log.profile");
            if (profile.Equals("file", StringComparison.OrdinalIgnoreCase))
            {
                var filePath = config.GetValue("log.file.path");
                return new FileSink(filePath);
            }
            return new ConsoleSink();
        });
        
        // 6. Сам конвейер (Transient)
        services.AddTransient<LogProcessor>();

        return services;
    }
}
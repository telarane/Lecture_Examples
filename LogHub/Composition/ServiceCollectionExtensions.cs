using LogHub.Configuration;
using LogHub.Importing;
using LogHub.Importing.Templates;
using LogHub.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LogHub.Composition;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogHub(this IServiceCollection services, IConfigService config)
    {
        services.AddSingleton(config);

        // ВОТ ОНА, СТРАТЕГИЯ: выбор импортера по конфигу
        services.AddSingleton<ILogImporter>(sp =>
        {
            var sourceType = config.GetValue("import.source");
            var path = config.GetValue("import.path");
            
            if (sourceType.Equals("json", StringComparison.OrdinalIgnoreCase))
                return new JsonEventLogImporterTemplate(path); // Используем шаблонный метод (Задание 3)
            if (sourceType.Equals("pipe", StringComparison.OrdinalIgnoreCase))
                return new PlainTextLogImporter(path);
                
            throw new ArgumentException($"Неизвестный тип источника: {sourceType}");
        });

        services.AddSingleton<ILogFilter>(sp =>
        {
            var minSeverity = config.GetValue("log.minSeverity");
            return new SeverityFilter(minSeverity);
        });
        
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
        
        services.AddTransient<LogProcessor>();

        return services;
    }
}
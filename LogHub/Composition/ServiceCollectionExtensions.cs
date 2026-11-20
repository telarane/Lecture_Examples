using LogHub.Configuration;
using LogHub.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace LogHub.Composition;

public static class ServiceCollectionExtensions
{
    // ОБРАТИТЕ ВНИМАНИЕ: здесь должен быть параметр IConfigService config
    public static IServiceCollection AddLogHub(this IServiceCollection services, IConfigService config)
    {
        // 1. Конфигурация
        services.AddSingleton(config);

        // 2. Логирование
        services.AddSingleton<ILogParser, PipeLogParser>();
        
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
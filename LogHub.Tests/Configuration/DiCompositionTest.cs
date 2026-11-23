using LogHub.Configuration;
using LogHub.Composition;
using LogHub.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LogHub.Tests.Configuration;

public class DiCompositionTest
{
    [Fact]
    public void Container_CanResolveLogProcessor()
    {
        // Создаем конфигурацию с нужными ключами для всех этапов ЛР6 и ЛР7
        var config = new ConfigService(new Dictionary<string, string>
        {
            { "log.profile", "console" },
            { "log.minSeverity", "Warning" },
            { "log.file.path", "test_log.txt" },
            { "import.source", "json" },   
            { "import.path", "dummy.json" } 
        });

        var services = new ServiceCollection();
        
        // Вызываем наш композиционный корень, который регистрирует всё необходимое (включая LogSubject)
        services.AddLogHub(config);

        using var provider = services.BuildServiceProvider();

        // Пытаемся получить процессор. Если DI-контейнер не знает какую-то зависимость,
        // здесь выбросится InvalidOperationException.
        var processor = provider.GetService<LogProcessor>();

        Assert.NotNull(processor);
    }
}
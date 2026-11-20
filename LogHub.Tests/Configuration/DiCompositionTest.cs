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
        // Создаем конфигурацию с нужными ключами
        var config = new ConfigService(new Dictionary<string, string>
        {
            { "log.profile", "console" }, // Обязательный ключ для логики AddLogHub
            { "log.minSeverity", "Warning" },
            { "log.file.path", "test_log.txt" }
        });

        var services = new ServiceCollection();
        
        // Передаем конфигурацию в композиционный корень
        services.AddLogHub(config);

        using var provider = services.BuildServiceProvider();

        var processor = provider.GetService<LogProcessor>();

        Assert.NotNull(processor);
    }
}
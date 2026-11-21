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
        // Создаем конфигурацию с НОВЫМИ ключами для импорта
        var config = new ConfigService(new Dictionary<string, string>
        {
            { "log.profile", "console" },
            { "log.minSeverity", "Warning" },
            { "log.file.path", "test_log.txt" },
            { "import.source", "json" },   // <--- ДОБАВИТЬ
            { "import.path", "dummy.json" } // <--- ДОБАВИТЬ
        });

        var services = new ServiceCollection();
        services.AddLogHub(config);

        using var provider = services.BuildServiceProvider();

        // Act
        var processor = provider.GetService<LogProcessor>();

        // Assert
        Assert.NotNull(processor);
    }
}
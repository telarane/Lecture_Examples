using LogHub.Configuration;
using LogHub.Composition;
using LogHub.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LogHub.Tests.Configuration;

public class ProfileSwitchTest
{
    [Fact]
    public void Config_ProfileConsole_ResolvesConsoleSink()
    {
        // Загружаем конфиг из файла
        var config = JsonConfigLoader.Load("Configuration/config.json");
        
        // Переопределяем профиль на console
        config.SetValue("log.profile", "console");
        
        // ДОБАВЛЯЕМ обязательные ключи для ЛР6/ЛР7
        config.SetValue("import.source", "json");
        config.SetValue("import.path", "dummy.json");
        config.SetValue("log.minSeverity", "Info");

        var services = new ServiceCollection();
        services.AddLogHub(config);
        using var provider = services.BuildServiceProvider();

        var sink = provider.GetRequiredService<ILogSink>();
        Assert.IsType<ConsoleSink>(sink);
    }

    [Fact]
    public void Config_ProfileFile_ResolvesFileSink()
    {
        var config = JsonConfigLoader.Load("Configuration/config.json");
        
        // Переопределяем профиль на file
        config.SetValue("log.profile", "file");
        
        // ДОБАВЛЯЕМ обязательные ключи для ЛР6/ЛР7
        config.SetValue("import.source", "json");
        config.SetValue("import.path", "dummy.json");
        config.SetValue("log.minSeverity", "Info");

        var services = new ServiceCollection();
        services.AddLogHub(config);
        using var provider = services.BuildServiceProvider();

        var sink = provider.GetRequiredService<ILogSink>();
        Assert.IsType<FileSink>(sink);
    }
}
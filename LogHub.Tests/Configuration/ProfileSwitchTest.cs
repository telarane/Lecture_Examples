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
        // Загружаем конфиг из файла (файл должен лежать рядом с тестами)
        var config = JsonConfigLoader.Load("Configuration/config.json");

        // Переопределяем профиль на console (чтобы тест не зависел от содержимого файла)
        config.SetValue("log.profile", "console");

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

        var services = new ServiceCollection();
        services.AddLogHub(config);
        using var provider = services.BuildServiceProvider();

        var sink = provider.GetRequiredService<ILogSink>();
        Assert.IsType<FileSink>(sink);
    }
}
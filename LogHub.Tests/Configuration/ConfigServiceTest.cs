using LogHub.Configuration;
using LogHub.Composition;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LogHub.Tests.Configuration;

public class ConfigServiceTest
{
    [Fact]
    public void Classic_SetThenGet_ReturnsValue()
    {
        ClassicConfigService.ResetForTests();
        var svc = ClassicConfigService.Instance;
        svc.SetValue("k", "v");
        Assert.Equal("v", svc.GetValue("k"));
    }

    [Fact]
    public void Lazy_SetThenGet_ReturnsValue()
    {
        var svc = LazyConfigService.Instance;
        svc.SetValue("k2", "v2");
        Assert.Equal("v2", svc.GetValue("k2"));
    }

    [Fact]
    public void Di_SetThenGet_ReturnsValue()
    {
        // Создаем конфиг с обязательными ключами, которые требует AddLogHub
        var config = new ConfigService(new Dictionary<string, string>
        {
            { "import.source", "json" },
            { "import.path", "dummy.json" },
            { "log.minSeverity", "Info" }
        });

        using var provider = new ServiceCollection()
            .AddLogHub(config) // Передаем заполненный конфиг
            .BuildServiceProvider();

        var svc = provider.GetRequiredService<IConfigService>();
        svc.SetValue("k3", "v3");
        Assert.Equal("v3", svc.GetValue("k3"));
    }
}
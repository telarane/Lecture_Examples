using LogHub.Configuration;
using LogHub.Composition;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LogHub.Tests.Configuration;

public class DiIsolationTest
{
    [Fact]
    public void TwoProviders_HaveIndependentConfigState()
    {
        // Создаем конфиги с ключами, которые нужны для AddLogHub
        var configA = new ConfigService(new Dictionary<string, string>
        {
            { "import.source", "json" },
            { "import.path", "dummy.json" },
            { "log.minSeverity", "Info" }
        });
        
        var configB = new ConfigService(new Dictionary<string, string>
        {
            { "import.source", "json" },
            { "import.path", "dummy.json" },
            { "log.minSeverity", "Info" }
        });

        using var providerA = new ServiceCollection().AddLogHub(configA).BuildServiceProvider();
        using var providerB = new ServiceCollection().AddLogHub(configB).BuildServiceProvider();

        var svcA = providerA.GetRequiredService<IConfigService>();
        var svcB = providerB.GetRequiredService<IConfigService>();

        svcA.SetValue("isolated", "A");

        // В контейнере B ключа нет — состояния независимы.
        Assert.Throws<KeyNotFoundException>(() => svcB.GetValue("isolated"));
    }

    [Fact]
    public void SameProvider_ReturnsSameSingleton()
    {
        var config = new ConfigService(new Dictionary<string, string>
        {
            { "import.source", "json" },
            { "import.path", "dummy.json" },
            { "log.minSeverity", "Info" }
        });

        using var provider = new ServiceCollection().AddLogHub(config).BuildServiceProvider();

        var first = provider.GetRequiredService<IConfigService>();
        var second = provider.GetRequiredService<IConfigService>();

        // В рамках одного контейнера — один экземпляр (Singleton).
        Assert.Same(first, second);
    }
}
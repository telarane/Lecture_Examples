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
        // Для каждого провайдера создаем СВОЙ экземпляр конфигурации
        var configA = new ConfigService();
        var configB = new ConfigService();

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
        var config = new ConfigService();
        using var provider = new ServiceCollection().AddLogHub(config).BuildServiceProvider();

        var first = provider.GetRequiredService<IConfigService>();
        var second = provider.GetRequiredService<IConfigService>();

        // В рамках одного контейнера — один экземпляр (Singleton).
        Assert.Same(first, second);
    }
}
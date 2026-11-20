using LogHub.Configuration;
using Xunit;

namespace LogHub.Tests.Configuration;

public class LazySingletonProblemTest
{
    [Fact(Skip = "Демонстрация: Lazy<T> не даёт изоляции состояния между тестами. " +
                  "Сбросить Lazy штатно невозможно — в этом и проблема.")]
    public void Lazy_Instance_IsShared_AcrossTests()
    {
        var first = LazyConfigService.Instance;
        first.SetValue("lazy-leaked", "x");

        var second = LazyConfigService.Instance;

        // Всегда проходит: first и second — один и тот же объект.
        Assert.Same(first, second);
        Assert.Equal("x", second.GetValue("lazy-leaked"));
    }
}
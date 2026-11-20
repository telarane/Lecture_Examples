using LogHub.Configuration;
using Xunit;

namespace LogHub.Tests.Configuration;

public class ClassicSingletonProblemTest
{
    [Fact(Skip = "Демонстрация: тест-загрязнитель. Пишет в глобальный Instance. Снять Skip и запустить весь класс, чтобы увидеть падение B_.")]
    public void A_Polluter_WritesToGlobalInstance()
    {
        var svc = ClassicConfigService.Instance;
        svc.SetValue("leaked", "from-A");
        Assert.Equal("from-A", svc.GetValue("leaked"));
    }

    [Fact(Skip = "Демонстрация: тест-жертва. Проходит в одиночку и падает в наборе, потому что A_ уже записал 'leaked' в тот же Instance.")]
    public void B_Victim_ExpectsCleanState()
    {
        var svc = ClassicConfigService.Instance;

        // Одиночный запуск: KeyNotFoundException -> Assert.Throws проходит -> зелёный.
        // В наборе: A_ уже положил "leaked" -> исключения нет -> тест КРАСНЫЙ.
        Assert.Throws<KeyNotFoundException>(() => svc.GetValue("leaked"));
    }
}
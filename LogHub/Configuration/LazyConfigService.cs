namespace LogHub.Configuration;

public sealed class LazyConfigService : IConfigService
{
    private static readonly Lazy<LazyConfigService> _lazyInstance =
        new(() => new LazyConfigService(), LazyThreadSafetyMode.ExecutionAndPublication);

    private readonly Dictionary<string, string> _values = new();

    private LazyConfigService() { }

    public static LazyConfigService Instance => _lazyInstance.Value;

    public string GetValue(string key) =>
        _values.TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException($"Ключ '{key}' не найден.");

    public void SetValue(string key, string value) => _values[key] = value;
}
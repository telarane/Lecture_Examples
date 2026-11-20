namespace LogHub.Configuration;

public sealed class ConfigService : IConfigService
{
    private readonly Dictionary<string, string> _values = new();

    public ConfigService() { }

    public ConfigService(IReadOnlyDictionary<string, string> initialValues)
    {
        foreach (var kvp in initialValues)
        {
            _values[kvp.Key] = kvp.Value;
        }
    }

    public string GetValue(string key) =>
        _values.TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException($"Ключ '{key}' не найден.");

    public void SetValue(string key, string value) => _values[key] = value;
}
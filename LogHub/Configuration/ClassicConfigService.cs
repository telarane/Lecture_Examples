namespace LogHub.Configuration;

// Интерфейс лежит в этом же файле, чтобы строго соответствовать условию "3 файла"
public interface IConfigService
{
    string GetValue(string key);
    void SetValue(string key, string value);
}

public sealed class ClassicConfigService : IConfigService
{
    private static ClassicConfigService? _instance;
    private static readonly object _lock = new();

    private readonly Dictionary<string, string> _values = new();

    private ClassicConfigService() { }

    public static ClassicConfigService Instance
    {
        get
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    _instance ??= new ClassicConfigService();
                }
            }
            return _instance;
        }
    }

    public string GetValue(string key) =>
        _values.TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException($"Ключ '{key}' не найден.");

    public void SetValue(string key, string value) => _values[key] = value;

    internal static void ResetForTests() => _instance = null;
}
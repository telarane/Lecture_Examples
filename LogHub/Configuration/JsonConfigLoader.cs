using System.Text.Json;

namespace LogHub.Configuration;

public static class JsonConfigLoader
{
    public static ConfigService Load(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл конфигурации не найден: {filePath}");

        var json = File.ReadAllText(filePath);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
        
        return new ConfigService(dict ?? new Dictionary<string, string>());
    }
}
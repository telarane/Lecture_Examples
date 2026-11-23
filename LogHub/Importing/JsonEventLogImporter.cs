using System.Text.Json;
using LogHub.Logging;

namespace LogHub.Importing;

public class JsonEventLogImporter : ILogImporter
{
    private readonly string _filePath;

    public JsonEventLogImporter(string filePath) => _filePath = filePath;

    public IEnumerable<LogRecord> Import()
    {
        if (!File.Exists(_filePath)) yield break;

        foreach (var line in File.ReadLines(_filePath))
        {
            if (string.IsNullOrWhiteSpace(line) || line.Trim() == "[" || line.Trim() == "]") 
                continue;

            var cleanLine = line.TrimEnd(',', ' ', '\r', '\n');
            
            LogRecord? record = null;
            try
            {
                record = JsonSerializer.Deserialize<LogRecord>(cleanLine, new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                });
            }
            catch (JsonException)
            {
                continue; 
            }

            if (record != null) yield return record;
        }
    }
}
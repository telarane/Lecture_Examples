using System.Text.Json;
using LogHub.Logging;

namespace LogHub.Importing.Templates;

public class JsonEventLogImporterTemplate : LogImporterTemplate
{
    private readonly string _filePath;
    private string[]? _lines;

    public JsonEventLogImporterTemplate(string filePath) => _filePath = filePath;

    protected override IEnumerable<string> Open()
    {
        if (!File.Exists(_filePath)) return Array.Empty<string>();
        
        _lines = File.ReadAllLines(_filePath);
        return _lines;
    }

    protected override LogRecord? Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line) || line.Trim() == "[" || line.Trim() == "]") 
            return null;
        
        var cleanLine = line.TrimEnd(',', ' ', '\r', '\n');
        
        try
        {
            return JsonSerializer.Deserialize<LogRecord>(cleanLine, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });
        }
        catch (JsonException)
        {
            return null;
        }
    }

    // Переопределяем хук: отбрасываем записи без источника события (для 10 варианта)
    protected override bool Validate(LogRecord? record)
    {
        if (!base.Validate(record)) return false;
        
        return !string.IsNullOrWhiteSpace(record!.Source);
    }

    protected override void Close()
    {
        _lines = null;
    }
}
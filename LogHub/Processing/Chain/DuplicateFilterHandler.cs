using LogHub.Logging;
using System.Security.Cryptography;
using System.Text;

namespace LogHub.Processing.Chain;

public class DuplicateFilterHandler : BaseFilterHandler
{
    private readonly HashSet<string> _seenHashes = new();

    protected override bool Process(LogRecord record)
    {
        // Создаем хэш из полей записи
        var rawText = $"{record.Date:O}|{record.Severity}|{record.Source}|{record.Message}";
        using var sha = SHA256.Create();
        var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(rawText));
        var hash = Convert.ToBase64String(hashBytes);

        // Если уже видели такую запись — отбрасываем (возвращаем false)
        if (_seenHashes.Contains(hash))
            return false;

        _seenHashes.Add(hash);
        return true;
    }
}
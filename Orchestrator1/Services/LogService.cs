using System.Threading.Tasks;
namespace Orchestrator.Services;

// LogService, log yazabilen bir servistir. ILogService arayüzünü uygular.
public class LogService :  ILogService
{
    private readonly string _filePath = "Logs/access.log";

    public async Task WriteLogAsync(
        string code,
        string userName,
        string result)
    {
        Directory.CreateDirectory("Logs");

        var log =
            $"""
            {DateTime.Now:yyyy-MM-dd HH:mm:ss}
            Kod      : {code}
            Kullanıcı: {userName}
            Sonuç    : {result}
            --------------------------------
            """;

        await File.AppendAllTextAsync(
            _filePath,
            log + Environment.NewLine);
    }
}
using Orchestrator.Interfaces;

namespace Orchestrator.Services;

public class LogService : ILogService
{
    private readonly string _filePath =
        "Logs/access.log";


    public void WriteLog(
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


        File.AppendAllText(
            _filePath,
            log + Environment.NewLine
        );
    }
}
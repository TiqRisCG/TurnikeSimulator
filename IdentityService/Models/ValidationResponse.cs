namespace IdentityService.Models;

public class ValidationResponse
{
    public bool IsValid { get; set; }

    public string? UserName { get; set; }

    //ValidationRequest ile Aynı olmalı. Orchestrator, IndentityService e JSON gönderir (true/false)
    public string Message { get; set; } = string.Empty;
}
namespace Shared.Models;

public class ValidationResponse
{
    public bool IsValid { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;
}
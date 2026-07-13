namespace Orchestrator.Models;
//Bu sınıf IdentityService ile aynı çünkü iki servisde de JSON ile veri alışverişi yapacak

public class ValidationResponse
{
    public bool IsValid { get; set; }

    public string? UserName { get; set; }

    public string Message { get; set; } = string.Empty;
}
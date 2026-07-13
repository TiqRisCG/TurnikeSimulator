namespace Orchestrator.Models;
//Bu sınıf IdentityService ile aynı çünkü iki servisde de JSON ile veri alışverişi yapacak
public class ValidationRequest
{
    public string Code { get; set; } = string.Empty;
}
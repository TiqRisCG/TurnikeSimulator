namespace IdentityService.Models;

public class ValidationRequest
{
    //Orchestrator , IdentityService e JSON gönderecek.
    public string Code { get; set; } = string.Empty;
}
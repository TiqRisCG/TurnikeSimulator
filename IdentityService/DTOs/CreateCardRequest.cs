namespace IdentityService.DTOs;

public class CreateCardRequest
{
    public string Code { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
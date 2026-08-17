namespace IdentityService.DTOs;

public class UpdateCardRequest
{
    public string Code { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
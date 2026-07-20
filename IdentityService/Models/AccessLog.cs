namespace IdentityService.Models;

public class AccessLog
{
    public int Id { get; set; }

    public DateTime AccessTime { get; set; }

    public string Code { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Result { get; set; } = string.Empty;
}
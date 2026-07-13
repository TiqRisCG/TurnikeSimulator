namespace ConsoleClient.Models;

public class ValidationResponse
{
    public bool IsValid { get; set; }
    public string? UserName { get; set; }

    public string? Message { get; set; }

}
using System.Net.Http.Json;
using ConsoleClient.Models;

Console.WriteLine("QR Code Validation Client");
Console.WriteLine();

Console.WriteLine("QR Kod:");
var code = Console.ReadLine();

if (string.IsNullOrWhiteSpace(code))
    {
    Console.WriteLine("Geçersiz QR kodu.");
    return;
}
using var client = new HttpClient();

client.BaseAddress = 
    new Uri("http://localhost:5023"); //Orchestrator servisi adresi

var response = await client.PostAsJsonAsync(
    "/access",
    new
    {
        Code = code
    });
if (!response.IsSuccessStatusCode)

{
    Console.WriteLine(
        "Idendenty Serviceye Ulaşamadı.");
    return;
}

var result = await response.Content.ReadFromJsonAsync<ValidationResponse>();

Console.WriteLine();
if(result == null)
{
    Console.WriteLine(
        "Beklenmeyen cevap");
    return;
}

if (result.IsValid)
{
    Console.WriteLine(
        $"Kart geçerli. Kullanıcı: {result.UserName}");
}
else
{
    Console.WriteLine(
        $"Kart geçersiz. Mesaj: {result.Message}");
}
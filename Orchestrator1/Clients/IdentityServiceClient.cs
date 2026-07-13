using System.Net.Http.Json;
using Orchestrator.Models;

namespace Orchestrator.Clients;

public class IdentityServiceClient
{
    private readonly HttpClient _httpClient;

    public IdentityServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ValidationResponse?> ValidateAsync(string code)
    {
        try
        {
            var request = new ValidationRequest
            {
                Code = code
            };

            var response = await _httpClient.PostAsJsonAsync("/validate", request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ValidationResponse>();
        }
        catch (HttpRequestException)
        {
            // IdentityService'e ulaşılamadı
            return null;
        }
        catch (TaskCanceledException)
        {
            // İstek zaman aşımına uğradı
            return null;
        }
    }
}
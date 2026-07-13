using System.Net.Http.Json;
using Orchestrator.Models;

namespace Orchestrator.Clients;
//Bu sınıf , IdentityService ile iletişim kurmak için bir HTTP istemcisi sağlar.
public class IdentityServiceClient
{
    private readonly HttpClient _httpClient;

    public IdentityServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ValidationResponse?> ValidateAsync(string code)
    {
        var request = new ValidationRequest
        {
            Code = code
        };

        var response = await _httpClient.PostAsJsonAsync("/validate", request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ValidationResponse>();
    }
}
using Orchestrator.Clients;
using Orchestrator.Interfaces;
using Orchestrator.Services;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IdentityServiceClient>((serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    var baseUrl = configuration
        .GetSection("IdentityService")
        .GetValue<string>("BaseUrl");

    if (string.IsNullOrWhiteSpace(baseUrl))
    {
        throw new InvalidOperationException(
            "IdentityService:BaseUrl ayarı bulunamadı.");
    }

    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(5);
});

builder.Services.AddSingleton<ILogService, LogService>();

var app = builder.Build();

app.MapGet("/", () => "Orchestrator çalışıyor.");

app.MapPost("/access",
    async (
        ValidationRequest request,
        IdentityServiceClient identityService,
        ILogService logService) =>
    {
        var result = await identityService.ValidateAsync(request.Code);

        // IdentityService'e ulaşılamadı
        if (result == null)
        {
            await logService.WriteLogAsync(
                request.Code,
                "-",
                "SERVİS HATASI");

            return Results.Problem(
                "Identity servisine ulaşılamadı.");
        }

        // Kart geçerli
        if (result.IsValid)
        {
            await logService.WriteLogAsync(
                request.Code,
                result.UserName,
                "BAŞARILI");
        }
        // Kart geçersiz
        else
        {
            await logService.WriteLogAsync(
                request.Code,
                "-",
                "KART GEÇERSİZ");
        }

        return Results.Ok(result);
    });

app.Run();
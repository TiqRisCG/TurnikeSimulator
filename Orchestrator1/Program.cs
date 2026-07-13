using Orchestrator.Clients;
using Orchestrator.Interfaces;
using Orchestrator.Models;
using Orchestrator.Services;

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


        if (result == null)
        {
            logService.WriteLog(
                request.Code,
                "-",
                "SERVİS HATASI"
            );

            return Results.Problem(
                "Identity servisine ulaşılamadı."
            );
        }


        if (result.IsValid)
        {
            logService.WriteLog(
                request.Code,
                result.UserName,
                "BAŞARILI"
            );
        }
        else
        {
            logService.WriteLog(
                request.Code,
                "-",
                "KART GEÇERSİZ"
            );
        }


        return Results.Ok(result);
    });

app.Run();
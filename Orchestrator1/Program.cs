using Orchestrator.Clients;

var builder = WebApplication.CreateBuilder(args);
//Şuda olur new HttpClient() ile de
builder.Services.AddHttpClient<IdentityServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7057");
});

var app = builder.Build();

app.MapGet("/", () => "Orchestrator çalışıyor.");

app.Run();
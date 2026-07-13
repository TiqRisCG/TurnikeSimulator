using IdentityService.Data;
using IdentityService.Models;

var builder = WebApplication.CreateBuilder(args);
 

var app = builder.Build();


// Test için ana sayfa
app.MapGet("/", () => "Identity Service çalışıyor.");

app.MapPost("/validate", (ValidationRequest request) =>
{
    var record = FakeDatabase.QrCodes
        .FirstOrDefault(x => x.Code == request.Code);

    if (record is null)
    {
        return Results.Ok(new ValidationResponse
        {
            IsValid = false,
            Message = "Kart bulunamadı."
        });
    }

    if (!record.IsActive)
    {
        return Results.Ok(new ValidationResponse
        {
            IsValid = false,
            UserName = record.UserName,
            Message = "Kart pasif."
        });
    }

    return Results.Ok(new ValidationResponse
    {
        IsValid = true,
        UserName = record.UserName,
        Message = "Geçerli kart."
    });
});

app.Run();
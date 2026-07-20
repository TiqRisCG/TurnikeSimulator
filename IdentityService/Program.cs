using IdentityService.Data;
using IdentityService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    SeedData.Initialize(context);
}

// Test için ana sayfa
app.MapGet("/", () => "Identity Service çalışıyor.");

app.MapPost("/validate",
    async (ValidationRequest request, AppDbContext context) =>
    {
        var record = await context.QrCodes
            .FirstOrDefaultAsync(x => x.Code == request.Code);

        // Kart bulunamadı
        if (record is null)
        {
            context.AccessLogs.Add(new AccessLog
            {
                AccessTime = DateTime.Now,
                Code = request.Code,
                UserName = "-",
                Result = "KART BULUNAMADI"
            });

            await context.SaveChangesAsync();

            return Results.Ok(new ValidationResponse
            {
                IsValid = false,
                Message = "Kart bulunamadı."
            });
        }

        // Kart pasif
        if (!record.IsActive)
        {
            context.AccessLogs.Add(new AccessLog
            {
                AccessTime = DateTime.Now,
                Code = request.Code,
                UserName = record.UserName,
                Result = "KART PASİF"
            });

            await context.SaveChangesAsync();

            return Results.Ok(new ValidationResponse
            {
                IsValid = false,
                UserName = record.UserName,
                Message = "Kart pasif."
            });
        }

        // Başarılı giriş
        context.AccessLogs.Add(new AccessLog
        {
            AccessTime = DateTime.Now,
            Code = request.Code,
            UserName = record.UserName,
            Result = "BAŞARILI"
        });

        await context.SaveChangesAsync();

        return Results.Ok(new ValidationResponse
        {
            IsValid = true,
            UserName = record.UserName,
            Message = "Geçerli kart."
        });
    });

app.Run();
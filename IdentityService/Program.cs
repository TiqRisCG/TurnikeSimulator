using IdentityService.Data;
using IdentityService.Models;
using IdentityService.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapScalarApiReference();
}

// Seed Data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();

    SeedData.Initialize(context);
}

// Ana Sayfa
app.MapGet("/", () => "Identity Service çalışıyor.");



// TÜM KARTLARI GETİR
// GET /cards


app.MapGet("/cards",
    async (AppDbContext context) =>
    {
        var cards = await context.QrCodes.ToListAsync();

        return Results.Ok(cards);
    });



// ID'YE GÖRE KART GETİR
// GET /cards/{id}


app.MapGet("/cards/{id:int}",
    async (int id, AppDbContext context) =>
    {
        var card = await context.QrCodes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card is null)
        {
            return Results.NotFound(new
            {
                Message = "Kart bulunamadı."
            });
        }

        return Results.Ok(card);
    });



// YENİ KART EKLE
// POST /cards


app.MapPost("/cards",
    async (CreateCardRequest request, AppDbContext context) =>
    {
        var card = new QrCodeRecord
        {
            Code = request.Code,
            UserName = request.UserName,
            IsActive = request.IsActive
        };

        context.QrCodes.Add(card);

        await context.SaveChangesAsync();

        return Results.Created($"/cards/{card.Id}", card);
    });


// KART GÜNCELLE
// PATCH /cards/{id}

app.MapPatch("/cards/{id:int}",
    async (int id, UpdateCardRequest request, AppDbContext context) =>
    {
        var card = await context.QrCodes
            .FirstOrDefaultAsync(x => x.Id == id);

        if (card is null)
        {
            return Results.NotFound(new
            {
                Message = "Kart bulunamadı."
            });
        }

        card.Code = request.Code;
        card.UserName = request.UserName;
        card.IsActive = request.IsActive;

        await context.SaveChangesAsync();

        return Results.Ok(card);
    });


// QR KART DOĞRULAMA
// POST /validate


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
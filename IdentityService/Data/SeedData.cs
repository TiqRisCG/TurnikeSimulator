using IdentityService.Models;

namespace IdentityService.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (context.QrCodes.Any())
        {
            return;
        }

        context.QrCodes.AddRange(
            new QrCodeRecord
            {
                Code = "ABC123",
                UserName = "Ahmet Yılmaz",
                IsActive = true
            },
            new QrCodeRecord
            {
                Code = "XYZ789",
                UserName = "Ayşe Demir",
                IsActive = true
            },
            new QrCodeRecord
            {
                Code = "TEST999",
                UserName = "Mehmet",
                IsActive = false
            },
            new QrCodeRecord
            {
                Code = "TEST777",
                UserName = "Ali",
                IsActive = false
            });

        context.SaveChanges();
    }
}
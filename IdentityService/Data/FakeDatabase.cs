using IdentityService.Models;

namespace IdentityService.Data;
//Static class olarak tanımladım çünkü veritabanı yerine geçecek
public static class FakeDatabase
{
    public static List<QrCodeRecord> QrCodes = new()
    {
        new()
        {
            Code = "ABC123",
            UserName = "Ahmet Yılmaz",
            IsActive = true
        },

        new()
        {
            Code = "XYZ789",
            UserName = "Ayşe Demir",
            IsActive = true
        },

        new()
        {
            Code = "TEST999",
            UserName = "Mehmet ",
            IsActive = false
        },

         new()
        {
            Code = "TEST777",
            UserName = "Ali ",
            IsActive = false
        }
    };
}
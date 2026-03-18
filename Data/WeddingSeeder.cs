using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Data;

public static class WeddingSeeder
{
    public static async Task SeedDefaultWeddingDataAsync(User user, HappyWeddingDbContext context)
    {
        var wedding = new Wedding
        {
            UserId = user.Id.ToString(),
            Name1 = "Minh",
            Name2 = "Anh",
            Date = new DateTime(2027, 12, 15),
            Location = "Hà Nội",
            Tagline = "Cuộc phiêu lưu vĩ đại nhất bắt đầu từ một tiếng 'Dạ'",
            Milestones = new List<Milestone>
        {
            new() { Title = "Lễ Dạm Ngõ",      Subtitle = "Lễ chạm ngõ — gặp gỡ hai gia đình",      Date = new DateTime(2025, 6,  1),  Completed = true,  Emoji = "🏠" },
            new() { Title = "Lễ Ăn Hỏi",       Subtitle = "Lễ đính hôn — trao tráp và sính lễ",     Date = new DateTime(2025, 9,  1),  Completed = true,  Emoji = "🎁" },
            new() { Title = "Lễ Cưới Nhà Trai", Subtitle = "Đám cưới bên nhà trai",                  Date = new DateTime(2025, 12, 14), Completed = false, Emoji = "🎊" },
            new() { Title = "Lễ Cưới Nhà Gái", Subtitle = "Đám cưới bên nhà gái",                   Date = new DateTime(2025, 12, 15), Completed = false, Emoji = "💒" },
            new() { Title = "Lễ Vu Quy",        Subtitle = "Rước dâu về nhà chồng",                  Date = new DateTime(2025, 12, 15), Completed = false, Emoji = "🎀" },
            new() { Title = "Tuần Trăng Mật",   Subtitle = "Khoảng thời gian cho riêng hai người",   Date = new DateTime(2025, 12, 20), Completed = false, Emoji = "✈️" },
        }
        };

        context.Weddings.Add(wedding);
        await context.SaveChangesAsync();
    }
}
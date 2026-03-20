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
            Date = new DateTime(2026, 12, 15),
            Location = "Hà Nội",
            Tagline = "Cuộc phiêu lưu vĩ đại nhất bắt đầu từ một tiếng 'Dạ'",
            Milestones =
            [
                new() { Title = "Lễ Dạm Ngõ",      Subtitle = "Lễ chạm ngõ — gặp gỡ hai gia đình",    Date = new DateTime(2026, 6,  1),  Completed = true,  Emoji = "🏠" },
                new() { Title = "Lễ Ăn Hỏi",       Subtitle = "Lễ đính hôn — trao tráp và sính lễ",   Date = new DateTime(2026, 9,  1),  Completed = true,  Emoji = "🎁" },
                new() { Title = "Lễ Cưới Nhà Trai", Subtitle = "Đám cưới bên nhà trai",                Date = new DateTime(2026, 12, 14), Completed = false, Emoji = "🎊" },
                new() { Title = "Lễ Cưới Nhà Gái",  Subtitle = "Đám cưới bên nhà gái",                Date = new DateTime(2026, 12, 15), Completed = false, Emoji = "💒" },
                new() { Title = "Lễ Vu Quy",        Subtitle = "Rước dâu về nhà chồng",               Date = new DateTime(2026, 12, 15), Completed = false, Emoji = "🎀" },
                new() { Title = "Tuần Trăng Mật",   Subtitle = "Khoảng thời gian cho riêng hai người", Date = new DateTime(2026, 12, 20), Completed = false, Emoji = "✈️" },
            ]
        };

        context.Weddings.Add(wedding);

        var guests = new List<Guest>
        {
            new() { WeddingId = wedding.Id, Name = "Chú Lâm",  Side = Side.Groom, Confirmed = true,  SeatCount = 1, Note = "bạn bố"   },
            new() { WeddingId = wedding.Id, Name = "Cô Nương", Side = Side.Groom, Confirmed = true,  SeatCount = 2, Note = "hàng xóm" },
            new() { WeddingId = wedding.Id, Name = "Cô Ninh",  Side = Side.Bride, Confirmed = true,  SeatCount = 1, Note = "bạn mẹ"   },
            new() { WeddingId = wedding.Id, Name = "Chú Sơn",  Side = Side.Bride, Confirmed = true,  SeatCount = 2, Note = "hàng xóm" },
            new() { WeddingId = wedding.Id, Name = "Anh Hải",  Side = Side.Groom, Confirmed = true,  SeatCount = 1, Note = "bạn anh"  },
            new() { WeddingId = wedding.Id, Name = "Chị Hà",   Side = Side.Bride, Confirmed = true,  SeatCount = 1, Note = "bạn chị"  },
            new() { WeddingId = wedding.Id, Name = "Bạn Ngần", Side = Side.Groom, Confirmed = false, SeatCount = 1, Note = "bạn thân" },
            new() { WeddingId = wedding.Id, Name = "Bạn Nhàn", Side = Side.Bride, Confirmed = false, SeatCount = 1, Note = "bạn thân" },
        };
        context.Guests.AddRange(guests);

        var expenseCategories = new List<ExpenseCategory>
        {
            new()
            {
                WeddingId = wedding.Id,
                Name = "Nghi thức & Lễ nghi truyền thống",
                Emoji = "🏮",
                Expenses =
                [
                    new() { Name = "Lễ dạm ngõ (tráp, trà bánh, phong bì…)",               EstimateCost = 8_000_000  },
                    new() { Name = "Lễ ăn hỏi (tráp 6–9–12 quả, tiền nạp tài, lễ vật…)",  EstimateCost = 30_000_000 },
                    new() { Name = "Mâm quả cưới (trầu cau, rượu, bánh phu thê…)",         EstimateCost = 5_000_000  },
                    new() { Name = "Đội bê tráp, người dẫn chương trình lễ ăn hỏi",        EstimateCost = 3_000_000  },
                    new() { Name = "Phong bì lễ gia tiên, lễ bái tổ tiên",                 EstimateCost = 2_000_000  },
                    new() { Name = "Trang phục lễ gia tiên (áo dài, vest)",                EstimateCost = 10_000_000 },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Trang phục cưới & Làm đẹp",
                Emoji = "👗",
                Expenses =
                [
                    new() { Name = "Áo cưới / Váy cưới (mua hoặc thuê)",                   EstimateCost = 15_000_000 },
                    new() { Name = "Áo dài cưới (truyền thống)",                           EstimateCost = 8_000_000  },
                    new() { Name = "Vest / Suit chú rể",                                   EstimateCost = 10_000_000 },
                    new() { Name = "Trang phục cho cha mẹ hai bên",                        EstimateCost = 12_000_000 },
                    new() { Name = "Trang điểm & làm tóc cô dâu",                         EstimateCost = 5_000_000  },
                    new() { Name = "Trang điểm & làm tóc cho mẹ",                         EstimateCost = 2_000_000  },
                    new() { Name = "Phụ kiện (nhẫn, vòng cổ, khuyên tai, trâm…)",         EstimateCost = 25_000_000 },
                    new() { Name = "Giày cưới, tất, đồ lót định hình…",                   EstimateCost = 3_000_000  },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Chụp ảnh & Quay phim",
                Emoji = "📸",
                Expenses =
                [
                    new() { Name = "Chụp ảnh cưới pre-wedding",                            EstimateCost = 15_000_000 },
                    new() { Name = "Chụp ảnh ngày cưới (lễ gia tiên + tiệc)",             EstimateCost = 8_000_000  },
                    new() { Name = "Quay phim trọn gói (highlight + full)",                EstimateCost = 12_000_000 },
                    new() { Name = "Album ảnh in, USB phim, ảnh phóng lớn",               EstimateCost = 5_000_000  },
                    new() { Name = "Drone quay flycam",                                    EstimateCost = 3_000_000  },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Địa điểm & Tiệc cưới",
                Emoji = "🏛️",
                Expenses =
                [
                    new() { Name = "Thuê nhà hàng / trung tâm tiệc cưới",                 EstimateCost = 20_000_000  },
                    new() { Name = "Phí bàn tiệc (ăn + đồ uống)",                         EstimateCost = 150_000_000 },
                    new() { Name = "Phí đồ uống (rượu, bia, nước ngọt…)",                EstimateCost = 20_000_000  },
                    new() { Name = "Phí phục vụ, setup bàn tiệc",                         EstimateCost = 5_000_000   },
                    new() { Name = "Phí thuê sảnh riêng / khu vực chụp ảnh",              EstimateCost = 10_000_000  },
                    new() { Name = "Tiệc nhà trai / nhà gái (nếu tổ chức riêng)",         EstimateCost = 30_000_000  },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Trang trí & Hoa cưới",
                Emoji = "💐",
                Expenses =
                [
                    new() { Name = "Trang trí backdrop sân khấu",                          EstimateCost = 10_000_000 },
                    new() { Name = "Trang trí bàn tiệc, lối đi, cổng hoa",                EstimateCost = 8_000_000  },
                    new() { Name = "Hoa cầm tay cô dâu",                                  EstimateCost = 2_000_000  },
                    new() { Name = "Hoa cài áo cho chú rể & phụ huynh",                   EstimateCost = 1_000_000  },
                    new() { Name = "Hoa bàn thờ gia tiên, bàn lễ",                        EstimateCost = 1_500_000  },
                    new() { Name = "Phụ kiện trang trí (đèn, rèm, nến…)",                EstimateCost = 5_000_000  },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Thiệp mời & In ấn",
                Emoji = "💌",
                Expenses =
                [
                    new() { Name = "Thiệp cưới in (thiết kế + in)",                        EstimateCost = 5_000_000 },
                    new() { Name = "Thiệp cảm ơn / phong bì mừng cưới",                   EstimateCost = 2_000_000 },
                    new() { Name = "Menu tiệc in",                                         EstimateCost = 1_000_000 },
                    new() { Name = "Bảng tên bàn, bảng chỉ dẫn",                          EstimateCost = 1_500_000 },
                    new() { Name = "Banner, standee, backdrop in",                         EstimateCost = 3_000_000 },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Âm thanh & Ánh sáng & MC",
                Emoji = "🎤",
                Expenses =
                [
                    new() { Name = "Thuê DJ / ban nhạc sống",                              EstimateCost = 8_000_000  },
                    new() { Name = "MC dẫn chương trình ngày cưới",                        EstimateCost = 5_000_000  },
                    new() { Name = "Âm thanh, ánh sáng, màn hình LED",                    EstimateCost = 10_000_000 },
                    new() { Name = "Máy chiếu slideshow ảnh cưới",                        EstimateCost = 2_000_000  },
                    new() { Name = "Hiệu ứng khói, pháo hoa lạnh, bong bóng…",           EstimateCost = 3_000_000  },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Xe đưa đón & Di chuyển",
                Emoji = "🚗",
                Expenses =
                [
                    new() { Name = "Xe hoa (xe sang hoặc xe đời mới)",                    EstimateCost = 5_000_000 },
                    new() { Name = "Xe đưa đón cô dâu chú rể",                            EstimateCost = 3_000_000 },
                    new() { Name = "Xe chở đoàn nhà trai / nhà gái",                      EstimateCost = 5_000_000 },
                    new() { Name = "Xe đưa đón khách VIP",                                EstimateCost = 2_000_000 },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Quà tặng & Phong bì",
                Emoji = "🎁",
                Expenses =
                [
                    new() { Name = "Phong bì mừng cho MC, nhiếp ảnh, trang trí…",        EstimateCost = 5_000_000 },
                    new() { Name = "Quà cảm ơn khách mời (hộp quà nhỏ, socola…)",        EstimateCost = 8_000_000 },
                    new() { Name = "Quà cho phù dâu, phù rể",                             EstimateCost = 3_000_000 },
                    new() { Name = "Quà biếu hai họ (nếu có phong tục)",                  EstimateCost = 2_000_000 },
                ],
            },
            new()
            {
                WeddingId = wedding.Id,
                Name = "Khác (phát sinh thường gặp)",
                Emoji = "📋",
                Expenses =
                [
                    new() { Name = "Vé máy bay / khách sạn (nếu cưới xa quê)",            EstimateCost = 10_000_000 },
                    new() { Name = "Ăn uống thử món trước tiệc",                          EstimateCost = 2_000_000  },
                    new() { Name = "Chi phí phát sinh (thời tiết, thêm bàn…)",           EstimateCost = 10_000_000 },
                    new() { Name = "Bảo hiểm đám cưới",                                   EstimateCost = 2_000_000  },
                    new() { Name = "Chi phí sau cưới (trăng mật, tiệc cảm ơn…)",         EstimateCost = 15_000_000 },
                ],
            },
        };

        context.ExpenseCategories.AddRange(expenseCategories);

        await context.SaveChangesAsync();
    }
}
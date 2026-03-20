using HappyWedding.Api.Models.Domain;
using HappyWedding.Api.Models.Dtos.Guest;

namespace HappyWedding.Api.Services;

public interface IGuestService
{
    Task<List<Guest>> GetGuestsAsync(string userId);
    Task<Guest?> AddGuestAsync(string userId, CreateGuestDto dto);
    Task<Guest?> UpdateGuestAsync(string userId, Guid guestId, UpdateGuestDto dto);
    Task<Guest?> ToggleConfirmedAsync(string userId, Guid guestId);
    Task<bool> DeleteGuestAsync(string userId, Guid guestId);
}

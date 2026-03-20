using HappyWedding.Api.DTOs.Wedding;
using HappyWedding.Api.Models.Domain;

namespace HappyWedding.Api.Services;

public interface IWeddingService
{
    Task<Wedding?> GetMyWeddingAsync(string userId);
    Task<Wedding?> CreateWeddingAsync(string userId, CreateWeddingDto dto);
    Task<Wedding?> UpdateWeddingAsync(string userId, UpdateWeddingDto dto);
    Task<bool> DeleteWeddingAsync(string userId);
}

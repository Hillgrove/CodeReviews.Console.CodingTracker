using CodingTracker.Hillgrove.Models;

namespace CodingTracker.Hillgrove.Data;

internal interface ICodingSessionRepository
{
    Task<IEnumerable<CodingSession>> GetAllAsync();
    Task<IEnumerable<CodingSession>> GetFilteredAsync(SessionQueryOptions options);
    Task<CodingSession?> GetByIdAsync(long id);
    Task CreateSessionAsync(CodingSession session);
    Task UpdateSessionAsync(CodingSession session);
    Task DeleteSessionAsync(long id);
}

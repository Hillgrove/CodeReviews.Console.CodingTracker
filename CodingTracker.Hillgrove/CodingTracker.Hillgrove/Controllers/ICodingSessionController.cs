using CodingTracker.Hillgrove.Models;

namespace CodingTracker.Hillgrove.Controllers;

internal interface ICodingSessionController
{
    Task CreateSessionAsync(CodingSession session);
    Task DeleteSessionAsync(long id);
    Task<IEnumerable<CodingSession>> GetAllSessionsAsync();
    Task<IEnumerable<CodingSession>> GetFilteredSessionsAsync(SessionQueryOptions options);
    Task<CodingSession?> GetSessionByIdAsync(long id);
    Task UpdateSessionAsync(CodingSession session);
}

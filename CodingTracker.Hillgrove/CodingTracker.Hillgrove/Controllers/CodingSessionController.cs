namespace CodingTracker.Hillgrove.Controllers;

using CodingTracker.Hillgrove.Data;
using CodingTracker.Hillgrove.Models;

internal class CodingSessionController
{
    private readonly ICodingSessionRepository _repository;

    public CodingSessionController(ICodingSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task CreateSessionAsync(CodingSession session)
    {
        await _repository.CreateSessionAsync(session);
    }

    public async Task<IEnumerable<CodingSession>> GetAllSessionsAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<IEnumerable<CodingSession>> GetFilteredSessionsAsync(
        SessionQueryOptions options
    )
    {
        return await _repository.GetFilteredAsync(options);
    }

    public async Task<CodingSession?> GetSessionByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task UpdateSessionAsync(CodingSession session)
    {
        await _repository.UpdateSessionAsync(session);
    }

    public async Task DeleteSessionAsync(long id)
    {
        await _repository.DeleteSessionAsync(id);
    }
}

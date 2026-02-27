using System.Data;
using CodingTracker.Hillgrove.Models;
using Dapper;

namespace CodingTracker.Hillgrove.Data;

internal class CodingSessionRepository : ICodingSessionRepository
{
    private readonly IDbConnection _dbConnection;

    public CodingSessionRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task CreateSessionAsync(CodingSession session)
    {
        var sql = "INSERT INTO [coding_sessions] (TimeStart, TimeEnd) VALUES (@Start, @End)";

        await _dbConnection.ExecuteAsync(sql, session);
    }

    public async Task<IEnumerable<CodingSession>> GetAllAsync()
    {
        var sql = "SELECT Id, TimeStart, TimeEnd FROM [coding_sessions]";
        var rows = await _dbConnection.QueryAsync<CodingSessionDto>(sql);
        var sessions = rows.Select(s => new CodingSession(s.Id, s.TimeStart, s.TimeEnd));

        return sessions;
    }

    public async Task<IEnumerable<CodingSession>> GetFilteredAsync(SessionQueryOptions options)
    {
        var orderDir = options.Order == SortOrder.Ascending ? "ASC" : "DESC";

        string sql;
        object? parameters = null;

        if (options.Period.HasValue && options.Amount.HasValue)
        {
            var cutoff = options.Period.Value switch
            {
                PeriodType.Days => DateTime.Now.AddDays(-options.Amount.Value),
                PeriodType.Weeks => DateTime.Now.AddDays(-options.Amount.Value * 7),
                PeriodType.Years => DateTime.Now.AddYears(-options.Amount.Value),
                _ => throw new ArgumentOutOfRangeException(nameof(options.Period)),
            };

            sql =
                $"SELECT Id, TimeStart, TimeEnd FROM [coding_sessions] WHERE TimeStart >= @Cutoff ORDER BY TimeStart {orderDir}";
            parameters = new { Cutoff = cutoff };
        }
        else
        {
            sql =
                $"SELECT Id, TimeStart, TimeEnd FROM [coding_sessions] ORDER BY TimeStart {orderDir}";
        }

        var rows = await _dbConnection.QueryAsync<CodingSessionDto>(sql, parameters);
        return rows.Select(s => new CodingSession(s.Id, s.TimeStart, s.TimeEnd));
    }

    public async Task<CodingSession?> GetByIdAsync(long id)
    {
        var sql = "SELECT Id, TimeStart, TimeEnd FROM [coding_sessions] WHERE Id = @Id";
        var row = await _dbConnection.QuerySingleOrDefaultAsync<CodingSessionDto>(
            sql,
            new { Id = id }
        );

        if (row is null)
            return null;

        return new CodingSession(row.Id, row.TimeStart, row.TimeEnd);
    }

    public async Task UpdateSessionAsync(CodingSession session)
    {
        var sql = "UPDATE [coding_sessions] SET TimeStart = @Start, TimeEnd = @End WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, session);
    }

    public async Task DeleteSessionAsync(long id)
    {
        var sql = "DELETE FROM [coding_sessions] WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}

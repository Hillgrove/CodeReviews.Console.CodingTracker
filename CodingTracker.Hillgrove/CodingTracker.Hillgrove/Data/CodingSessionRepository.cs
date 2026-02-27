using System.Data;
using CodingTracker.Hillgrove.Models;
using Dapper;

namespace CodingTracker.Hillgrove.Data;

internal class CodingSessionRepository : ICodingSessionRepository
{
    private readonly IDbConnection _dbConnection;

    private readonly string _tableName;

    public CodingSessionRepository(IDbConnection dbConnection, string tableName)
    {
        _dbConnection = dbConnection;
        _tableName = tableName;
    }

    public async Task CreateSessionAsync(CodingSession session)
    {
        var sql = $"INSERT INTO [{_tableName}] (TimeStart, TimeEnd) VALUES (@Start, @End)";

        await _dbConnection.ExecuteAsync(sql, session);
    }

    public async Task<IEnumerable<CodingSession>> GetAllAsync()
    {
        var sql = $"SELECT Id, TimeStart, TimeEnd FROM {_tableName}";
        var rows = await _dbConnection.QueryAsync<CodingSessionDTO>(sql);
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
                $"SELECT Id, TimeStart, TimeEnd FROM {_tableName} WHERE TimeStart >= @Cutoff ORDER BY TimeStart {orderDir}";
            parameters = new { Cutoff = cutoff };
        }
        else
        {
            sql = $"SELECT Id, TimeStart, TimeEnd FROM {_tableName} ORDER BY TimeStart {orderDir}";
        }

        var rows = await _dbConnection.QueryAsync<CodingSessionDTO>(sql, parameters);
        return rows.Select(s => new CodingSession(s.Id, s.TimeStart, s.TimeEnd));
    }

    public async Task<CodingSession?> GetByIdAsync(long id)
    {
        var sql = $"SELECT Id, TimeStart, TimeEnd FROM {_tableName} WHERE Id = @Id";
        var row = await _dbConnection.QuerySingleOrDefaultAsync<CodingSessionDTO>(
            sql,
            new { Id = id }
        );

        if (row is null)
            return null;

        return new CodingSession(row.Id, row.TimeStart, row.TimeEnd);
    }

    public async Task UpdateSessionAsync(CodingSession session)
    {
        var sql = $"UPDATE [{_tableName}] SET TimeStart = @Start, TimeEnd = @End WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, session);
    }

    public async Task DeleteSessionAsync(long id)
    {
        var sql = $"DELETE FROM [{_tableName}] WHERE Id = @Id";
        await _dbConnection.ExecuteAsync(sql, new { Id = id });
    }
}

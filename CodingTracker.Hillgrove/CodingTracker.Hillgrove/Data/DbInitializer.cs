using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Hillgrove.Data;

internal static class DbInitializer
{
    public static void CreateDatabase(IDbConnection connection, string tableName)
    {
        EnsureDatabaseExists(connection);
        EnsureTableExists(connection, tableName);
    }

    public static void SeedDatabase(IDbConnection connection, string tableName)
    {
        var sql = $"SELECT EXISTS(SELECT 1 FROM [{tableName}])";
        var isEmpty = !connection.QuerySingle<bool>(sql);

        if (isEmpty)
        {
            Console.WriteLine("Database empty. Creating sample sessions...");
            var sessions = new[]
            {
                new
                {
                    TimeStart = new DateTime(2024, 1, 1, 9, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 1, 10, 30, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 2, 14, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 2, 16, 45, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 3, 8, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 3, 12, 0, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 4, 10, 15, 0),
                    TimeEnd = new DateTime(2024, 1, 4, 11, 45, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 5, 16, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 5, 18, 30, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 6, 9, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 6, 13, 20, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 7, 11, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 7, 13, 0, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 8, 15, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 8, 17, 15, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 9, 8, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 9, 10, 45, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 10, 13, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 10, 15, 50, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 11, 9, 45, 0),
                    TimeEnd = new DateTime(2024, 1, 11, 12, 30, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 12, 14, 20, 0),
                    TimeEnd = new DateTime(2024, 1, 12, 16, 40, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 13, 10, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 13, 11, 30, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 14, 16, 15, 0),
                    TimeEnd = new DateTime(2024, 1, 14, 18, 45, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 15, 8, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 15, 10, 50, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 16, 13, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 16, 15, 30, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 17, 9, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 17, 12, 15, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 18, 15, 45, 0),
                    TimeEnd = new DateTime(2024, 1, 18, 17, 20, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 19, 10, 30, 0),
                    TimeEnd = new DateTime(2024, 1, 19, 13, 0, 0),
                },
                new
                {
                    TimeStart = new DateTime(2024, 1, 20, 14, 0, 0),
                    TimeEnd = new DateTime(2024, 1, 20, 16, 30, 0),
                },
            };

            sql = $"INSERT INTO [{tableName}] (TimeStart, TimeEnd) VALUES (@TimeStart, @TimeEnd)";

            int addedSessions = connection.Execute(sql, sessions);

            Console.WriteLine($"Database seeded with {addedSessions} session samples");
        }
    }

    private static void EnsureDatabaseExists(IDbConnection connection)
    {
        var builder = new SqliteConnectionStringBuilder(
            ((SqliteConnection)connection).ConnectionString
        );
        var dbFile = builder.DataSource;

        bool dbExists = File.Exists(dbFile);

        Console.WriteLine($"Checking for database file: {dbFile}");

        if (!dbExists)
        {
            Console.WriteLine($"Database file not found. Creating new file: {dbFile}");
        }
        else
        {
            Console.WriteLine($"Database file found: {dbFile}");
        }
    }

    private static void EnsureTableExists(IDbConnection connection, string tableName)
    {
        Console.WriteLine($"Checking for table: {tableName}");

        var tableCheckSql =
            $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}';";

        var foundTable = connection.ExecuteScalar<string>(tableCheckSql);
        if (string.IsNullOrEmpty(foundTable))
        {
            Console.WriteLine($"Table '{tableName}' not found. Creating table...");
        }
        else
        {
            Console.WriteLine($"Table {tableName} found.");
        }

        var sql =
            $@"
            CREATE TABLE IF NOT EXISTS [{tableName}] (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                TimeStart TEXT NOT NULL,
                TimeEnd TEXT NOT NULL
            );
        ";
        connection.Execute(sql);
    }
}

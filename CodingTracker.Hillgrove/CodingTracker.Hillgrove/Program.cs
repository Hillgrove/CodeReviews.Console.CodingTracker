using System.Data;
using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Data;
using CodingTracker.Hillgrove.UI;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

string connectionString =
    config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string is missing in appsettings.json");

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(config);
services.AddSingleton<IDbConnection>(_ => new SqliteConnection(connectionString));
services.AddSingleton<ICodingSessionRepository>(sp => new CodingSessionRepository(
    sp.GetRequiredService<IDbConnection>()
));
services.AddSingleton<CodingSessionController>();
services.AddSingleton<ConsoleMenu>();

using var provider = services.BuildServiceProvider();

var connection = provider.GetRequiredService<IDbConnection>();
try
{
    DbInitializer.CreateDatabase(connection);
    DbInitializer.SeedDatabase(connection);
}
catch (Exception ex)
{
    Console.WriteLine($"Database error ({ex.GetType().Name}): {ex.Message}");
    return;
}

var menu = provider.GetRequiredService<ConsoleMenu>();
await menu.RunAsync();

Console.Write("Press any key to continue...");
Console.ReadKey();

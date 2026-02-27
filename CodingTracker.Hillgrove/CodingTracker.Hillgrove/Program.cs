using System.Data;
using CodingTracker.Hillgrove.Controllers;
using CodingTracker.Hillgrove.Data;
using CodingTracker.Hillgrove.UI;
using CodingTracker.Hillgrove.UI.Commands;
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
services.AddSingleton<IDbInitializer, DbInitializer>();
services.AddSingleton<ICodingSessionRepository>(sp => new CodingSessionRepository(
    sp.GetRequiredService<IDbConnection>()
));
services.AddSingleton<ICodingSessionController, CodingSessionController>();
services.AddSingleton<AppState>();
services.AddSingleton<IMenuCommand, CreateSessionCommand>();
services.AddSingleton<IMenuCommand, ViewSessionsCommand>();
services.AddSingleton<IMenuCommand, UpdateSessionCommand>();
services.AddSingleton<IMenuCommand, DeleteSessionCommand>();
services.AddSingleton<IMenuCommand, ExitCommand>();
services.AddSingleton<ConsoleMenu>();

using var provider = services.BuildServiceProvider();

var dbInitializer = provider.GetRequiredService<IDbInitializer>();
try
{
    dbInitializer.CreateDatabase();
    dbInitializer.SeedDatabase();
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

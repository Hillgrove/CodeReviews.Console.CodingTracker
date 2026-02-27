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

using var provider = BuildServiceProvider(config, connectionString);

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

static ServiceProvider BuildServiceProvider(IConfiguration config, string connectionString)
{
    var services = new ServiceCollection();

    services.AddSingleton<IConfiguration>(config);
    services.AddTransient<IDbConnection>(_ => new SqliteConnection(connectionString));

    services.AddTransient<IDbInitializer, DbInitializer>();
    services.AddTransient<ICodingSessionRepository, CodingSessionRepository>();

    services.AddTransient<ICodingSessionController, CodingSessionController>();

    services.AddSingleton<AppState>();
    services.AddTransient<IMenuCommand, CreateSessionCommand>();
    services.AddTransient<IMenuCommand, ViewSessionsCommand>();
    services.AddTransient<IMenuCommand, UpdateSessionCommand>();
    services.AddTransient<IMenuCommand, DeleteSessionCommand>();
    services.AddTransient<IMenuCommand, ExitCommand>();
    services.AddTransient<ConsoleMenu>();

    return services.BuildServiceProvider(
        new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true }
    );
}

namespace CodingTracker.Hillgrove.Data;

internal interface IDbInitializer
{
    void CreateDatabase();
    void SeedDatabase();
}

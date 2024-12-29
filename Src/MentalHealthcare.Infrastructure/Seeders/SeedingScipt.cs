using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Seeders;

public static class ClearDatabaseSeeder
{
    public static async Task ClearDatabaseAsync(this Seeder seeder)
    {
        var dbContext = seeder._dbContext;

        // Disable foreign key constraints
        await dbContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 0;");

        // Get all table names
        var tableNames = dbContext.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();

        foreach (var tableName in tableNames)
        {
            if (!string.IsNullOrWhiteSpace(tableName))
            {
                // Truncate each table
                await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE `{tableName}`;");
            }
        }

        // Re-enable foreign key constraints
        await dbContext.Database.ExecuteSqlRawAsync("SET FOREIGN_KEY_CHECKS = 1;");
    }
}
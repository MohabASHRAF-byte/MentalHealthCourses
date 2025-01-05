using MentalHealthcare.Infrastructure.Persistence;

namespace MentalHealthcare.Infrastructure.Seeders;

public class Seeder
{
    public MentalHealthDbContext _dbContext;

    public Seeder(MentalHealthDbContext dbContext)
    {
        this._dbContext = dbContext;
        
    }

    public async Task SeedAsync()
    {
        await this.ClearDatabaseAsync();
    }
}
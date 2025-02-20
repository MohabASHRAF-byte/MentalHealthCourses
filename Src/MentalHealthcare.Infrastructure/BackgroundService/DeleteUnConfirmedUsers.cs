using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.BackgroundService;

public class DeleteUnConfirmedUsers(
    ILogger<DeleteUnConfirmedUsers> logger,
    IServiceProvider serviceProvider

) : Microsoft.Extensions.Hosting.BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var interval = TimeSpan.FromMinutes(1);
            logger.LogInformation("Running user cleanup job at: {time}", DateTimeOffset.Now);
            await DeleteUnconfirmedUsersAsync();
            await Task.Delay(interval, stoppingToken);
        }
    }

private async Task DeleteUnconfirmedUsersAsync()
{
    try
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MentalHealthDbContext>();
        const int unConfirmationTimeInMinutes = 1;
        var cutoffTime = DateTime.UtcNow.AddMinutes(-unConfirmationTimeInMinutes);

        var unconfirmedUsers = await dbContext.Users
            .Where(u => !u.EmailConfirmed && u.RegistrationDate <= cutoffTime)
            .ToListAsync();

        if (!unconfirmedUsers.Any())
        {
            logger.LogInformation("No unconfirmed users found for deletion.");
            return;
        }

        //  Convert User IDs to HashSet<string>
        var unconfirmedUserIds = unconfirmedUsers.Select(u => u.Id.ToString()).ToHashSet();

        //  Ensure SystemUsers and Admins use string for comparison
        var sysUsers = await dbContext.SystemUsers
            .Where(su => unconfirmedUserIds.Contains(su.UserId)) // No need for Guid.Parse()
            .ToListAsync();

        var adminUsers = await dbContext.Admins
            .Where(au => unconfirmedUserIds.Contains(au.UserId)) // No need for Guid.Parse()
            .ToListAsync();

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            dbContext.SystemUsers.RemoveRange(sysUsers);
            dbContext.Admins.RemoveRange(adminUsers);
            dbContext.Users.RemoveRange(unconfirmedUsers);

            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            logger.LogInformation("Deleted {userCount} unconfirmed users.", unconfirmedUsers.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            logger.LogError(ex, "Error occurred while deleting unconfirmed users.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while executing user deletion process.");
    }
}
}
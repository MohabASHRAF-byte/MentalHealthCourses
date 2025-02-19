using MentalHealthcare.Application.Resources.Localization.Resources;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories;

public class AdminRepository(
    MentalHealthDbContext dbContext,
    UserManager<User> userManager,
    ILogger<AdminRepository> logger,
    ILocalizationService localizationService) : IAdminRepository
{
    public async Task RegisterUser(User user, string password, Admin userToRegister)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            // Check if the username or email already exists
            var existingUser = await dbContext.Users
                .Where(u =>
                    u.NormalizedUserName!.ToLower() == user.UserName!.ToLower() ||
                    u.NormalizedEmail!.ToLower() == user.Email!.ToLower())
                .Select(u => new { u.NormalizedUserName, u.NormalizedEmail })
                .FirstOrDefaultAsync();

            if (existingUser is not null)
            {
                if (existingUser.NormalizedUserName!.Equals(user.UserName, StringComparison.CurrentCultureIgnoreCase))
                    throw new BadHttpRequestException(
                        localizationService.GetMessage("UsernameAlreadyTaken", "Username is already taken.")
                    );

                if (existingUser.NormalizedEmail!.Equals(user.Email, StringComparison.CurrentCultureIgnoreCase))
                    throw new BadHttpRequestException(
                        localizationService.GetMessage("EmailAlreadyTaken", "Email is already taken.")
                    );
            }

            // Check if the email exists in the PendingAdmins table
            var pendingAdmin = await dbContext.PendingAdmins
                .Where(pa => pa.Email.ToLower() == user.Email!.ToLower())
                .FirstOrDefaultAsync();

            if (pendingAdmin is null)
            {
                throw new BadHttpRequestException(
                    localizationService.GetMessage("EmailNotRegisteredAsAdmin", "Email is not registered as admin.")
                );
            }

            // Create the user in the Identity system
            var createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
            {
                await transaction.RollbackAsync();
                throw new BadHttpRequestException(
                    localizationService.GetMessage("UserCreationFailed", "User creation failed. Please try again.")
                );
            }

            // Add the user to the Admins table and remove from PendingAdmins
            await dbContext.Admins.AddAsync(userToRegister);
            dbContext.PendingAdmins.Remove(pendingAdmin);

            await dbContext.SaveChangesAsync();

            // Commit the transaction
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            // Rollback the transaction on error
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> IsExistAsync(string email)
    {
        return await dbContext.Admins.AnyAsync(a => a.User.Email == email);
    }
    

    public async Task<bool> IsPendingExistAsync(string email)
    {
        // Use 'AnyAsync' to check for existence.
        return await dbContext.PendingAdmins.AnyAsync(a => a.Email == email);
    }

    public async Task<bool> AddPendingAsync(string email, int adminId)
    {
        var pendingAdmin = new PendingAdmins
        {
            Email = email,
            AdminId = adminId
        };
        await dbContext.PendingAdmins.AddAsync(pendingAdmin);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePendingAsync(string oldEmail, string newEmail, int adminId)
    {
        var pendingAdmin = await dbContext.PendingAdmins
            .FirstOrDefaultAsync(p => p.Email == oldEmail);
        if (pendingAdmin == null)
        {
            return false;
        }

        pendingAdmin.Email = newEmail;
        pendingAdmin.AdminId = adminId;

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeletePendingAsync(List<string> emails)
    {
        var pendingAdmins = await dbContext.PendingAdmins
            .Where(p => emails.Contains(p.Email))
            .ToListAsync();

        if (!pendingAdmins.Any())
        {
            return false;
        }

        dbContext.PendingAdmins.RemoveRange(pendingAdmins);

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<(int, IEnumerable<PendingAdmins>)> GetAllAsync(string? searchName, int pageNumber, int pageSize)
    {
        searchName ??= string.Empty;
        searchName = searchName.ToLower();
        var baseQuery = dbContext.PendingAdmins
            .Where(r => r.Email.ToLower().Contains(searchName));
        var totalCount = await baseQuery.CountAsync();
        var admins = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();
        return (totalCount, admins);
    }

    public async Task<Admin> GetAdminByIdentityAsync(string adminId)
    {
        var admin = await dbContext.Admins.FirstOrDefaultAsync(
            a => a.UserId == adminId
        );

        if (admin is null)
            throw new ResourceNotFound(
                "Admin",
                "المسؤول",
                adminId.ToString()
            );
        return admin;
    }
}
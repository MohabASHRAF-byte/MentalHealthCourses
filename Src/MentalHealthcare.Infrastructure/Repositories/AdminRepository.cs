using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories;

public class AdminRepository(
    MentalHealthDbContext dbContext,
    UserManager<User> userManager
) : IAdminRepository
{
    public async Task<bool> RegisterUser(User user, string password, Admin userToRegister)
    {
        IdentityResult createUserResult = await userManager.CreateAsync(user, password);
        if (!createUserResult.Succeeded)
            return false;
        //TODO return the error list 
        createUserResult.Errors.ToList().ForEach(error => error.Description = error.Description);
        await dbContext.AddAsync(userToRegister);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsExistAsync(string email)
    {
        return await dbContext.Admins.AnyAsync(a => a.User.Email == email);
    }

    public Task<bool> UpdateAsync(string oldEmail, string newEmail)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(string email)
    {
        throw new NotImplementedException();
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

    public async Task<Admin?> GetAdminByIdentityAsync(string adminId)
    {
        return await dbContext.Admins.FirstOrDefaultAsync(
            a => a.UserId == adminId
        );
    }
}
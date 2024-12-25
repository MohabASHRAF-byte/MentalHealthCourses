using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MentalHealthcare.Infrastructure.Repositories;

public class UserRepository(
    MentalHealthDbContext dbContext,
    UserManager<User> userManager,
    ILogger<UserRepository> logger) : IUserRepository
{
    public async Task<(bool, List<string>)> RegisterUser(User user, string password, SystemUser userToRegister)
    {
        var errors = new List<string>();
        var existingUser = await dbContext.Users
            .Where(u =>
                u.NormalizedUserName == user.UserName ||
                u.Email == user.Email
            ).ToListAsync();
        var validUserName = !existingUser.Any(u =>
            u.NormalizedUserName!.Equals(user.UserName));
        var validEmail = !existingUser.Any(u => u.Email!.Equals(user.Email));
        if (!validUserName)
            errors.Add($"{user.UserName} is already taken.");
        if (!validEmail)
            errors.Add($"{user.Email} is already taken.");
        if (errors.Count != 0)
            return (false, errors);
        try
        {
            IdentityResult createUserResult = await userManager.CreateAsync(user, password);
            if (!createUserResult.Succeeded)
                return (false, new());
            await dbContext.AddAsync(userToRegister);
            await dbContext.SaveChangesAsync();
            return (true, new());
        }
        catch (Exception e)
        {
            var userCred = await dbContext.Users.FindAsync(user.Id);
            if (userCred != null)
                dbContext.Users.Remove(user);
            var sysUser = await dbContext.SystemUsers.FindAsync(userToRegister.SystemUserId);
            if (sysUser != null)
                dbContext.SystemUsers.Remove(sysUser);
            logger.LogWarning(e.Message);
            return (false, new());
        }
    }


    public async Task<Guid?> GetUserTokenCodeAsync(User user)
    {
        var tokenRecord = await dbContext.SystemUserTokenCodes
            .FirstOrDefaultAsync(t => t.User.Id == user.Id);
        if (tokenRecord == null)
        {
            return null;
        }

        return tokenRecord.Id;
    }

    public async Task<Guid> ChangePasswordAsync(User user)
    {
        var tokenRecord = await dbContext.SystemUserTokenCodes
            .FirstOrDefaultAsync(t => t.User.Id == user.Id);

        // If a record exists, remove it
        if (tokenRecord != null)
        {
            dbContext.SystemUserTokenCodes.Remove(tokenRecord);
            await dbContext.SaveChangesAsync();
        }

        // Create a new token record
        var newToken = new SystemUserTokenCode
        {
            Id = Guid.NewGuid(), // Generate a new GUID
            User = user
        };

        dbContext.SystemUserTokenCodes.Add(newToken);
        await dbContext.SaveChangesAsync();
        return newToken.Id;
    }

    public async Task<long> GetUserRolesAsync(string username, string tenant)
    {
        // Query for the user based on username and tenant
        var user = await (from u in dbContext.Users
            where u.UserName == username && u.Tenant == tenant
            select new { u.Roles }).FirstOrDefaultAsync();

        // If the user is null, return -1 to indicate no such user
        if (user == null)
        {
            return -1;
        }

        // Return the roles (can be 0 or any other value from the database)
        return user.Roles;
    }

    public async Task SetUserRolesAsync(string username, string tenant, long roles)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == username && u.Tenant == tenant);

        if (user == null)
            throw new Exception("User does not exist");

        user.Roles = roles;

        await dbContext.SaveChangesAsync();
    }


    public async Task<User?> GetUserByUserNameAsync(string userName, string tenant)
    {
        var user = await userManager.Users
            .Where(u => u.UserName == userName && u.Tenant == tenant)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email, string tenant)
    {
        var user = await userManager.Users
            .Where(u => u.Email == email && u.Tenant == tenant)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByPhoneNumberAsync(string phoneNumber, string tenant)
    {
        var user = await userManager.Users
            .Where(u => u.PhoneNumber == phoneNumber && u.Tenant == tenant)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<(int? systemUserId, int? adminId)> GetSystemUserOrAdminIdByIdentityIdAsync(string userId)
    {
        var systemUser = await dbContext.SystemUsers
            .Where(su => su.UserId == userId)
            .FirstOrDefaultAsync();

        var adminUser = await dbContext.Admins
            .Where(au => au.UserId == userId)
            .FirstOrDefaultAsync();
        return (systemUser?.SystemUserId, adminUser?.AdminId);
    }
}
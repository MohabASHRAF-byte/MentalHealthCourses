using MentalHealthcare.Domain.Dtos.User;
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IUserRepository
{
    public Task<(bool Succeeded, List<string> Errors)> RegisterUser(User user, string password,
        SystemUser userToRegister);

    public Task<Guid?> GetUserTokenCodeAsync(User user);
    public Task<Guid> ChangePasswordAsync(User user);

    public Task<long> GetUserRolesAsync(string username, string tenant);
    public Task SetUserRolesAsync(string username, string tenant, long roles);
    public Task<User?> GetUserByUserNameAsync(string userName, string tenant);
    public Task<User?> GetUserByEmailAsync(string email, string tenant);
    public Task<User?> GetUserByPhoneNumberAsync(string phoneNumber, string tenant);

    public Task<(int? systemUserId, int? adminId)> GetSystemUserOrAdminIdByIdentityIdAsync(
        string userId
    );

    public Task UpdateUserProfileAsync(
        int userId,
        string? firstName,
        string? lastName,
        string? phoneNumber,
        DateOnly? birthDate
    );

    public Task<UserProfileDto> GetUserProfileByIdAsync(int userId);
}
using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IAdminRepository
{
    public Task<(bool Succeeded, List<string> Errors)> RegisterUser(User user, string password, Admin userToRegister);

    public Task<bool> IsExistAsync(string email);
    public Task<bool> UpdateAsync(string oldEmail, string newEmail);
    public Task<bool> DeleteAsync(string email);
    public Task<bool> IsPendingExistAsync(string email);
    public Task<bool> AddPendingAsync(string email, int admin);
    public Task<bool> UpdatePendingAsync(string oldEmail, string newEmail, int adminId);
    public Task<bool> DeletePendingAsync(List<string> emails);
    public Task<(int, IEnumerable<PendingAdmins>)> GetAllAsync(string? search, int requestPageNumber, int requestPageSize);
    public Task<Admin> GetAdminByIdentityAsync(string adminId);


}
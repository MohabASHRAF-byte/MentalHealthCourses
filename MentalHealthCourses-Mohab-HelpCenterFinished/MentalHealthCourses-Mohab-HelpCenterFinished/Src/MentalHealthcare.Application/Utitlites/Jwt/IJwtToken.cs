using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Application.Utitlites.Jwt
{
    public interface IJwtToken
    {
        public Task<(string, string)> GetTokens(User user);
        public Task<(string, string)> RefreshTokens(string token);
    }
}
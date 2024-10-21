using MentalHealthcare.Domain.Constants;
using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MentalHealthcare.Application.Utitlites.Jwt;
/*
   * How Refresh Tokens Work:
   * 
   * Problem with Traditional Approach:
   * - Storing tokens, refresh tokens, and expiration times in the database
   *   can lead to significant database growth over time.
   * - This results in inefficiency and poor scalability as the system expands.
   *
   * Improved Approach:
   * - Instead of storing tokens and refresh tokens directly, we store only a mutable GUID passcode in the database.
   * - This passcode is embedded in the generated tokens.
   *
   * Token Validation:
   * - When a token is received, it is decrypted to extract the passcode.
   * - The extracted passcode is compared with the one stored in the database.
   *
   * Token Refresh Process:
   * 1. If the passcode matches, new tokens (both access and refresh tokens) are generated.
   * 2. The stored passcode in the database is updated with a new GUID.
   * 3. This ensures tokens remain unique and secure, while still being changeable.
   *
   * Pros:
   * - Minimal database growth: Only the GUID passcode is stored, reducing the storage overhead.
   * - Scalability: The system can handle growth efficiently without being burdened by the increasing token data.
   *
   * Cons:
   * - Token state is tied to the database: If the passcode changes and a token hasn't been refreshed, the old token becomes invalid, requiring synchronization.
   * - Complexity: This approach introduces complexity in terms of token validation and database operations, as tokens need to be frequently refreshed and passcodes updated.
*/
public class JwtToken(
    IConfiguration configuration,
    IUserRepository userRepository
    ) : IJwtToken
{
    private void GenerateToken(out string jwtToken, List<Claim> claims, DateTime expiration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);
        jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
    }

    private JwtSecurityToken? GetTokenInfo(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)); // Security key

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key, // Add the security key here
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            return (JwtSecurityToken)validatedToken;
        }
        catch (Exception)
        {
            return null;
        }
    }


    private Task<string> CreateJwtToken(User user)
    {
        // adding info of the user 
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(Global.TenantClaimType,user.Tenant)
        };
        // adding the roles 
        claims.Add(new Claim(Global.Roles, user.Roles.ToString()));
        GenerateToken(out var token, claims, DateTime.Now.AddMinutes(30));
        return Task.FromResult(token);
    }

    private async Task<string> CreateRefreshToken(User user)
    {
        var passcode = await userRepository.ChangePasswordAsync(user);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName!),
            new(Global.PassCode, passcode.ToString()),
            new(Global.TenantClaimType,user.Tenant)
        };
        GenerateToken(out var token, claims, DateTime.Now.AddDays(20));
        return token;
    }

    private Task<bool> IsTokenValid(JwtSecurityToken token, string passcode)
    {

        if (token.ValidTo < DateTime.Now)
        {
            return Task.FromResult(false);
        }
        var tokenPasscode = token.Claims.FirstOrDefault(x => x.Type == Global.PassCode)?.Value;
        return Task.FromResult(tokenPasscode == passcode);
    }

    public async Task<(string, string)> GetTokens(User user)
    {
        var token = await CreateJwtToken(user);
        var refreshToken = await CreateRefreshToken(user);
        return (token, refreshToken);
    }

    public async Task<(string, string)> RefreshTokens(string token)
    {
        var tokenInfo = GetTokenInfo(token);

        if (tokenInfo == null)
            return (null, null)!;

        var userName = tokenInfo.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        if (userName == null)
            return (null, null)!;
        var tenant = tokenInfo.Claims.FirstOrDefault(x => x.Type == Global.TenantClaimType)?.Value;
        if (tenant == null)
            return (null, null)!;
        var user = await userRepository.GetUserByUserNameAsync(userName, tenant);
        if (user == null)
            return (null, null)!;
        var passcode = await userRepository.GetUserTokenCodeAsync(user);
        if (passcode == null)
            return (null, null)!;
        if (!await IsTokenValid(tokenInfo, passcode.ToString()!))
        {
            return (null, null)!;
        }
        return await GetTokens(user);
    }
}
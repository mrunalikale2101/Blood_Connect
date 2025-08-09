using BackendDotNet.Models;
using System.Security.Claims;

namespace BackendDotNet.Services
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
        string? GetEmailFromToken(string token);
        bool IsTokenExpired(string token);
    }
}

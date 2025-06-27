using System.Security.Claims;
using TechnicalService.Domain.Entities;

namespace TechnicalService.Application.Contracts.ServicesContracts
{
    public interface IAuthService
    {
        (string, DateTime) GenerateJwtTokenForUser(User user);
        (string, DateTime) GenerateJwtTokenForPersonnel(Personnel personnel);
        string GenerateRefreshToken();
        string GenerateEmailVerificationToken(string email);
        string GeneratePasswordResetToken(string userId);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        ClaimsPrincipal GetPrincipalFromVerifyToken(string token);
    }
}

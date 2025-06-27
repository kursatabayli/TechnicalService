using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Shared.Services.Contracts
{
    public interface IAuthService
    {
        Task<Result> LoginAsync(LoginDto loginDto, string logoinTokenEndpoint, string clientTypes);
        Task<Result> RefreshTokenAsync(string refreshTokenEndpoint, string clientTypes);
        Task<Result> LogoutAsync(string logoutTokenEndpoint, string clientTypes);
        Task<Result> RegisterUserAsync(RegisterDto registerUser, string clientTypes);
        Task<UserClaims> CheckSessionAsync(string clientTypes); 

    }
}

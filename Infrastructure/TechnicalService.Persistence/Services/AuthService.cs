using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Domain.Entities;
using TechnicalService.Persistence.Helpers;

namespace TechnicalService.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSection _jwtSection;
        public AuthService(IOptions<JwtSection> jwtSection)
        {
            _jwtSection = jwtSection.Value;
        }

        public (string, DateTime) GenerateJwtTokenForUser(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Surname, user.Surname),
                new(ClaimTypes.Role, user.Role.GetDescription()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSection.Issuer,
                audience: _jwtSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }
        
        public (string, DateTime) GenerateJwtTokenForPersonnel(Personnel personnel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, personnel.Id.ToString()),
                new(ClaimTypes.Email, personnel.Email),
                new(ClaimTypes.Name, personnel.Name),
                new(ClaimTypes.Surname, personnel.Surname),
                new(ClaimTypes.Role, personnel.Role.GetDescription()),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSection.Issuer,
                audience: _jwtSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }

        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtSection.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSection.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key)),
                ValidateLifetime = false // Süresi dolmuş token'ları kabul et
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }

        public string GenerateEmailVerificationToken(string email)
        {
            var claims = new[]
            { 
                new Claim(JwtRegisteredClaimNames.Email, email), 
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSection.Issuer,
                audience: _jwtSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public string GeneratePasswordResetToken(string userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSection.Issuer,
                audience: _jwtSection.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal GetPrincipalFromVerifyToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = _jwtSection.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSection.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSection.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

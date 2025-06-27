using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechnicalService.Application.Features.CQRS.PersonnelAuth.Commands;
using TechnicalService.Application.Features.CQRS.Personnels.Commands;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelAuthController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;
        public PersonnelAuthController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = Mapper.Map<PersonnelLoginCommand>(dto);
            var result = await Mediator.Send(command);
            var response = Result.Default(result.IsSuccess, result.StatusMessage, result.StatusCode, result.Status);

            if (result.IsSuccess)
            {
                Response.Cookies.Append(nameof(TokenTypes.PersonnelAccessToken), result.Data.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.AccessTokenExpiration,

                });

                if (dto.RememberMe)
                {
                    Response.Cookies.Append(nameof(TokenTypes.PersonnelRefreshToken), result.Data.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = result.Data.RefreshTokenExpiration
                    });
                }

                return Ok(response);
            }
            else
                return BadRequest(result);
        }

        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {

            var refreshToken = Request.Cookies[nameof(TokenTypes.PersonnelRefreshToken)];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var tokenDto = new RefreshTokenDto { RefreshToken = refreshToken };

            var command = Mapper.Map<PersonnelRefreshTokenCommand>(tokenDto);

            var result = await Mediator.Send(command);

            var response = Result.Default(result.IsSuccess, result.StatusMessage, result.StatusCode, result.Status);

            if (result.IsSuccess)
            {
                Response.Cookies.Append(nameof(TokenTypes.PersonnelAccessToken), result.Data.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.AccessTokenExpiration
                });

                Response.Cookies.Append(nameof(TokenTypes.PersonnelRefreshToken), result.Data.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.RefreshTokenExpiration
                });
                return Ok(response);
            }
            else
                return Unauthorized(result);
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            var accessToken = Request.Cookies[nameof(TokenTypes.PersonnelAccessToken)];
            var refreshToken = Request.Cookies[nameof(TokenTypes.PersonnelRefreshToken)];

            if (!string.IsNullOrEmpty(accessToken))
                Response.Cookies.Append(nameof(TokenTypes.PersonnelAccessToken), accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(-60)
                });

            if (!string.IsNullOrEmpty(refreshToken))
                Response.Cookies.Append(nameof(TokenTypes.PersonnelRefreshToken), refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-8)
                });

            return Ok(Result.Success());
        }

        [HttpPost("RequestPasswordReset")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PersonnelRequestPasswordResetLinkDto requestPersonnelPasswordResetLinkDto)
        {
            var command = Mapper.Map<PersonnelRequestPasswordResetLinkCommand>(requestPersonnelPasswordResetLinkDto);
            var result = await Mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }


        [HttpPut("ChangePassword")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> ChangePassword(ChangePersonnelPasswordDto dto)
        {
            var command = Mapper.Map<ChangePersonnelPasswordCommand>(dto);
            command.Id = CurrentPersonnelId;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        private Guid CurrentPersonnelId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı");
                }
                return Guid.Parse(userIdClaim);
            }
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.DTOs.DTOs.AuthDTOs;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserAuthController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;
        public UserAuthController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = Mapper.Map<UserLoginCommand>(dto);
            var result = await Mediator.Send(command);

            var response = Result.Default(result.IsSuccess, result.StatusMessage, result.StatusCode, result.Status);

            if (result.IsSuccess)
            {
                Response.Cookies.Append(nameof(TokenTypes.UserAccessToken), result.Data.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.AccessTokenExpiration
                });

                if (dto.RememberMe)
                {
                    Response.Cookies.Append(nameof(TokenTypes.UserRefreshToken), result.Data.RefreshToken, new CookieOptions
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
        public async Task<IActionResult> RefreshToken()
        {

            var refreshToken = Request.Cookies[nameof(TokenTypes.UserRefreshToken)];

            var tokenDto = new RefreshTokenDto { RefreshToken = refreshToken };

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var command = Mapper.Map<UserRefreshTokenCommand>(tokenDto);

            var result = await Mediator.Send(command);

            var response = Result.Default(result.IsSuccess, result.StatusMessage, result.StatusCode, result.Status);


            if (result.IsSuccess)
            {
                Response.Cookies.Append(nameof(TokenTypes.UserAccessToken), result.Data.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = result.Data.AccessTokenExpiration
                });

                Response.Cookies.Append(nameof(TokenTypes.UserRefreshToken), result.Data.RefreshToken, new CookieOptions
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
        public IActionResult Logout()
        {
            var refreshToken = Request.Cookies[nameof(TokenTypes.UserRefreshToken)];
            var accessToken = Request.Cookies[nameof(TokenTypes.UserAccessToken)];

            if (!string.IsNullOrEmpty(accessToken))
                Response.Cookies.Append(nameof(TokenTypes.UserAccessToken), accessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddMinutes(-60)
                });

            if (!string.IsNullOrEmpty(refreshToken))
                Response.Cookies.Append(nameof(TokenTypes.UserRefreshToken), refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-8)
                });

            return Ok(Result.Success());
        }

        [HttpGet("RequestPasswordReset/{email}")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var result = await Mediator.Send(new UserRequestResetPasswordLinkCommand { Email = email });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}

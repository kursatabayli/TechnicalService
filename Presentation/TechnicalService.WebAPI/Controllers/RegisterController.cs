using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.UserAuth.Commands;
using TechnicalService.DTOs.DTOs.AuthDTOs;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class RegisterController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;
        public RegisterController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = Mapper.Map<UserRegisterCommand>(dto);
            var result = await Mediator.Send(command);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("verify-email/{confirm}")]
        public async Task<IActionResult> VerifyEmail(string confirm)
        {
            var result = await Mediator.Send(new UserEmailVerifyCommand { Token = confirm });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("resend-email/{email}")]
        public async Task<IActionResult> ResendEmail(string email)
        {
            var result = await Mediator.Send(new UserRequestVerifyMailCommand { Email = email });
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}

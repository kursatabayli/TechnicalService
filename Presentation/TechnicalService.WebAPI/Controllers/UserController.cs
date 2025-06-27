using TechnicalService.Application.Features.CQRS.Users.Commands;
using TechnicalService.Application.Features.CQRS.Users.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TechnicalService.DTOs.DTOs.UserDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }
        private Guid CurrentUserId
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


        [HttpGet("GetAllUsers")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            return Ok(result);
        }

        [HttpGet("CurrentUser")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetUserByIdQuery(CurrentUserId));
            return Ok(result);
        }

        [HttpGet("GetUserById/{Id}")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetUserById(Guid Id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(Id));
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> Update(UpdateUserDto dto)
        {
            var command = _mapper.Map<UpdateUserCommand>(dto);
            command.Id = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("ChangePassword")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> ChangePassword(ChangeUserPasswordDto dto)
        {
            var command = _mapper.Map<ChangeUserPasswordCommand>(dto);
            command.Id = CurrentUserId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("SendPhoneNumberVerificationCode")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> SendPhoneNumberVerificationCode()
        {
            var command = new SendPhoneNumberVerificationCodeCommand { UserId = CurrentUserId };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetUserPasswordDto dto)
        {
            var command = _mapper.Map<ResetUserPasswordCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

    }
}

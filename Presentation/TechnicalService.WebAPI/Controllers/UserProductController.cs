using TechnicalService.Application.Features.CQRS.UserProducts.Commands;
using TechnicalService.Application.Features.CQRS.UserProducts.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TechnicalService.DTOs.DTOs.UserProductDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllUserProductQuery());
            return Ok(result);
        }

        [HttpGet("GetUserProductByUserProductId/{id}")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetUserProductByIdQuery(id));
            if (result.Data.UserId == CurrentUserId)
                return Ok(result);
            return Unauthorized("Yetkisiz Erişim.");
        }

        [HttpGet("GetUserProductById/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetUserProductById(int id)
        {
            var result = await _mediator.Send(new GetUserProductByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("GetUserProducts")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> GetByUserId()
        {
            var result = await _mediator.Send(new GetUsersProductsByUserIdQuery(CurrentUserId));
            return Ok(result);
        }

        [HttpPost("AddUserProduct")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> Create(AddUserProductDto dto)
        {
            dto.UserId = CurrentUserId;
            var command = _mapper.Map<AddUserProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserProductDto dto)
        {
            var command = _mapper.Map<UpdateUserProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteUserProductCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
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
    }
}

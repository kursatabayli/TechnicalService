using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Commands;
using TechnicalService.Application.Features.CQRS.ServiceRecords.Queries;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.DTOs.ServiceRecordDTOs;
using TechnicalService.Application.Extensions;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRecordController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;
        public ServiceRecordController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }
        [HttpGet("GetAllServiceRecords")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetAllServiceRecordsQuery());
            return Ok(result);
        }

        [HttpGet("GetUserServiceRecordsByUserId")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> GetRepairRequests()
        {
            var result = await Mediator.Send(new GetServiceRecordsByUserIdQuery(CurrentUserId));
            return Ok(result);
        }

        [HttpGet("GetServiceRecordById/{id}")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await Mediator.Send(new GetServiceRecordByIdQuery(id));
            if (result.Data.UserId == CurrentUserId)
                return Ok(result);
            return Unauthorized("Yetkisiz Erişim.");
        }

        [HttpGet("GetServiceRecordDetail/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetServiceRecordDetail(Guid id)
        {
            var result = await Mediator.Send(new GetServiceRecordByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateServiceRecord")]
        [Authorize(Policy = nameof(AppPolicies.UserAccesses))]
        public async Task<IActionResult> CreateRepairRequest([FromBody] CreateServiceRecordDto dto)
        {
            dto.UserId = CurrentUserId;
            var command = Mapper.Map<CreateServiceRecordCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateServiceRecord")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> UpdateServiceRecord([FromBody] UpdateServiceRecordDto dto)
        {
            var command = Mapper.Map<UpdateServiceRecordCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetServiceRecordsByPersonnelId")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetServiceRecordsByPersonnelId()
        {
            var result = await Mediator.Send(new GetServiceRecordsByPersonnelIdQuery(CurrrentPersonnelId));
            return Ok(result);
        }

        [HttpGet("SearchServiceRecord/{searchTerm}")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> SearchServiceRecord(string searchTerm)
        {
            var result = await Mediator.Send(new SearchServiceRecordsQuery(searchTerm));
            return Ok(result);
        }

        [HttpGet("GetServiceRecordsByServiceId")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetServiceRecordsByServiceId()
        {
            var result = await Mediator.Send(new GetServiceRecordsByServiceIdQuery(CurrrentPersonnelId));
            return Ok(result);
        }

        private Guid CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var roleClaim = User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(userIdClaim) || roleClaim != RoleDto.User.GetDescription())
                {
                    throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı veya rol yetkisiz.");
                }

                return Guid.Parse(userIdClaim);
            }
        }
        private Guid CurrrentPersonnelId
        {
            get
            {
                var personnelIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var roleClaim = User.FindFirstValue(ClaimTypes.Role);

                if (string.IsNullOrEmpty(personnelIdClaim) || string.IsNullOrEmpty(roleClaim))
                    throw new UnauthorizedAccessException("Kullanıcı kimliği veya rol bilgisi bulunamadı.");

                string disallowedRole = RoleDto.User.GetDescription();

                if (roleClaim == disallowedRole)
                    throw new UnauthorizedAccessException("Bu işlemi yapmak için yetkiniz bulunmamaktadır.");

                return Guid.Parse(personnelIdClaim);
            }
        }
    }
}

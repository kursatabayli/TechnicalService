using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Commands;
using TechnicalService.Application.Features.CQRS.ServiceRecordSteps.Queries;
using TechnicalService.DTOs.DTOs.ServiceRecordStepDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRecordStepController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;

        public ServiceRecordStepController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }

        [HttpGet("GetServiceRecordStepsByServiceRecordId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetServiceRecordStepsByServiceRecordId(Guid id)
        {
            var result = await Mediator.Send(new GetServiceRecordStepsByServiceRecordIdQuery(id));
            return Ok(result);
        }

        [HttpPost("AddServiceRecordStep")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> AddServiceRecordStep([FromBody] AddServiceRecordStepDto addServiceRecordStepDto)
        {
            addServiceRecordStepDto.PersonnelId = CurrentPersonnelId;
            var command = Mapper.Map<AddServiceRecordStepCommand>(addServiceRecordStepDto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetServiceRecordStepById/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetServiceRecordStepById(int id)
        {
            var result = await Mediator.Send(new GetServiceRecordStepByIdQuery(id));
            return Ok(result);
        }

        [HttpPut("UpdateServiceRecordStep")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> UpdateServiceRecordStep([FromBody] UpdateServiceRecordStepDto updateServiceRecordStepDto)
        {
            updateServiceRecordStepDto.PersonnelId = CurrentPersonnelId;
            var command = Mapper.Map<UpdateServiceRecordStepCommand>(updateServiceRecordStepDto);
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

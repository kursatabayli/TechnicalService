using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TechnicalService.Application.Features.CQRS.Personnels.Commands;
using TechnicalService.Application.Features.CQRS.Personnels.Queries;
using TechnicalService.DTOs.DTOs.PersonnelDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly IMediator Mediator;
        private readonly IMapper Mapper;

        public PersonnelController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
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

        [HttpGet("GetAllPersonnels")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> GetAllPersonnels()
        {
            var result = await Mediator.Send(new GetAllPersonnelsQuery());
            return Ok(result);
        }
        [HttpGet("GetPersonnelsByService")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> GetPersonnelsByService()
        {
            var result = await Mediator.Send(new GetPersonnelsByServiceQuery(CurrentPersonnelId));
            return Ok(result);
        }

        [HttpPost("CreatePersonnel")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> CreatePersonnel(CreatePersonnelDto dto)
        {
            var command = Mapper.Map<CreatePersonnelCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("GetPersonnelById/{Id}")]
        [Authorize(Policy = nameof(AppPolicies.ManagementAccess))]
        public async Task<IActionResult> GetPersonnelById(Guid Id)
        {
            var result = await Mediator.Send(new GetPersonnelByIdQuery(Id));
            return Ok(result);
        }

        [HttpGet("CurrentPersonnel")]
        [Authorize(Policy = nameof(AppPolicies.AllEmployees))]
        public async Task<IActionResult> CurrentPersonnel()
        {
            var result = await Mediator.Send(new GetPersonnelByIdQuery(CurrentPersonnelId));
            return Ok(result);
        }

        [HttpPut("UpdatePersonnel")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> UpdatePersonnel(UpdatePersonnelDto dto)
        {
            var command = Mapper.Map<UpdatePersonnelCommand>(dto);
            var result = await Mediator.Send(command);
            return Ok(result);
        }

    }
}

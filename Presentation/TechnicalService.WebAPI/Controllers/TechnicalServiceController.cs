using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Commands;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Queries;
using TechnicalService.DTOs.DTOs.TechnicalServiceDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicalServiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TechnicalServiceController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllTechnicalServices")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllTechnicalServicesQuery());
            return Ok(result);
        }

        [HttpGet("GetTechnicalServiceById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetTechnicalServiceByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateTechnicalService")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Create(CreateTechnicalServiceDto dto)
        {
            var command = _mapper.Map<CreateTechnicalServiceCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateTechnicalService")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Update(UpdateTechnicalServiceDto dto)
        {
            var command = _mapper.Map<UpdateTechnicalServiceCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteTechnicalService/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteTechnicalServiceCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

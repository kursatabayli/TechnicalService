using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Commands;
using TechnicalService.Application.Features.CQRS.SerialNumbers.Queries;
using TechnicalService.DTOs.DTOs.SerialNumberDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SerialNumberController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SerialNumberController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllSerialNumbers")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> GetAllSerialNumbers()
        {
            var result = await _mediator.Send(new GetAllSerialNumbersQuery());
            return Ok(result);
        }

        [HttpGet("GetSerialNumberById/{id}")]
        public async Task<IActionResult> GetSerialNumberById(int id)
        {
            var result = await _mediator.Send(new GetSerialNumberByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateSerialNumber")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> CreateSerialNumber(CreateSerialNumberDto dto)
        {
            var command = _mapper.Map<CreateSerialNumberCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateSerialNumber")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> UpdateSerialNumber(UpdateSerialNumberDto dto)
        {
            var command = _mapper.Map<UpdateSerialNumberCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteSerialNumber/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> DeleteSerialNumber(int id)
        {
            var command = new DeleteSerialNumberCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

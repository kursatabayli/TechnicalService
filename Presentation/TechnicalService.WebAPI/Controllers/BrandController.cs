using TechnicalService.Application.Features.CQRS.Brands.Commands;
using TechnicalService.Application.Features.CQRS.Brands.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public BrandController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllBrands")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBrandQuery());
            return Ok(result);
        }

        [HttpGet("GetBrandById/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBrandByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateBrand")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Create(CreateBrandDto dto)
        {
            var command = _mapper.Map<CreateBrandCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateBrand")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Update(UpdateBrandDto dto)
        {
            var command = _mapper.Map<UpdateBrandCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteBrand/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBrandCommand(id));
            return Ok(result);
        }
    }
}

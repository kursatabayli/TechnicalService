using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.ProductTypes.Commands;
using TechnicalService.Application.Features.CQRS.ProductTypes.Queries;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductTypeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllProductTypes")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> GetProductTypes()
        {
            var result = await _mediator.Send(new GetAllProductTypeQuery());
            return Ok(result);
        }

        [HttpGet("GetProductTypeById/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> GetProductTypeById(int id)
        {
            var result = await _mediator.Send(new GetProductTypeByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateProductType")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> CreateProductType(CreateProductTypeDto dto)
        {
            var command = _mapper.Map<CreateProductTypeCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateProductType")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> UpdateProductType(UpdateProductTypeDto dto)
        {
            var command = _mapper.Map<UpdateProductTypeCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteProductType/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> DeleteProductType(int id)
        {
            var result = await _mediator.Send(new DeleteProductTypeCommand(id));
            return Ok(result);
        }
    }
}

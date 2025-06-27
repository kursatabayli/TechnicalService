using TechnicalService.Application.Features.CQRS.Products.Commands;
using TechnicalService.Application.Features.CQRS.Products.Queries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.DTOs.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductQuery());
            return Ok(result);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            return Ok(result);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var command = _mapper.Map<CreateProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var command = _mapper.Map<UpdateProductCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteProduct/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return Ok(result);
        }
    }
}

using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Commands;
using TechnicalService.Application.Features.CQRS.LegalDocuments.Queries;
using TechnicalService.DTOs.DTOs.LegalDocumentDTOs;
using TechnicalService.DTOs.Enums;

namespace TechnicalService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LegalDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public LegalDocumentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("GetAllLegalDocuments")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllLegalDocuments()
        {
            var result = await _mediator.Send(new GetAllLegalDocumentsQuery());
            return Ok(result);
        }

        [HttpGet("GetLegalDocumentById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLegalDocumentById(int id)
        {
            var result = await _mediator.Send(new GetLegalDocumentByIdQuery(id));
            return Ok(result);
        }

        [HttpGet("GetLegalDocumentByDocumentType/{documentType}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLegalDocumentByDocumentType(int documentType)
        {
            var result = await _mediator.Send(new GetLegalDocumentByDocumentTypeQuery(documentType));
            return Ok(result);
        }

        [HttpPost("CreateLegalDocument")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> CreateLegalDocument(CreateLegalDocumentDto dto)
        {
            var command = _mapper.Map<CreateLegalDocumentCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateLegalDocument")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> UpdateLegalDocument(UpdateLegalDocumentDto dto)
        {
            var command = _mapper.Map<UpdateLegalDocumentCommand>(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteLegalDocument/{id}")]
        [Authorize(Policy = nameof(AppPolicies.AdminOnly))]
        public async Task<IActionResult> DeleteLegalDocument(int id)
        {
            var command = new DeleteLegalDocumentCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}

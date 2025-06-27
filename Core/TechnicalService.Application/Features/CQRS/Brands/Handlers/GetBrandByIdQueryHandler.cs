using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Brands.Queries;
using TechnicalService.Application.Features.CQRS.Brands.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Handlers
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, Result<BrandResult>>
    {
        private readonly IRepository<Brand, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBrandByIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public GetBrandByIdQueryHandler(IRepository<Brand, int> repository, IMapper mapper, ILogger<GetBrandByIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<BrandResult>> Handle(GetBrandByIdQuery request, CancellationToken ct)
        {
            var brand = await _repository.GetByIdAsync(request.Id);

            var entityName = _returnMessages[ReturnMessages.EntityType_Brand];

            if (brand == null)
                return Result<BrandResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedBrand = _mapper.Map<BrandResult>(brand);
                return Result<BrandResult>.Success(mappedBrand, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);

                return Result<BrandResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

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
    public class GetAllBrandQueryHandler : IRequestHandler<GetAllBrandQuery, Result<List<BrandResult>>>
    {
        private readonly IRepository<Brand, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllBrandQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllBrandQueryHandler(IRepository<Brand, int> repository, IMapper mapper, ILogger<GetAllBrandQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<BrandResult>>> Handle(GetAllBrandQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_Brand_Plural];

            try
            {
                var brands = await _repository.GetAllAsync();
                var mappedBrands = _mapper.Map<List<BrandResult>>(brands);

                return Result<List<BrandResult>>.Success(mappedBrands, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);

                return Result<List<BrandResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

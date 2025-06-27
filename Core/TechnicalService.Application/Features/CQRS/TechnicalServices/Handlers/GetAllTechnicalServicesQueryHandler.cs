using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Queries;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Results;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Handlers
{
    public class GetAllTechnicalServicesQueryHandler : IRequestHandler<GetAllTechnicalServicesQuery, Result<List<TechnicalServiceResult>>>
    {
        private readonly IRepository<Domain.Entities.TechnicalService, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllTechnicalServicesQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetAllTechnicalServicesQueryHandler(IRepository<Domain.Entities.TechnicalService, int> repository, IMapper mapper, ILogger<GetAllTechnicalServicesQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<List<TechnicalServiceResult>>> Handle(GetAllTechnicalServicesQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_TechnicalService_Plural];

            try
            {
                var technicalServices = await _repository.GetAllAsync();
                var mappedTechnicalServices = _mapper.Map<List<TechnicalServiceResult>>(technicalServices);
                return Result<List<TechnicalServiceResult>>.Success(mappedTechnicalServices, _returnMessages[ReturnMessages.Message_Success_Listed, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_List, entityName]);
                return Result<List<TechnicalServiceResult>>.Failure(_returnMessages[ReturnMessages.Error_Operation_List, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

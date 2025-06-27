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
    internal class GetTechnicalServiceByIdQueryHandler : IRequestHandler<GetTechnicalServiceByIdQuery, Result<TechnicalServiceResult>>
    {
        private readonly IRepository<Domain.Entities.TechnicalService, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTechnicalServiceByIdQueryHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public GetTechnicalServiceByIdQueryHandler(IRepository<Domain.Entities.TechnicalService, int> repository, IMapper mapper, ILogger<GetTechnicalServiceByIdQueryHandler> logger, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _returnMessages = returnMessages;
        }

        public async Task<Result<TechnicalServiceResult>> Handle(GetTechnicalServiceByIdQuery request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_TechnicalService];

            var technicalService = await _repository.GetByIdAsync(request.Id);

            if (technicalService == null)
                return Result<TechnicalServiceResult>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            try
            {
                var mappedTechnicalService = _mapper.Map<TechnicalServiceResult>(technicalService);
                return Result<TechnicalServiceResult>.Success(mappedTechnicalService, _returnMessages[ReturnMessages.Message_Success_Retrieved, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName], request.Id);
                return Result<TechnicalServiceResult>.Failure(_returnMessages[ReturnMessages.Error_Operation_Retrieve, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

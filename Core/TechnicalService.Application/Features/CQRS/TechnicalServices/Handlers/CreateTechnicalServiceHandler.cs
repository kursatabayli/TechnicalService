using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.TechnicalServices.Commands;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.TechnicalServices.Handlers
{
    public class CreateTechnicalServiceHandler : IRequestHandler<CreateTechnicalServiceCommand, Result<int>>
    {
        private readonly IRepository<Domain.Entities.TechnicalService, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTechnicalServiceHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public CreateTechnicalServiceHandler(IRepository<Domain.Entities.TechnicalService, int> repository, IMapper mapper, ILogger<CreateTechnicalServiceHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }
        public async Task<Result<int>> Handle(CreateTechnicalServiceCommand request, CancellationToken ct)
        {
            var technicalService = _mapper.Map<Domain.Entities.TechnicalService>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_TechnicalService];

            // var existingTechnicalService = await _repository.GetFirstOrDefaultAsync(ts => ts.ServiceName.ToLower() == technicalService.ServiceName.ToLower());
            // if (existingTechnicalService != null)
            //    return Result<int>.Failure(existingTechnicalService.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, existingTechnicalService.ServiceName, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                await _repository.CreateAsync(technicalService);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(technicalService.Id, _returnMessages[ReturnMessages.Message_Success_Created, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName]);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

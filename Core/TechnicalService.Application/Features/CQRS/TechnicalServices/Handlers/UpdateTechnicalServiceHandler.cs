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
    public class UpdateTechnicalServiceHandler : IRequestHandler<UpdateTechnicalServiceCommand, Result<int>>
    {
        private readonly IRepository<Domain.Entities.TechnicalService, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTechnicalServiceHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public UpdateTechnicalServiceHandler(IRepository<Domain.Entities.TechnicalService, int> repository, IMapper mapper, ILogger<UpdateTechnicalServiceHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateTechnicalServiceCommand request, CancellationToken ct)
        {
            var technicalService = await _repository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_TechnicalService];

            if (technicalService == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            // var existingTechnicalServiceWithSameName = await _repository.GetFirstOrDefaultAsync(ts => ts.Name.ToLower() == request.Name.ToLower() && ts.Id != request.Id);
            // if (existingTechnicalServiceWithSameName != null)
            //    return Result<int>.Failure(existingTechnicalServiceWithSameName.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.Name, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, technicalService);
                await _repository.UpdateAsync(technicalService);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Success(technicalService.Id, _returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

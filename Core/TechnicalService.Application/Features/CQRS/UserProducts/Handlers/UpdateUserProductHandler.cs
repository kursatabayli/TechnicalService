using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.UserProducts.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.UserProducts.Handlers
{
    public class UpdateUserProductHandler : IRequestHandler<UpdateUserProductCommand, Result<int>>
    {
        private readonly IRepository<UserProduct, int> _repository;
        private readonly ILogger<UpdateUserProductCommand> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public UpdateUserProductHandler(IRepository<UserProduct, int> repository, ILogger<UpdateUserProductCommand> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateUserProductCommand request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct];
            var userProduct = await _repository.GetByIdAsync(request.Id);

            if (userProduct == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);


            try
            {
                _mapper.Map(request, userProduct);
                await _repository.UpdateAsync(userProduct);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(userProduct.Id, _returnMessages[ReturnMessages.Message_Success_Updated, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

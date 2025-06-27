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
    public class AddUserProductHandler : IRequestHandler<AddUserProductCommand, Result<int>>
    {
        private readonly IRepository<UserProduct, int> _repository;
        private readonly ISerialNumberRepository _serialNumberRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AddUserProductHandler> _logger;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public AddUserProductHandler(IRepository<UserProduct, int> repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<AddUserProductHandler> logger, ISerialNumberRepository serialNumberRepository, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _serialNumberRepository = serialNumberRepository;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(AddUserProductCommand request, CancellationToken ct)
        {
            var entityName = _returnMessages[ReturnMessages.EntityType_UserProduct];

            var serialNumberDetail = await _serialNumberRepository.GetSerialNumberBySerialNumberAsync(request.SerialNumber);

            if (serialNumberDetail == null || request.PurchaseDate < serialNumberDetail.RegisterDate)
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_UserProduct_Mismatch], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingUserProductWithSameSerialNumber = await _repository.GetFirstOrDefaultAsync(up => up.SerialNumberId == serialNumberDetail.Id);

            if (existingUserProductWithSameSerialNumber != null)
                return Result<int>.Failure(existingUserProductWithSameSerialNumber.Id, _returnMessages[ReturnMessages.Error_UserProduct_AlreadyRegistered, request.SerialNumber, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                var userProductToCreate = _mapper.Map<UserProduct>(request);
                userProductToCreate.SerialNumberId = serialNumberDetail.Id;
                userProductToCreate.WarrantyDate = request.PurchaseDate.AddMonths(serialNumberDetail.Product.WarrantyPeriod);

                await _repository.CreateAsync(userProductToCreate);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Success(userProductToCreate.Id, _returnMessages[ReturnMessages.Message_Success_UserProduct_Added, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName], request.SerialNumber);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

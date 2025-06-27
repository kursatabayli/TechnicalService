using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Brands.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Brands.Handlers
{
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand, Result<int>>
    {
        private readonly IRepository<Brand, int> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateBrandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;
        public UpdateBrandHandler(IRepository<Brand, int> repository, IMapper mapper, ILogger<UpdateBrandHandler> logger, IUnitOfWork unitOfWork, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateBrandCommand request, CancellationToken ct)
        {
            var brand = await _repository.GetByIdAsync(request.Id);

            var entityName = _returnMessages[ReturnMessages.EntityType_Brand];

            if (brand == null)
                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingBrandWithSameName = await _repository.GetFirstOrDefaultAsync(x => x.BrandName == request.BrandName && x.Id != request.Id);

            if (existingBrandWithSameName != null)
                return Result<int>.Failure(existingBrandWithSameName.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.BrandName, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, brand);
                await _repository.UpdateAsync(brand);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Success(brand.Id, _returnMessages[ReturnMessages.Message_Success_Updated_WithName, brand.BrandName, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);

                return Result<int>.Failure(request.Id, _returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

using TechnicalService.Application.Features.CQRS.Brands.Commands;
using AutoMapper;
using TechnicalService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.DTOs.Results;
using System.Net;
using TechnicalService.DTOs.Enums;
using Microsoft.Extensions.Localization;
using TechnicalService.Application.Extensions;

namespace TechnicalService.Application.Features.CQRS.Brands.Handlers
{
    public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, Result<int>>
    {
        private readonly IRepository<Brand, int> _repository;
        private readonly ILogger<CreateBrandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public CreateBrandHandler(IRepository<Brand, int> repository, ILogger<CreateBrandHandler> logger, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _repository = repository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Brand>(request);

            var existingBrand = await _repository.GetFirstOrDefaultAsync(x => x.BrandName == brand.BrandName);

            var entityName = _returnMessages[ReturnMessages.EntityType_Brand];

            if (existingBrand != null)
                return Result<int>.Failure(existingBrand.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, existingBrand.BrandName, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                await _repository.CreateAsync(brand);
                await _unitOfWork.SaveChangesAsync();

                return Result<int>.Success(brand.Id, _returnMessages[ReturnMessages.Message_Success_Created_WithName, brand.BrandName, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName], request.BrandName);

                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

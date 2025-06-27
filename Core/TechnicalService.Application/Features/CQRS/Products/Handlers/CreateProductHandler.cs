using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Net;
using TechnicalService.Application.Contracts.RepositoryContracts;
using TechnicalService.Application.Contracts.ServicesContracts;
using TechnicalService.Application.Extensions;
using TechnicalService.Application.Features.CQRS.Products.Commands;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.Enums;
using TechnicalService.DTOs.Results;

namespace TechnicalService.Application.Features.CQRS.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<int>>
    {
        private readonly ILogger<CreateProductHandler> _logger;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public CreateProductHandler(ILogger<CreateProductHandler> logger, IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _logger = logger;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var product = _mapper.Map<Product>(request);
            var entityName = _returnMessages[ReturnMessages.EntityType_Product];

            var existingProduct = await _productRepository.GetFirstOrDefaultAsync(p => p.ProductName.ToLower() == product.ProductName.ToLower() &&
                                                                            p.BrandId == product.BrandId &&
                                                                            p.ProductTypeId == product.ProductTypeId);

            if (existingProduct != null)
                return Result<int>.Failure(existingProduct.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.ProductName, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                await _productRepository.CreateAsync(product);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(product.Id, _returnMessages[ReturnMessages.Message_Success_Created_WithName, product.ProductName, entityName], HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Create, entityName], request.ProductName);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Create, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

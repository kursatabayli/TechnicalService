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
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<int>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ReturnMessages> _returnMessages;

        public UpdateProductHandler(IRepository<Product, int> repository, IMapper mapper, ILogger<UpdateProductHandler> logger, IUnitOfWork unitOfWork, IProductRepository productRepository, IStringLocalizer<ReturnMessages> returnMessages)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _returnMessages = returnMessages;
        }

        public async Task<Result<int>> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            var entityName = _returnMessages[ReturnMessages.EntityType_Product];

            if (product == null)
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Entity_NotFound, entityName], StatusCode.NotFound, HttpStatusCode.NotFound);

            var existingProduct = await _productRepository.GetFirstOrDefaultAsync(p =>
                                                    p.ProductName.ToLower() == request.ProductName.ToLower() &&
                                                    p.BrandId == request.BrandId &&
                                                    p.ProductTypeId == request.ProductTypeId &&
                                                    p.Id != request.Id);

            if (existingProduct != null)
                return Result<int>.Failure(existingProduct.Id, _returnMessages[ReturnMessages.Error_Entity_AlreadyExists_WithName, request.ProductName, entityName], StatusCode.Conflict, HttpStatusCode.Conflict);

            try
            {
                _mapper.Map(request, product);
                await _productRepository.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();
                return Result<int>.Success(product.Id, _returnMessages[ReturnMessages.Message_Success_Updated_WithName, product.ProductName, entityName], HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, _returnMessages[ReturnMessages.Error_Operation_Update, entityName], request.Id);
                return Result<int>.Failure(_returnMessages[ReturnMessages.Error_Operation_Update, entityName, ReturnMessages.Error_Support_Contact_Message], StatusCode.InternalServerError, HttpStatusCode.InternalServerError);
            }
        }
    }
}

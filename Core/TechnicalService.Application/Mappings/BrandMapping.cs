using AutoMapper;
using TechnicalService.Application.Features.CQRS.Brands.Commands;
using TechnicalService.Application.Features.CQRS.Brands.Results;
using TechnicalService.Domain.Entities;
using TechnicalService.DTOs.DTOs.BrandDTOs;

namespace TechnicalService.Application.Mappings
{
    public class BrandMapping : Profile
    {
        public BrandMapping()
        {
            CreateMap<Brand, BrandResult>();
            CreateMap<BrandResult, BrandDto>();
            CreateMap<CreateBrandDto, CreateBrandCommand>();
            CreateMap<CreateBrandCommand, Brand>();
            CreateMap<BrandResult, UpdateBrandCommand>();
            CreateMap<UpdateBrandDto, UpdateBrandCommand>();
            CreateMap<UpdateBrandCommand, Brand>();
        }
    }
}

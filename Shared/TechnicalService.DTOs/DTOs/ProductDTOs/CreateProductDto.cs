using TechnicalService.DTOs.DTOs.BrandDTOs;
using TechnicalService.DTOs.DTOs.ProductTypeDTOs;

namespace TechnicalService.DTOs.DTOs.ProductDTOs
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public BrandDto Brand { get; set; }
        public int BrandId { get; set; }
        public ProductTypeDto ProductType { get; set; }
        public int ProductTypeId { get; set; }
    }
}

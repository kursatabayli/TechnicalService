namespace TechnicalService.DTOs.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public byte WarrantyPeriod { get; set; } // in months
    }
}

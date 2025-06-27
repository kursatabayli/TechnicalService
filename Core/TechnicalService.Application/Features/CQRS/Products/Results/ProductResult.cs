namespace TechnicalService.Application.Features.CQRS.Products.Results
{
    public class ProductResult
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public int BrandId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductName { get; set; }
        public byte WarrantyPeriod { get; set; } // in months
    }
}

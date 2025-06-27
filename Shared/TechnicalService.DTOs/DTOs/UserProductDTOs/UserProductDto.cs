namespace TechnicalService.DTOs.DTOs.UserProductDTOs
{
    public class UserProductDto
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public DateOnly RegisterDate { get; set; }
        public Guid UserId { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly WarrantyDate { get; set; }
    }
}

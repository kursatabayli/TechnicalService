namespace TechnicalService.DTOs.DTOs.UserProductDTOs
{
    public class AddUserProductDto
    {
        public string SerialNumber { get; set; }
        public Guid UserId { get; set; }
        public DateTime? PurchaseDate { get; set; }
    }
}

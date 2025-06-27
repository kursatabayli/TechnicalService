using TechnicalService.DTOs.DTOs.ProductDTOs;

namespace TechnicalService.DTOs.DTOs.SerialNumberDTOs
{
    public class UpdateSerialNumberDto
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public ProductDto Product { get; set; }
        public int ProductId { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}

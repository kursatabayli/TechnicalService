namespace TechnicalService.DTOs.DTOs.SerialNumberDTOs
{
    public class SerialNumberDto
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public DateOnly RegisterDate { get; set; }
    }
}

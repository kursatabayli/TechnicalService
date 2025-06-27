namespace TechnicalService.Domain.Entities
{
    public class SerialNumber
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public DateOnly RegisterDate { get; set; }
        public List<UserProduct>? UserProducts { get; set; }
    }
}

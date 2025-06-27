namespace TechnicalService.Domain.Entities
{
    public class UserProduct
    {
        public int Id { get; set; }
        public SerialNumber SerialNumber { get; set; }
        public int SerialNumberId { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public DateOnly PurchaseDate { get; set; }
        public DateOnly WarrantyDate { get; set; } // in months

        public List<ServiceRecord>? RepairRequests { get; set; }

    }
}

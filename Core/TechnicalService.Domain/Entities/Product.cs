using System.ComponentModel.DataAnnotations;

namespace TechnicalService.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public int BrandId { get; set; }
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }
        [MaxLength(20)]
        public string ProductName { get; set; }
        public byte WarrantyPeriod { get; set; } // in months
        public List<SerialNumber>? SerialNumbers { get; set; }
    }
}

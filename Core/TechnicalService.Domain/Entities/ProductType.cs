using System.ComponentModel.DataAnnotations;

namespace TechnicalService.Domain.Entities
{
    public class ProductType
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string Type { get; set; }
        public List<Product> Products { get; set; }
    }
}

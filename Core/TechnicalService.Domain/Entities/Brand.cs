using System.ComponentModel.DataAnnotations;

namespace TechnicalService.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        [MaxLength(20)]
        public string BrandName { get; set; }
        public List<Product> Products { get; set; }
    }
}

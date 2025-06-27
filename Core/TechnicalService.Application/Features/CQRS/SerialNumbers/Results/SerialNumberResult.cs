using TechnicalService.Application.Features.CQRS.Products.Results;

namespace TechnicalService.Application.Features.CQRS.SerialNumbers.Results
{
    public class SerialNumberResult
    {
        public int Id { get; set; }
        public string Serial_Number { get; set; }
        public string BrandName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public DateOnly RegisterDate { get; set; }
    }
}

namespace TechnicalService.DTOs.DTOs.TechnicalServiceDTOs
{
    public class TechnicalServiceDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
    }
}

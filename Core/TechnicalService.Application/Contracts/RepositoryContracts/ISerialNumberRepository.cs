using TechnicalService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalService.Application.Contracts.RepositoryContracts
{
    public interface ISerialNumberRepository
    {
        Task<List<SerialNumber>> GetAllSerialNumbersAsync();
        Task<SerialNumber> GetSerialNumberByIdAsync(int id);
        Task<SerialNumber> GetSerialNumberBySerialNumberAsync(string serialNumber);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalService.Application.Contracts.ServicesContracts
{
    public interface ISmsService
    {
        Task<bool> SendVerificationCode(string phoneNumber, int verificationCode);
    }
}

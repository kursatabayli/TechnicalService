using System.ComponentModel;

namespace TechnicalService.Domain.Enums
{
    public enum ServiceStatus
    {
        [Description("Beklemede")]
        Pending,

        [Description("Onarılıyor")]
        InProgress,

        [Description("Tamamlandı")]
        Completed
    }
}

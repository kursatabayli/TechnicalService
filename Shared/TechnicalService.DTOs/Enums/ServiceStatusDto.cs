using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum ServiceStatusDto
    {
        [Description("Beklemede")]
        Pending,

        [Description("Onarılıyor")]
        InProgress,

        [Description("Tamamlandı")]
        Completed
    }
}

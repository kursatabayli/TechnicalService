using System.ComponentModel;

namespace TechnicalService.Domain.Enums
{
    public enum PersonnelStatus
    {
        [Description("Aktif")]
        Active,

        [Description("İzinli")]
        OnLeave,

        [Description("İşten Ayrıldı")]
        Terminated,

        [Description("Askıya Alındı")]
        Suspended
    }
}

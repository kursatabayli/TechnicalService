using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum PersonnelStatusDto
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

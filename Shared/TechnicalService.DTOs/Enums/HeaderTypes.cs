using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum HeaderTypes
    {
        [Description("X-Client-Type")]
        HeaderKey,

        Personnel,
        User,
    }
}

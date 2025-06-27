using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum GenderDto
    {
        [Description("Erkek")]
        Male,

        [Description("Kız")]
        Female,

        [Description("Belirtmek İstemiyorum")]
        Empty
    }
}

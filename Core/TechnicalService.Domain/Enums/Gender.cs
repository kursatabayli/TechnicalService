using System.ComponentModel;

namespace TechnicalService.Domain.Enums
{
    public enum Gender
    {
        [Description("Erkek")]
        Male,

        [Description("Kız")]
        Female,

        [Description("Belirtmek İstemiyorum")]
        Empty
    }
}

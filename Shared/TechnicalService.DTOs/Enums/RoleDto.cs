using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum RoleDto
    {
        [Description("Sistem Yöneticisi")]
        Admin,

        [Description("Müdür")]
        Manager,

        [Description("Teknisyen")]
        Technician,

        [Description("Müşteri Hizmetleri")]
        CustomerService,

        [Description("Kullanıcı")]
        User,
    }
}

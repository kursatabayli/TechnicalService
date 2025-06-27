using System.ComponentModel;

namespace TechnicalService.Domain.Enums
{
    public enum Role
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

using System.ComponentModel;

namespace TechnicalService.DTOs.Enums
{
    public enum DocumentTypeDto
    {
        [Description("Kullanım Koşulları ve Üyelik Sözleşmesi")]
        TermsOfService,
        [Description("Gizlilik Politikası ve KVKK Aydınlatma Metni")]
        PrivacyAndPdplPolicy,
        [Description("Çerez Politikası")]
        CookiePolicy,
        [Description("Pazarlama İletişimi Açık Rıza Metni")]
        ExplicitConsentForMarketing
    }
}

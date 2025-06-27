using System.ComponentModel;

namespace TechnicalService.Domain.Enums
{
    public enum DocumentType
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

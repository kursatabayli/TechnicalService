namespace TechnicalService.DTOs.Enums
{
    public enum StatusCode
    {
        None = 0,              // Belirtilmemiş veya başlangıç durumu

        // --- Başarı Durumları (2xxx) ---
        Ok = 2000,             // Genel başarı (HTTP 200 OK gibi)
        Created = 2001,        // Kaynak oluşturuldu (HTTP 201 Created gibi)
        Accepted = 2002,       // İstek kabul edildi, işlem sürüyor (HTTP 202 Accepted gibi) - Uzun süren işlemler için
        NoContent = 2004,      // Başarılı istek, yanıt içeriği yok (HTTP 204 No Content gibi) - Silme işlemi sonrası vb.

        // --- İstemci Hataları (4xxx) ---
        BadRequest = 4000,         // Genel istemci hatası (HTTP 400 Bad Request) - İstek formatı bozuk vb.
        ValidationError = 4001,    // Doğrulama hatası (Genellikle 400 Bad Request ile birlikte kullanılır)
        InvalidToken = 4002,       // Geçersiz Token (Formatı bozuk, anlaşılamayan token - HTTP 401 Unauthorized'a map edilebilir)
        Unauthorized = 4003,       // Yetkisiz (Giriş yapılmamış veya geçersiz/süresi dolmuş token - HTTP 401 Unauthorized)
        Forbidden = 4004,          // Yasaklı (Giriş yapılmış ama kaynağa erişim yetkisi yok - HTTP 403 Forbidden)
        NotFound = 4005,           // Kaynak bulunamadı (HTTP 404 Not Found)
        NotAcceptable = 4006,      // Kabul Edilemez İstek başlığı (HTTP 406 Not Acceptable)
        MethodNotAllowed = 4007,   // İzin Verilmeyen Metot (HTTP 405 Method Not Allowed)
        Conflict = 4009,           // Çakışma (Kaynak zaten var veya durum çakışması - HTTP 409 Conflict) - UserAlreadyExists bunun özel bir hali
        Gone = 4010,               // Kaynak Artık Mevcut Değil (Örn: Süresi dolmuş link - HTTP 410 Gone) - Token süresi dolması için alternatif
        UnsupportedMediaType = 4015,// Desteklenmeyen Medya Tipi (HTTP 415 Unsupported Media Type)
        TooManyRequests = 4029,    // Çok Fazla İstek (Rate Limit - HTTP 429 Too Many Requests)

        // --- Özel İstemci Uygulama Hataları (41xx) ---
        InvalidCredentials = 4101, // Geçersiz kullanıcı adı/şifre (Özel kod)
        EmailNotVerified = 4102,   // E-posta doğrulanmamış (Özel kod)
        UserAlreadyExists = 4103,  // Kullanıcı zaten var (Özel kod - 4009 Conflict'in spesifik hali)
        TokenExpired = 4104,       // Token'ın süresi dolmuş (Özel kod - 4003 Unauthorized veya 4010 Gone'a map edilebilir)

        // --- Sunucu Hataları (5xxx) ---
        InternalServerError = 5000,// Genel sunucu hatası (HTTP 500 Internal Server Error)
        DatabaseError = 5001,      // Veritabanı hatası (Özel kod)
        TokenGenerationFailed = 5002,// Token üretme hatası (Özel kod)
        ServiceUnavailable = 5003, // Servis Kullanılamıyor (Geçici - HTTP 503 Service Unavailable)
        ExternalServiceError = 5004// Harici servis hatası (Özel kod)
    }
}

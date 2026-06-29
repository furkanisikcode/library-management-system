# 📚 Library Management System

> ASP.NET Core 10, Entity Framework Core ve PostgreSQL ile geliştirilmiş, **katmanlı mimari** ve **SOLID prensipleri**ne sıkı sıkıya bağlı, kurumsal düzey bir kütüphane yönetim sistemi REST API'si.

![Status](https://img.shields.io/badge/status-complete-success)
![.NET](https://img.shields.io/badge/.NET-10-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-blue)
![JWT](https://img.shields.io/badge/Auth-JWT-yellow)

Bu proje sadece çalışan bir uygulama değil; aynı zamanda profesyonel bir backend projesinin nasıl yapılandırılması gerektiğine dair bir referanstır. Her katmanın tek bir sorumluluğu vardır, bağımlılıklar tek yönlüdür ve her detay test edilebilir, genişletilebilir, sürdürülebilir ve güvenli olacak şekilde tasarlanmıştır.

---

## 🛠️ Teknolojiler

- **.NET 10** (LTS)
- **ASP.NET Core Web API**
- **Entity Framework Core 10**
- **PostgreSQL** (Npgsql provider)
- **AutoMapper** (Entity ↔ DTO dönüşümü)
- **FluentValidation** (Model doğrulama)
- **JWT Bearer Authentication** (Microsoft.AspNetCore.Authentication.JwtBearer)
- **BCrypt.Net-Next** (Parola hashleme)
- **Serilog** (Yapılandırılmış loglama, dosyaya yazma)
- **Swagger / OpenAPI** (API dokümantasyonu)

---

## 🏗️ Mimari

4 katmanlı solution yapısı:
LibraryManagement/

├── LibraryManagement.Entities      → Domain modelleri

├── LibraryManagement.DataAccess    → DbContext, Repository, Migrations

├── LibraryManagement.Business      → Service, DTO, Validation, Mapping, Settings, Exceptions, Auth

└── LibraryManagement.WebAPI        → Controller, Middleware, REST endpoints

**Bağımlılık yönü:** `WebAPI → Business → DataAccess → Entities`

---

## ✨ Özellikler

- ✅ Generic + Specific Repository Pattern
- ✅ DTO ile veri taşıma (List, Detail, Create, Update)
- ✅ AutoMapper Profile'larla mapping
- ✅ FluentValidation ile doğrulama
- ✅ Configuration Pattern (Options) — iş kuralları `appsettings.json`'dan
- ✅ Custom Exception Hierarchy (NotFound, BusinessRule, Conflict)
- ✅ Global Exception Middleware (RFC 7807 Problem Details)
- ✅ Atomik transaction yönetimi
- ✅ Soft delete (`IsDeleted` bayrağı)
- ✅ UTC DateTime
- ✅ Dependency Injection (her katmanda)
- ✅ Async/Await (tüm DB işlemleri)
- ✅ Pagination (generic `PagedResult<T>`)
- ✅ **JWT Authentication & Authorization** 🔐
- ✅ **Role-Based Access Control** (Admin / Librarian / Member)
- ✅ **BCrypt parola hashleme**
- ✅ **Serilog Logging** (dosyaya yazma, günlük rotasyon)
- ✅ RESTful API + doğru HTTP status code'ları
- ✅ Swagger UI

---

## 🎯 Domain Modeli

- **Book** — Kitap (başlık, ISBN, stok)
- **Author** — Yazar
- **Category** — Kategori
- **Publisher** — Yayınevi
- **Member** — Üye (Admin/Librarian/Member rolleri ile)
- **Loan** — Ödünç işlemi
- **Penalty** — Ceza kayıtları (gecikme cezaları)

Tüm entity'ler `BaseEntity`'den miras alır (Id, CreatedDate, UpdatedDate, IsDeleted).

---

## 💼 İş Kuralları

`appsettings.json`'da yapılandırılabilir, Service katmanında uygulanan kurallar:

- Bir üye aynı anda en fazla **5 kitap** ödünç alabilir
- Ödünç alma süresi **14 gün**
- Geciken her gün için **5 TL** sabit ceza
- Stokta olmayan kitap ödünç verilemez
- Pasif üye ödünç alamaz
- Ödenmemiş cezası olan üye yeni kitap alamaz
- Email unique kontrolü
- Çift ödeme engeli
- İade sırasında gecikme varsa **otomatik ceza** oluşturulur
- Stok otomatik yönetimi (ödünç→azalır, iade→artar, atomik)

---

## 🔐 Authentication & Authorization

JWT Bearer token tabanlı kimlik doğrulama:

### Endpoints

- `POST /api/Auth/register` — Yeni hesap aç (her zaman Member rolü)
- `POST /api/Auth/login` — Giriş yap, token al

### Role-Based Access

| Rol | Yetki |
|-----|-------|
| **Admin** | Tam yetki: tüm CRUD, üye yönetimi, silme |
| **Librarian** | Kitap/yazar/kategori/yayınevi ekleme/güncelleme, ödünç verme, iade alma, ceza tahsil |
| **Member** | Sadece okuma (kitap arama, kendi ödünçlerini görme) |

### Endpoint Korumaları

- `[AllowAnonymous]` — Auth endpoint'leri
- `[Authorize]` — Giriş yapan herkes (GET endpoint'leri)
- `[Authorize(Roles = "Admin,Librarian")]` — Yönetim işlemleri
- `[Authorize(Roles = "Admin")]` — Hassas işlemler (silme, üye yönetimi)

### Güvenlik Özellikleri

- BCrypt ile parola hashleme (salt + cost factor)
- Token expiry kontrolü (60 dakika)
- Issuer/Audience doğrulaması
- HMAC-SHA256 imza algoritması

---

## 📝 Logging (Serilog)

- **Konsol + Dosya** loglama
- **Günlük dosya rotasyonu** (`logs/log-YYYYMMDD.txt`)
- **30 günlük geriye dönük saklama**
- **HTTP request loglaması** (URL, method, status, süre)
- **Yapılandırılmış format** (Timestamp, Level, Message, Exception)

Örnek log satırı:
2026-06-29 18:55:24.381 +03:00 [INF] HTTP GET /api/Books responded 200 in 25.4 ms

2026-06-29 18:58:42.123 +03:00 [WRN] İş kuralı hatası: 'Kar' kitabı şu anda stokta yok.

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/)

### Adımlar

```bash
git clone https://github.com/furkanisikcode/library-management-system.git
cd library-management-system

dotnet restore

# appsettings.json'daki connection string'i kendi şifrenle güncelle

dotnet ef database update --project src/LibraryManagement.DataAccess --startup-project src/LibraryManagement.WebAPI

dotnet run --project src/LibraryManagement.WebAPI
```

Swagger UI: `http://localhost:5245/swagger`

---

## ⚙️ Yapılandırma

`src/LibraryManagement.WebAPI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=YOUR_PASSWORD"
  },
  "LoanSettings": {
    "MaxActiveLoanPerMember": 5,
    "LoanDurationInDays": 14,
    "DailyPenaltyAmount": 5.00
  },
  "JwtSettings": {
    "Secret": "BuCokGizliBirAnahtarOlmaliVeEnAz32KarakterUzunlugundaTutulmali_2026",
    "Issuer": "LibraryManagementAPI",
    "Audience": "LibraryManagementClient",
    "ExpirationInMinutes": 60
  }
}
```

---

## 📡 API Endpoint'leri (40+)

### Auth
- `POST /api/Auth/register` — Yeni hesap
- `POST /api/Auth/login` — Giriş

### Books, Authors, Categories, Publishers (Standart CRUD + Pagination)
- `GET    /api/{resource}` — Tümünü listele
- `GET    /api/{resource}/paged?pageNumber=1&pageSize=20` — Sayfalı liste
- `GET    /api/{resource}/{id}` — Detay
- `POST   /api/{resource}` — Yeni kayıt (Admin/Librarian)
- `PUT    /api/{resource}/{id}` — Güncelle (Admin/Librarian)
- `DELETE /api/{resource}/{id}` — Sil (Admin)

### Members (Admin Only)
- Yukarıdakiyle aynı + email unique kontrolü

### Loans (özel iş mantığı)
- `POST   /api/Loans/borrow` — Ödünç ver (Admin/Librarian)
- `POST   /api/Loans/return` — İade et — otomatik ceza (Admin/Librarian)

### Penalties
- `GET    /api/Penalties/member/{memberId}` — Üye ceza özeti
- `POST   /api/Penalties/{id}/pay` — Cezayı öde (Admin/Librarian)

---

## 🛡️ Hata Yönetimi

RFC 7807 Problem Details formatında hata cevapları:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Bulunamadı",
  "status": 404,
  "detail": "Id=999 olan kitap bulunamadı.",
  "traceId": "0HN07845G51HI:00000003"
}
```

| Hata | HTTP |
|------|------|
| NotFoundException | 404 |
| BusinessRuleException | 400 |
| ConflictException | 409 |
| Token yok/geçersiz | 401 Unauthorized |
| Yetkin yok | 403 Forbidden |
| FluentValidation | 400 |

---

## 🚧 Durum

- ✅ Tüm temel CRUD (5 entity)
- ✅ Loan sistemi (6 iş kuralı)
- ✅ Penalty sistemi (otomatik ceza, ödeme, üye özeti)
- ✅ Global Exception Handling
- ✅ Pagination
- ✅ **JWT Authentication & Role-Based Authorization**
- ✅ **Serilog Logging**
- ⏳ Unit ve Integration testler (gelecek geliştirme)
- ⏳ Refresh Token (gelecek geliştirme)

---

## 👤 Geliştirici

**[@furkanisikcode](https://github.com/furkanisikcode)**

---

## 📝 Lisans

Bu proje öğrenim amaçlı geliştirilmiştir.
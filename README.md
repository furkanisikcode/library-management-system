# 📚 Library Management System

> ASP.NET Core 10, Entity Framework Core ve PostgreSQL ile geliştirilmiş, **katmanlı mimari** ve **SOLID prensipleri**ne sıkı sıkıya bağlı, kurumsal düzey bir kütüphane yönetim sistemi REST API'si.

![Status](https://img.shields.io/badge/status-complete-success)
![.NET](https://img.shields.io/badge/.NET-10-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-blue)

Bu proje sadece çalışan bir uygulama değil; aynı zamanda profesyonel bir backend projesinin nasıl yapılandırılması gerektiğine dair bir referanstır. Her katmanın tek bir sorumluluğu vardır, bağımlılıklar tek yönlüdür ve her detay test edilebilir, genişletilebilir ve sürdürülebilir olacak şekilde tasarlanmıştır.

---

## 🛠️ Teknolojiler

- **.NET 10** (LTS)
- **ASP.NET Core Web API**
- **Entity Framework Core 10**
- **PostgreSQL** (Npgsql provider)
- **AutoMapper** (Entity ↔ DTO dönüşümü)
- **FluentValidation** (Model doğrulama)
- **Swagger / OpenAPI** (API dokümantasyonu)

---

## 🏗️ Mimari

4 katmanlı solution yapısı:

\`\`\`
LibraryManagement/
├── LibraryManagement.Entities      → Domain modelleri
├── LibraryManagement.DataAccess    → DbContext, Repository, Migrations
├── LibraryManagement.Business      → Service, DTO, Validation, Mapping, Settings, Exceptions
└── LibraryManagement.WebAPI        → Controller, Middleware, REST endpoints
\`\`\`

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
- ✅ UTC DateTime (zaman dilimine bağımsız)
- ✅ Dependency Injection (her katmanda)
- ✅ Async/Await (tüm DB işlemleri)
- ✅ Pagination (generic `PagedResult<T>`)
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
- Çift ödeme engeli (aynı ceza iki kez ödenemez)
- İade sırasında gecikme varsa **otomatik ceza** oluşturulur
- Stok otomatik yönetimi (ödünç→azalır, iade→artar, atomik)

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/)

### Adımlar

\`\`\`bash
git clone https://github.com/furkanisikcode/library-management-system.git
cd library-management-system

dotnet restore

# appsettings.json'daki connection string'i kendi şifrenle güncelle

dotnet ef database update --project src/LibraryManagement.DataAccess --startup-project src/LibraryManagement.WebAPI

dotnet run --project src/LibraryManagement.WebAPI
\`\`\`

Swagger UI: `http://localhost:5245/swagger`

---

## ⚙️ Yapılandırma

`src/LibraryManagement.WebAPI/appsettings.json`:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=YOUR_PASSWORD"
  },
  "LoanSettings": {
    "MaxActiveLoanPerMember": 5,
    "LoanDurationInDays": 14,
    "DailyPenaltyAmount": 5.00
  }
}
\`\`\`

---

## 📡 API Endpoint'leri (35+)

**Books, Authors, Categories, Publishers, Members:**
- `GET    /api/{resource}` — Tümünü listele
- `GET    /api/{resource}/paged?pageNumber=1&pageSize=20` — Sayfalı liste
- `GET    /api/{resource}/{id}` — Detay
- `POST   /api/{resource}` — Yeni kayıt
- `PUT    /api/{resource}/{id}` — Güncelle
- `DELETE /api/{resource}/{id}` — Sil (soft)

**Loans (özel iş mantığı):**
- `POST   /api/Loans/borrow` — Ödünç ver
- `POST   /api/Loans/return` — İade et (otomatik ceza)

**Penalties:**
- `GET    /api/Penalties/member/{memberId}` — Üye ceza özeti
- `POST   /api/Penalties/{id}/pay` — Cezayı öde

---

## 🛡️ Hata Yönetimi

RFC 7807 Problem Details formatında hata cevapları:

\`\`\`json
{
  "type": "https://httpstatuses.com/404",
  "title": "Bulunamadı",
  "status": 404,
  "detail": "Id=999 olan kitap bulunamadı.",
  "traceId": "0HN07845G51HI:00000003"
}
\`\`\`

| Hata | HTTP |
|------|------|
| NotFoundException | 404 |
| BusinessRuleException | 400 |
| ConflictException | 409 |
| FluentValidation | 400 |

---

## 📄 Pagination

\`\`\`
GET /api/Books/paged?pageNumber=2&pageSize=20
\`\`\`

\`\`\`json
{
  "items": [...],
  "pageNumber": 2,
  "pageSize": 20,
  "totalCount": 153,
  "totalPages": 8,
  "hasPreviousPage": true,
  "hasNextPage": true
}
\`\`\`

`pageSize` maksimum **100** ile sınırlandırılmıştır (güvenlik).

---

## 🚧 Durum

- ✅ Tüm temel CRUD (5 entity)
- ✅ Loan sistemi
- ✅ Penalty sistemi
- ✅ Global Exception Handling
- ✅ Pagination
- ⏳ JWT Authentication (opsiyonel)
- ⏳ Logging — Serilog (opsiyonel)
- ⏳ Unit testler (opsiyonel)

---

## 👤 Geliştirici

**[@furkanisikcode](https://github.com/furkanisikcode)**

---

## 📝 Lisans

Bu proje öğrenim amaçlı geliştirilmiştir.
# 📚 Library Management System

> ASP.NET Core 10, Entity Framework Core ve PostgreSQL ile geliştirilmiş, **katmanlı mimari** ve **SOLID prensipleri**ne sıkı sıkıya bağlı, kurumsal düzey bir kütüphane yönetim sistemi REST API'si.

Bu proje sadece çalışan bir uygulama değil; aynı zamanda profesyonel bir backend projesinin nasıl yapılandırılması gerektiğine dair bir referanstır. Her katmanın tek bir sorumluluğu vardır, bağımlılıklar tek yönlüdür ve her detay test edilebilir, genişletilebilir ve sürdürülebilir olacak şekilde tasarlanmıştır.

![Status](https://img.shields.io/badge/status-complete-success)
![.NET](https://img.shields.io/badge/.NET-10-blueviolet)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-18-blue)

---

## 🛠️ Kullanılan Teknolojiler

| Teknoloji | Amaç |
|-----------|------|
| **.NET 10** | LTS sürümü, uzun süreli destek |
| **ASP.NET Core Web API** | REST endpoint'leri |
| **Entity Framework Core 10** | ORM, veritabanı erişimi |
| **PostgreSQL** | İlişkisel veritabanı |
| **Npgsql** | PostgreSQL provider'ı |
| **AutoMapper** | Entity ↔ DTO dönüşümleri |
| **FluentValidation** | Model doğrulama |
| **Swagger / OpenAPI** | API dokümantasyonu |

---

## 🏗️ Mimari

Proje, dört ayrı sınıf kütüphanesinden (Class Library) oluşan bir solution yapısı üzerine kurulmuştur. **Her katmanın tek bir sorumluluğu vardır** ve bağımlılıklar **tek yönlüdür**:

LibraryManagement/
├── LibraryManagement.Entities      → Domain modelleri (saf, bağımlılıksız)
├── LibraryManagement.DataAccess    → DbContext, Repository pattern, Migrations
├── LibraryManagement.Business      → Service'ler, DTO'lar, Validation, Mapping, Settings, Exceptions
└── LibraryManagement.WebAPI        → Controller'lar, REST endpoints, Middleware

**Bağımlılık yönü:**
WebAPI  →  Business  →  DataAccess  →  Entities

`Entities` katmanı **hiçbir şeye bağımlı değildir**. Bu, **Dependency Inversion** prensibinin (SOLID'in D'si) somut bir uygulamasıdır.

---

## ✨ Mimari Özellikler

- ✅ **Generic + Specific Repository Pattern** — tekrar eden CRUD kodu yok, gerektiğinde özel sorgular için extension
- ✅ **DTO ile Veri Taşıma** — her endpoint için özel DTO (List, Detail, Create, Update)
- ✅ **AutoMapper Profile'ları** — Entity ↔ DTO dönüşümleri tek bir yerde
- ✅ **FluentValidation** — okunaklı, kompozisyonel, Türkçe doğrulama kuralları
- ✅ **Configuration Pattern (Options)** — iş kuralları `appsettings.json`'dan okunuyor
- ✅ **Custom Exception Hierarchy** — `NotFoundException`, `BusinessRuleException`, `ConflictException`
- ✅ **Global Exception Middleware** — RFC 7807 Problem Details formatında hata cevapları
- ✅ **Atomik Transaction Yönetimi** — çoklu DB değişiklikleri tek transaction'da
- ✅ **Soft Delete** — kayıtlar fiziksel olarak silinmez (`IsDeleted = true`)
- ✅ **UTC DateTime** — tüm tarih alanları zaman dilimine bağımsız
- ✅ **Dependency Injection** — her katman interface'lere bağımlı
- ✅ **Async/Await** — tüm veritabanı işlemleri asenkron
- ✅ **Pagination** — generic `PagedResult<T>` ile tüm listelerde sayfalama
- ✅ **RESTful API** — doğru HTTP verb'leri ve status code'lar (200, 201, 204, 400, 404, 409)
- ✅ **Swagger UI** — interaktif API dokümantasyonu

---

## 🎯 Domain Modeli

Sistem aşağıdaki varlıkları yönetir:

- **Book** — Kitap (başlık, ISBN, sayfa sayısı, stok adedi, yayın yılı)
- **Author** — Yazar (ad, soyad, biyografi, milliyet)
- **Category** — Kategori/Tür (kitapların sınıflandırılması)
- **Publisher** — Yayınevi (kitap yayıncıları)
- **Member** — Üye (kütüphane üyeleri, rol sistemi ile: Admin/Librarian/Member)
- **Loan** — Ödünç işlemi (hangi üye, hangi kitabı, ne zaman)
- **Penalty** — Ceza kayıtları (gecikme cezaları)

### Varlıklar Arası İlişkiler

- Bir `Publisher`'ın birden fazla `Book`'u olabilir → **one-to-many**
- Bir `Book`'un birden fazla `Author`'u, bir `Author`'un birden fazla `Book`'u olabilir → **many-to-many**
- Bir `Book` birden fazla `Category`'ye ait olabilir → **many-to-many**
- Bir `Member`'ın birden fazla `Loan`'u olabilir → **one-to-many**
- Bir `Loan`'a bağlı birden fazla `Penalty` olabilir → **one-to-many**

Tüm entity'ler, ortak alanları (`Id`, `CreatedDate`, `UpdatedDate`, `IsDeleted`) içeren abstract bir `BaseEntity` sınıfından miras alır.

---

## 💼 İş Kuralları (Tamamen Uygulandı)

`appsettings.json`'da yapılandırılabilir, **service katmanında** uygulanan kurallar:

- ✅ Bir üye aynı anda en fazla **5 kitap** ödünç alabilir
- ✅ Ödünç alma süresi **14 gün**
- ✅ Geciken her gün için **5 TL** sabit ceza uygulanır
- ✅ **Stokta bulunmayan** bir kitap ödünç verilemez
- ✅ **Pasif** (inactive) üye ödünç alamaz
- ✅ **Ödenmemiş cezası** bulunan üye yeni kitap ödünç alamaz
- ✅ Bir email adresi sadece **bir üyede** kayıtlı olabilir (unique kontrolü)
- ✅ Ödünç iade edildiğinde gecikme varsa **otomatik ceza oluşturulur**
- ✅ Ödenmiş bir ceza **tekrar ödenemez** (çift ödeme engeli)
- ✅ Tüm silme işlemleri **soft delete** mantığıyla gerçekleştirilir
- ✅ Stok yönetimi otomatik: ödünç verince azalır, iade alınca artar (atomik)

---

## 🔥 SOLID Prensiplerinin Uygulanması

| Prensip | Uygulama |
|---------|----------|
| **S** — Single Responsibility | Her sınıfın tek bir görevi var: Entity sadece veri tutar, Validator sadece doğrular, Service iş mantığını yönetir, Controller HTTP'yi çevirir, Middleware exception'ları yakalar |
| **O** — Open/Closed | `BaseEntity`, generic `Repository<T>`, `AppException` gibi temel sınıflar genişlemeye açık, değişikliğe kapalı |
| **L** — Liskov Substitution | `BaseEntity`'den türeyen tüm entity'ler onun yerine kullanılabilir; generic Repository bu sayede tüm entity'lerle çalışır |
| **I** — Interface Segregation | Service ve Repository arayüzleri amaca yönelik, küçük ve odaklı |
| **D** — Dependency Inversion | Üst katmanlar somut sınıflara değil, interface'lere bağımlı; bağımlılıklar DI ile çalışma anında enjekte edilir |

---

## 🚀 Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (17+ önerilir)
- [pgAdmin](https://www.pgadmin.org/) (opsiyonel, görsel yönetim için)

### Adımlar

```bash
# Repo'yu klonla
git clone https://github.com/furkanisikcode/library-management-system.git
cd library-management-system

# Paketleri restore et
dotnet restore

# Veritabanı bağlantısını yapılandır
# src/LibraryManagement.WebAPI/appsettings.json dosyasında
# ConnectionStrings.DefaultConnection değerini kendi PostgreSQL şifrenizle güncelleyin

# Veritabanını oluştur
dotnet ef database update --project src/LibraryManagement.DataAccess --startup-project src/LibraryManagement.WebAPI

# Uygulamayı çalıştır
dotnet run --project src/LibraryManagement.WebAPI
```

### Swagger UI

Uygulama çalıştıktan sonra tarayıcıdan:
http://localhost:5245/swagger

---

## ⚙️ Yapılandırma

`src/LibraryManagement.WebAPI/appsettings.json` dosyasında PostgreSQL bağlantı bilgilerini ve iş kurallarını güncelleyin:

```json
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
```

---

## 📡 API Endpoint'leri

Sistemde **7 controller** ve **35+ endpoint** vardır:

### Books, Authors, Categories, Publishers
Standart CRUD + Pagination:
- `GET    /api/{resource}` — Tümünü listele
- `GET    /api/{resource}/paged?pageNumber=1&pageSize=20` — Sayfalı liste
- `GET    /api/{resource}/{id}` — Tek kayıt detayı
- `POST   /api/{resource}` — Yeni kayıt
- `PUT    /api/{resource}/{id}` — Güncelle
- `DELETE /api/{resource}/{id}` — Sil (soft delete)

### Members
Yukarıdaki CRUD + email unique kontrolü

### Loans (özel iş mantığı)
- `GET    /api/Loans` — Tüm ödünç işlemleri
- `GET    /api/Loans/{id}` — Detay
- `POST   /api/Loans/borrow` — **Ödünç ver** (6 iş kuralı kontrol edilir)
- `POST   /api/Loans/return` — **İade et** (gecikme varsa otomatik ceza oluşur)

### Penalties (özel iş mantığı)
- `GET    /api/Penalties` — Tüm cezalar
- `GET    /api/Penalties/{id}` — Detay
- `GET    /api/Penalties/member/{memberId}` — **Üyenin ceza özeti** (toplam borç, ödenmiş, ödenmemiş)
- `POST   /api/Penalties/{id}/pay` — **Cezayı öde**

---

## 🛡️ Hata Yönetimi

Sistem, **RFC 7807 Problem Details** standardına uygun hata cevapları döner:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Bulunamadı",
  "status": 404,
  "detail": "Id=999 olan kitap bulunamadı.",
  "traceId": "0HN07845G51HI:00000003"
}
```

| Durum | Exception | HTTP Status |
|-------|-----------|-------------|
| Kayıt bulunamadı | `NotFoundException` | 404 |
| İş kuralı ihlali (stok yok, limit aşıldı vb.) | `BusinessRuleException` | 400 |
| Çakışma (email zaten var vb.) | `ConflictException` | 409 |
| Beklenmeyen hata | (generic) | 500 |
| DTO doğrulama hatası | (FluentValidation) | 400 |

Tüm exception'lar **`ExceptionHandlingMiddleware`** tarafından yakalanır. Stack trace **frontend'e sızdırılmaz**, sadece log'lara yazılır.

---

## 📄 Pagination

Tüm liste endpoint'lerinde pagination desteği vardır:
GET /api/Books/paged?pageNumber=2&pageSize=20

Cevap formatı:

```json
{
  "items": [...],
  "pageNumber": 2,
  "pageSize": 20,
  "totalCount": 153,
  "totalPages": 8,
  "hasPreviousPage": true,
  "hasNextPage": true
}
```

**Güvenlik:** `pageSize` maksimum **100** ile sınırlandırılmıştır.

---

## 🚧 Geliştirme Durumu

- ✅ Altyapı (Solution, Entities, DbContext, Migrations)
- ✅ Generic + Specific Repository Pattern
- ✅ Book CRUD (DTO, Service, Validator, Mapper, Controller)
- ✅ Author / Category / Publisher CRUD
- ✅ Member CRUD (email unique kontrolü ile)
- ✅ Loan sistemi (ödünç verme, iade, iş kuralları)
- ✅ Penalty sistemi (gecikme cezası hesaplama, ödeme, üye özeti)
- ✅ Configuration Pattern (`appsettings.json`'da iş kuralları)
- ✅ Custom Exception Hierarchy
- ✅ Global Exception Handling Middleware (RFC 7807)
- ✅ Pagination (tüm liste endpoint'lerinde)
- ⏳ Authentication & Authorization (JWT) — opsiyonel
- ⏳ Logging (Serilog) — opsiyonel
- ⏳ Unit ve Integration testler — opsiyonel

---

## 👤 Geliştirici

**[@furkanisikcode](https://github.com/furkanisikcode)**

---

## 📝 Lisans

Bu proje öğrenim amaçlı geliştirilmiştir. Eğitim ve referans olarak özgürce kullanılabilir.
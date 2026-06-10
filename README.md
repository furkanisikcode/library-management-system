#  Library Management System

> ASP.NET Core 10, Entity Framework Core ve PostgreSQL ile geliştirilmiş, **katmanlı mimari** ve **SOLID prensipleri**ne sıkı sıkıya bağlı, kurumsal düzey bir kütüphane yönetim sistemi REST API'si.

Bu proje sadece çalışan bir uygulama değil; aynı zamanda profesyonel bir backend projesinin nasıl yapılandırılması gerektiğine dair bir referanstır. Her katmanın tek bir sorumluluğu vardır, bağımlılıklar tek yönlüdür ve her detay test edilebilir, genişletilebilir ve sürdürülebilir olacak şekilde tasarlanmıştır.

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

##  Mimari

Proje, dört ayrı sınıf kütüphanesinden (Class Library) oluşan bir solution yapısı üzerine kurulmuştur. **Her katmanın tek bir sorumluluğu vardır** ve bağımlılıklar **tek yönlüdür**:

\`\`\`
LibraryManagement/
├── LibraryManagement.Entities      → Domain modelleri (saf, bağımlılıksız)
├── LibraryManagement.DataAccess    → DbContext, Repository pattern, Migrations
├── LibraryManagement.Business      → Service'ler, DTO'lar, Validation, Mapping
└── LibraryManagement.WebAPI        → Controller'lar, REST endpoints
\`\`\`

**Bağımlılık yönü:**

\`\`\`
WebAPI  →  Business  →  DataAccess  →  Entities
\`\`\`

`Entities` katmanı **hiçbir şeye bağımlı değildir**. Bu, **Dependency Inversion** prensibinin (SOLID'in D'si) somut bir uygulamasıdır.

---

##  Mimari Özellikler

-  **Generic + Specific Repository Pattern** — tekrar eden CRUD kodu yok, gerektiğinde özel sorgular için extension
-  **DTO ile Veri Taşıma** — her endpoint için özel DTO (List, Detail, Create, Update)
-  **AutoMapper Profile'ları** — Entity ↔ DTO dönüşümleri tek bir yerde
-  **FluentValidation** — okunaklı, kompozisyonel, Türkçe doğrulama kuralları
-  **Soft Delete** — kayıtlar fiziksel olarak silinmez (`IsDeleted = true`), audit ve raporlama için korunur
-  **UTC DateTime** — tüm tarih alanları zaman dilimine bağımsız
-  **Dependency Injection** — her katman interface'lere bağımlı, somut sınıflara değil
-  **Async/Await** — tüm veritabanı işlemleri asenkron
-  **RESTful API** — doğru HTTP verb'leri ve status code'lar (200, 201, 204, 400, 404)
-  **RFC 9110 Problem Details** — hata mesajları standart formatta
-  **Swagger UI** — interaktif API dokümantasyonu

---

##  Domain Modeli

Sistem aşağıdaki varlıkları yönetir:

- **Book** — Kitap (başlık, ISBN, sayfa sayısı, stok adedi, yayın yılı)
- **Author** — Yazar (ad, soyad, biyografi, milliyet)
- **Category** — Kategori/Tür (kitapların sınıflandırılması)
- **Publisher** — Yayınevi (kitap yayıncıları)
- **Member** — Üye (kütüphane üyeleri, rol sistemi ile)
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

##  SOLID Prensiplerinin Uygulanması

| Prensip | Uygulama |
|---------|----------|
| **S** — Single Responsibility | Her sınıfın tek bir görevi var: Entity sadece veri tutar, Validator sadece doğrular, Service iş mantığını yönetir, Controller HTTP'yi çevirir |
| **O** — Open/Closed | `BaseEntity` ve generic `Repository<T>` gibi temel sınıflar genişlemeye açık, değişikliğe kapalı |
| **L** — Liskov Substitution | `BaseEntity`'den türeyen tüm entity'ler onun yerine kullanılabilir; generic Repository bu sayede tüm entity'lerle çalışır |
| **I** — Interface Segregation | Service ve Repository arayüzleri amaca yönelik, küçük ve odaklı |
| **D** — Dependency Inversion | Üst katmanlar somut sınıflara değil, interface'lere bağımlı; bağımlılıklar DI ile çalışma anında enjekte edilir |

---

##  Kurulum ve Çalıştırma

### Gereksinimler

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (17+ önerilir)
- [pgAdmin](https://www.pgadmin.org/) (opsiyonel, görsel yönetim için)

### Adımlar

\`\`\`bash
# Repo'yu klonla
git clone https://github.com/furkanisikcode/library-management-system.git
cd library-management-system

# Paketleri restore et
dotnet restore

# Veritabanı bağlantısını yapılandır
# src/LibraryManagement.WebAPI/appsettings.json dosyasında
# ConnectionStrings.DefaultConnection değerini güncelle

# Veritabanını oluştur
dotnet ef database update \\
  --project src/LibraryManagement.DataAccess \\
  --startup-project src/LibraryManagement.WebAPI

# Uygulamayı çalıştır
dotnet run --project src/LibraryManagement.WebAPI
\`\`\`

### Swagger UI

Uygulama çalıştıktan sonra tarayıcıdan:

\`\`\`
http://localhost:5245/swagger
\`\`\`

---

## ⚙️ Yapılandırma

`src/LibraryManagement.WebAPI/appsettings.json` dosyasında PostgreSQL bağlantı bilgilerini güncelleyin:

\`\`\`json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=YOUR_PASSWORD"
  }
}
\`\`\`

---

## 📡 API Endpoint'leri

Şu an aktif olan controller'lar:

### Books
- `GET    /api/Books` — Tüm kitapları listele
- `GET    /api/Books/{id}` — Tek kitabın detayı
- `POST   /api/Books` — Yeni kitap ekle
- `PUT    /api/Books/{id}` — Kitap güncelle
- `DELETE /api/Books/{id}` — Kitap sil (soft delete)

### Authors, Categories, Publishers
Yukarıdakiyle aynı CRUD endpoint'leri (toplam **20 endpoint**).

---

##  Geliştirme Durumu

-  Altyapı (Solution, Entities, DbContext, Migrations)
-  Generic + Specific Repository Pattern
-  Book CRUD (DTO, Service, Validator, Mapper, Controller)
-  Publisher CRUD
-  Category CRUD
-  Author CRUD
- ⏳ Member CRUD (rol sistemi ile)
- ⏳ Loan sistemi (ödünç verme, iade, iş kuralları)
- ⏳ Penalty sistemi (gecikme cezası hesaplama)
- ⏳ Global exception handling middleware
- ⏳ Authentication & Authorization (JWT)
- ⏳ Unit ve Integration testler

---

##  İş Kuralları (Planlanan)

- Bir üye aynı anda en fazla **5 kitap** ödünç alabilir
- Ödünç alma süresi **14 gün**'dür
- Geciken her gün için **sabit ceza** uygulanır
- **Stokta bulunmayan** bir kitap ödünç verilemez
- **Ödenmemiş cezası** bulunan üye yeni kitap ödünç alamaz
- Tüm silme işlemleri **soft delete** mantığıyla gerçekleştirilir

---

##  Geliştirici

**[@furkanisikcode](https://github.com/furkanisikcode)**

---

##  Lisans

Bu proje öğrenim amaçlı geliştirilmiştir. Eğitim ve referans olarak özgürce kullanılabilir.

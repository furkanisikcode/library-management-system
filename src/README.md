# 📚 Library Management System

ASP.NET Core 10 ile katmanlı mimari (N-Tier) ve SOLID prensiplerine uygun olarak geliştirilmiş kütüphane yönetim sistemi.

## 🛠️ Teknolojiler

- **.NET 10** (LTS)
- **ASP.NET Core Web API**
- **Entity Framework Core 10**
- **PostgreSQL** (Npgsql provider)
- **AutoMapper** (Entity ↔ DTO dönüşümü)
- **FluentValidation** (Model doğrulama)
- **Swagger / OpenAPI** (API dokümantasyonu)

## 🏗️ Mimari

Proje 4 katmanlı bir solution yapısına sahiptir:

LibraryManagement/
├── LibraryManagement.Entities      → Domain modelleri (saf, bağımlılıksız)
├── LibraryManagement.DataAccess    → DbContext, Repository pattern
├── LibraryManagement.Business      → Service'ler, DTO'lar, Validation, Mapping
└── LibraryManagement.WebAPI        → Controller'lar, REST endpoints

**Bağımlılık yönü:** `WebAPI → Business → DataAccess → Entities`

## ✨ Özellikler

- ✅ Generic + Specific Repository Pattern
- ✅ Service katmanında iş mantığı
- ✅ DTO ile veri taşıma (List, Detail, Create, Update)
- ✅ AutoMapper Profile'larla otomatik mapping
- ✅ FluentValidation ile model doğrulama
- ✅ Soft delete (IsDeleted bayrağı)
- ✅ Tüm DateTime alanları UTC
- ✅ RESTful API tasarımı
- ✅ Swagger UI ile tam dokümantasyon

## 🎯 Domain Modeli

- **Book** - Kitap
- **Author** - Yazar (many-to-many ile Book)
- **Category** - Kategori (many-to-many ile Book)
- **Publisher** - Yayınevi (one-to-many ile Book)
- **Member** - Üye (rol sistemi ile)
- **Loan** - Ödünç işlemi
- **Penalty** - Ceza kayıtları

## 🚀 Çalıştırma

```bash
# Solution kök dizinde
dotnet restore
dotnet build

# Veritabanı migration'ı uygula
dotnet ef database update --project src/LibraryManagement.DataAccess --startup-project src/LibraryManagement.WebAPI

# Çalıştır
dotnet run --project src/LibraryManagement.WebAPI
```

Swagger UI: `http://localhost:5245/swagger`

## ⚙️ Konfigürasyon

`src/LibraryManagement.WebAPI/appsettings.json` dosyasında PostgreSQL bağlantı string'ini güncelleyin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=LibraryDB;Username=postgres;Password=YOUR_PASSWORD"
}
```

## 📝 Lisans

Bu proje öğrenim amaçlı geliştirilmiştir.
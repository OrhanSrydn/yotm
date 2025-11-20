# Yaz Okulu Takip ve Yönetim Sistemi (YOTM)

Üniversiteler için yaz okulu ders başvuru ve kontenjan takip sistemi. Öğrenciler derslere başvurabilir, adminler başvuruları yönetebilir.

## 📋 İçindekiler

- [Özellikler](#özellikler)
- [Teknolojiler](#teknolojiler)
- [Gereksinimler](#gereksinimler)
- [Kurulum](#kurulum)
- [Kullanım](#kullanım)
- [Docker ile Çalıştırma](#docker-ile-çalıştırma)
- [Proje Yapısı](#proje-yapısı)
- [API Dokümantasyonu](#api-dokümantasyonu)

## ✨ Özellikler

### Öğrenci Özellikleri

- 📱 **OTP ile Giriş**: Telefon numarası ile SMS doğrulama
- 📚 **Ders Listesi**: Aktif yaz okulu derslerini görüntüleme
- ✍️ **Başvuru Yapma**: Derslere başvuru gönderme
- 📊 **Başvuru Takibi**: Başvuru durumlarını görüntüleme (Beklemede/Onaylandı/Reddedildi)
- 👤 **Profil Yönetimi**: Kişisel bilgileri güncelleme
- 🔔 **Kontenjan Bilgisi**: Anlık kontenjan durumu takibi

### Admin Özellikleri

- 🔐 **Güvenli Giriş**: Kullanıcı adı ve şifre ile admin paneli
- 📋 **Dashboard**: Tüm dersleri ve kontenjan durumlarını görüntüleme
- ✅ **Başvuru Yönetimi**: Başvuruları onaylama/reddetme
- 📝 **Not Ekleme**: Başvurulara açıklama/not ekleme
- 👥 **Başvuru Detayları**: Öğrenci bilgileri ve başvuru geçmişi

## 🛠 Teknolojiler

### Backend

- **.NET 8** - Framework
- **ASP.NET Core Web API** - RESTful API
- **ASP.NET Core MVC** - Web uygulaması
- **Entity Framework Core** - ORM
- **SQL Server** - Veritabanı
- **JWT Bearer** - Authentication
- **Swagger/OpenAPI** - API dokümantasyonu

### Frontend

- **Razor Pages** - Server-side rendering
- **Bootstrap 5** - UI framework
- **Bootstrap Icons** - İkonlar
- **JavaScript (Vanilla)** - İnteraktif özellikler

### DevOps

- **Docker** - Containerization
- **Docker Compose** - Multi-container orchestration

## 📦 Gereksinimler

### Local Development

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server) (Express veya Developer)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) veya [VS Code](https://code.visualstudio.com/)

### Docker ile Çalıştırma

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

## 🚀 Kurulum

### 1. Projeyi Klonlayın

```bash
git clone <repository-url>
cd yotm
```

### 2. Veritabanı Bağlantısını Yapılandırın

`yotm.API/appsettings.Development.json` dosyasını düzenleyin:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=YotmDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Migration Çalıştırın

```bash
cd yotm.API
dotnet ef database update --project ../yotm.Insfrastructure
```

### 4. Projeyi Çalıştırın

**Terminal 1 - API:**

```bash
cd yotm.API
dotnet run
```

**Terminal 2 - Web:**

```bash
cd yotm.Web
dotnet run
```

### Visual Studio üzerinden çalıştıracaksanız solution properties kısmından multiple startup project kısmından yotm.API ve yotm.Web projelerini start ayarladığınızda aynı anda 2 projeyi çalıştırıp kullanmaya başlayabilirsiniz.

### 5. Uygulamaya Erişin

- **Web Uygulaması**: http://localhost:5000
- **API**: http://localhost:5001
- **Swagger**: http://localhost:5001/swagger

## 🐳 Docker ile Çalıştırma

Detaylı bilgi için [DOCKER-SETUP.md](DOCKER-SETUP.md) dosyasına bakın.

### Hızlı Başlangıç

```bash
# Docker Desktop'ı başlatın ve ardından:
docker-compose up --build

# Arka planda çalıştırmak için:
docker-compose up -d --build
```

### Erişim

- **Web Uygulaması**: http://localhost:5000
- **API**: http://localhost:5001
- **SQL Server**: localhost:1433
  - Kullanıcı: `sa`
  - Şifre: `H4l1c3duTr!2024@Sql`

### Durdurma

```bash
docker-compose down
```

## 📚 API Dokümantasyonu

API çalıştığında Swagger UI'ya erişebilirsiniz: http://localhost:5001/swagger

### Ana Endpoint'ler

#### Authentication

- `POST /api/auth/request-otp` - OTP kodu talep et
- `POST /api/auth/verify-otp` - OTP kodunu doğrula ve token al
- `POST /api/auth/admin-login` - Admin girişi

#### Courses (Dersler)

- `GET /api/courses` - Tüm aktif dersleri listele
- `GET /api/courses/{id}` - Ders detayını getir
- `GET /api/courses/{id}/applications` - Derse ait başvuruları listele

#### Course Applications (Başvurular)

- `POST /api/courseapplications` - Derse başvuru yap (Öğrenci)
- `GET /api/courseapplications/me/applications` - Öğrencinin başvurularını listele
- `PUT /api/courseapplications/{id}/status` - Başvuru durumunu güncelle (Admin)

#### Students (Öğrenciler)

- `GET /api/students/me` - Profil bilgilerini getir
- `PUT /api/students/me` - Profil bilgilerini güncelle

### Authentication

API JWT Bearer token kullanır. Token almak için:

1. OTP kodu talep et: `POST /api/auth/request-otp`
2. OTP kodunu doğrula: `POST /api/auth/verify-otp`
3. Dönen token'ı header'a ekle: `Authorization: Bearer {token}`

## 🔐 Varsayılan Kullanıcılar

### Admin

Migration çalıştırıldığında otomatik oluşturulur:

- **Kullanıcı Adı**: `admin`
- **Şifre**: `Admin123!`

### Öğrenci

Herhangi bir telefon numarası ile OTP girişi yapabilir. İlk girişte otomatik kayıt olur.

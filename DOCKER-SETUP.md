# Docker ile Çalıştırma Kılavuzu

Bu proje Docker ve Docker Compose kullanarak kolayca çalıştırılabilir.

## Gereksinimler

- Docker Desktop (Windows için)
- Docker Compose (genellikle Docker Desktop ile birlikte gelir)

## Yapı

Proje 3 container içerir:
1. **sqlserver** - Microsoft SQL Server 2022
2. **api** - .NET 8 Web API
3. **web** - .NET 8 MVC Web Uygulaması

## Çalıştırma

### 1. Tüm servisleri başlat

Proje kök dizininde:

```bash
docker-compose up --build
```

veya arka planda çalıştırmak için:

```bash
docker-compose up -d --build
```

### 2. Servislere Erişim

- **Web Uygulaması**: http://localhost:5000
- **API**: http://localhost:5001
- **SQL Server**: localhost:1433
  - Kullanıcı: `sa`
  - Şifre: `H4l1c3duTr!2024@Sql`

### 3. Logları İzleme

```bash
docker-compose logs -f
```

Sadece bir servisin loglarını izlemek için:

```bash
docker-compose logs -f api
docker-compose logs -f web
docker-compose logs -f sqlserver
```

### 4. Servisleri Durdurma

```bash
docker-compose down
```

Verileri de silmek için (SQL Server volume):

```bash
docker-compose down -v
```

## Veritabanı Migration

İlk çalıştırmada veritabanı otomatik oluşturulmayabilir. Migration çalıştırmak için:

### Yöntem 1: Container içinde

```bash
# API container'ına bağlan
docker exec -it yotm-api bash

# Migration çalıştır
dotnet ef database update

# Container'dan çık
exit
```

### Yöntem 2: Local (eğer .NET SDK yüklüyse)

Connection string'i docker-compose.yml'deki ile değiştirin:

```bash
dotnet ef database update --project yotm.Insfrastructure --startup-project yotm.API
```

## Önemli Notlar

### Şifreler

**ÖNEMLİ**: `docker-compose.yml` ve `appsettings.Docker.json` içindeki şifreler:
- SQL Server SA şifresi: `H4l1c3duTr!2024@Sql`
- JWT Secret Key: `h4L1c3dUtR!SecR3tK3y@2024#YotmJwtT0k3nS3cUr1tY&`

**Production'da bu şifreleri mutlaka değiştirin ve ortam değişkenleri veya secret management kullanın!**

### Network

Containerlar `yotm-network` adlı bridge network üzerinden haberleşir:
- Web uygulaması, API'yi `http://api:80/api/` adresinden çağırır
- API, SQL Server'a `sqlserver` hostname ile bağlanır

### Volume

SQL Server verileri `sqlserver_data` adlı volume'de saklanır. Bu sayede container yeniden başlatıldığında veriler korunur.

## Sorun Giderme

### SQL Server bağlantı hatası

Eğer API SQL Server'a bağlanamıyorsa:

1. SQL Server container'ının hazır olduğundan emin olun:
   ```bash
   docker-compose ps
   ```

2. Healthcheck durumunu kontrol edin:
   ```bash
   docker inspect yotm-sqlserver
   ```

3. SQL Server loglarını inceleyin:
   ```bash
   docker-compose logs sqlserver
   ```

### Port çakışması

Eğer 5000, 5001 veya 1433 portları kullanılıyorsa, `docker-compose.yml` içindeki port mapping'leri değiştirin:

```yaml
ports:
  - "5002:80"  # 5000 yerine 5002
```

### Build hataları

Cache'i temizleyip yeniden build edin:

```bash
docker-compose build --no-cache
docker-compose up
```

## Sadece Bazı Servisleri Çalıştırma

Örneğin sadece SQL Server:

```bash
docker-compose up sqlserver
```

veya API ve SQL Server (Web olmadan):

```bash
docker-compose up sqlserver api
```

# Halil Mert Develi — Yazılım Sitesi

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Razor_Pages-5C2D91?logo=dotnet)](https://learn.microsoft.com/aspnet/core)
[![Flutter](https://img.shields.io/badge/Flutter-mobil-02569B?logo=flutter)](https://flutter.dev/)
[![Kotlin](https://img.shields.io/badge/Kotlin-Android-7F52FF?logo=kotlin)](https://kotlinlang.org/)

Tek sayfalık, **koyu temalı** kişisel **yazılım vitrini / CV**.

İş başvurusunda veya sana ulaşmak isteyen biri için: *“Bu da benim sitem.”*  
Sadece .NET değil — **Flutter mobil**, **Kotlin**, **Mevora / Mevora 2**, LED ve GitHub’daki gerçek projeler bir arada.

---

## Bu repo nedir?

| | |
| --- | --- |
| **Tür** | Tek sayfa portföy / CV web sitesi |
| **Framework** | ASP.NET Core 8 · Razor Pages · C# |
| **UI** | Özel CSS/JS (animasyonlu hero, particles, typewriter…) |
| **İçerik** | Hakkımda, yığın, projeler, GitHub listesi, iletişim |
| **Canlıya alma** | Domain + VPS veya Windows ASP.NET Core 8 (Azure yok) |

---

## Hızlı çalıştır

### Gereksinim
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- veya [Visual Studio 2022](https://visualstudio.microsoft.com/) + *ASP.NET and web development* workload

### Terminal

```bash
dotnet restore
dotnet run --urls http://0.0.0.0:45217
```

Tarayıcı: [http://127.0.0.1:45217](http://127.0.0.1:45217)

### Visual Studio

1. Bu klasörü klonla / indir  
2. Kökteki **`Portfolio.csproj`** dosyasını aç  
3. **F5** ile çalıştır  

---

## Sitede neler var?

- **Hero** — Flutter · .NET · Kotlin rozetleri, animasyonlu arka plan  
- **Hakkımda** — kim olduğum, yazılım odaklı CV metni  
- **Yığın** — GitHub dil dağılımı (canlı API)  
- **Projeler** — Mevora, Mevora 2, Flutter/Kotlin mobil, ClearPay, LED…  
- **Öne çıkan** — proje hikâyeleri + küçük animasyonlar  
- **İletişim** — e-posta + GitHub  

---

## Klasör yapısı

```text
├── Portfolio.csproj          # Visual Studio giriş noktası
├── Program.cs
├── Pages/
│   └── Index.cshtml          # Tek sayfa içerik
├── wwwroot/
│   ├── css/site.css          # Koyu tema
│   └── js/site.js            # Animasyonlar
├── Dockerfile                # Linux VPS / Docker
├── web.config                # Windows / IIS
├── deploy/
│   ├── nginx.conf.example
│   └── publish.sh
├── HOSTING.md                # Canlıya alma alış listesi
└── README.md                 # Bu dosya
```

---

## Canlıya alma (özet)

1. **Alan adı** al (domain)  
2. **Hosting** al → Linux VPS (önerilen) veya Windows + ASP.NET Core 8  
3. DNS + SSL + publish  

Detaylı adımlar: **[HOSTING.md](./HOSTING.md)**

```bash
docker build -t hmd-portfolio .
docker run --rm -p 8080:8080 hmd-portfolio
```

---

## İletişim

**Halil Mert Develi** · İstanbul  

- Mail: [halilmertdeveliii@gmail.com](mailto:halilmertdeveliii@gmail.com)  
- GitHub: [github.com/HalilMertDeveli](https://github.com/HalilMertDeveli)

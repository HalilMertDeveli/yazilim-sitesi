# Halil Mert Develi — Yazılım Sitesi

Tek sayfalık, koyu temalı **yazılım vitrini / CV**.  
Stack: **ASP.NET Core 8** (Razor Pages) · Flutter · Kotlin · Mevora / Mevora 2.

> İş başvurularında “bu da benim sitem” diyebileceğin adres.  
> Mobil uygulamalar, .NET backend, ürün hatları ve GitHub projeleri bir arada.

## Önizleme (yerel)

```bash
# Gereksinim: .NET 8 SDK
dotnet restore
dotnet run --urls http://0.0.0.0:45217
```

Tarayıcı: http://127.0.0.1:45217

Visual Studio: kökteki `Portfolio.csproj` dosyasını aç → F5.

## Ne var?

| Bölüm | İçerik |
| --- | --- |
| Hero | Flutter · .NET · Kotlin rozetleri, animasyonlar |
| Projeler | Mevora, Mevora 2, Flutter/Kotlin mobil, ClearPay, LED… |
| GitHub | `@HalilMertDeveli` repoları (canlı API + fallback) |
| İletişim | E-posta + GitHub profil |

## Teknoloji

- **ASP.NET Core 8** / Razor Pages / C#
- Özel CSS + JS (particles, typewriter, tilt, scroll reveal)
- Docker + `web.config` + nginx örneği (Azure’suz canlıya alma)

## Canlıya alma

Domain + VPS (veya Windows ASP.NET Core 8 hosting). Adım adım: **[HOSTING.md](./HOSTING.md)**

```bash
docker build -t hmd-portfolio .
docker run --rm -p 8080:8080 hmd-portfolio
```

## Repo yapısı

```
Portfolio.csproj          ← Visual Studio giriş noktası
Program.cs
Pages/Index.cshtml        ← tek sayfa içerik
wwwroot/css|js            ← tema & animasyon
Dockerfile
HOSTING.md
```

## Lisans / iletişim

Halil Mert Develi · İstanbul  
Mail: halilmertdeveliii@gmail.com · GitHub: [HalilMertDeveli](https://github.com/HalilMertDeveli)

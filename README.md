# Halil Mert Develi — Kişisel Portföy

ASP.NET Core 8 (Razor Pages) ile kurulmuş **tek sayfalık**, koyu temalı kişisel tanıtım sitesi. Animasyonlu hero, GitHub projeleri adımı (`@HalilMertDeveli` canlı API + fallback) ve Mevora vitrin bölümü içerir.

## Yerel çalıştırma

Gereksinim: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
dotnet restore
dotnet run --urls http://0.0.0.0:45217
```

Tarayıcı: [http://127.0.0.1:45217](http://127.0.0.1:45217)

## Docker ile canlıya alma

```bash
docker build -t hmd-portfolio .
docker run --rm -p 8080:8080 hmd-portfolio
```

Ardından container’ı Azure App Service, Railway, Fly.io veya herhangi bir container host’a push edebilirsin.

## Cursor Cloud ortamı

Bu repo `.cursor/environment.json` ile Cloud Agent ortamını tanımlar:

- **install:** .NET 8 SDK kurulumu, `restore` + `build`
- **terminals:** port `45217` üzerinde `dotnet run`

Ortam, Cursor dashboard’daki **Environments → e/…** konumunda tutulur. Builds’i etkinleştirmek için ortam sayfasını kullan.

## Yapı

| Yol | Açıklama |
| --- | --- |
| `Pages/Index.cshtml` | Tek sayfa içerik (hero, hakkımda, yetenekler, projeler, iletişim) |
| `wwwroot/css/site.css` | Tipografi ve görsel dil |
| `wwwroot/js/site.js` | Menü + scroll reveal |
| `Dockerfile` | Üretim imajı |

İçerik metinlerini `Pages/Index.cshtml` içinde güncelle.

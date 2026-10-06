# Halil Mert Develi — Yazılım Sitesi

[![.NET](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Razor_Pages-5C2D91?logo=dotnet)](https://learn.microsoft.com/aspnet/core)
[![Flutter](https://img.shields.io/badge/Flutter-mobil-02569B?logo=flutter)](https://flutter.dev/)
[![Kotlin](https://img.shields.io/badge/Kotlin-Android-7F52FF?logo=kotlin)](https://kotlinlang.org/)

Halil Mert Develi'nin kişisel yazılım mühendisi portföyü (TR + EN): **ürünler ve sistemler**, depo listesi değil.
Öne çıkan vaka çalışmaları: **Mevora** (Flutter + Firebase), **LED-COM** (B2B LED ticaret platformu) ve **LED Support Bot** (WhatsApp destek karşılama).

---

## Bu repo nedir?

| | |
| --- | --- |
| **Tür** | Tek sayfa portföy, Türkçe `/` ve İngilizce `/en` |
| **Framework** | ASP.NET Core 8 · Razor Pages · C# (frontend framework yok) |
| **UI** | Özel CSS + küçük vanilla JS (LED-matris hero, scroll reveal, `prefers-reduced-motion` desteği) |
| **İçerik** | Vaka çalışmaları, diğer projeler, hakkımda, yetkinlikler, yolculuk, GitHub, iletişim |
| **GitHub verisi** | Sunucu tarafında 30 dk önbellekli; API düşerse kayıtlı özet gösterilir |
| **Canlıya alma** | **Vercel** · canonical: https://halilmertdeveli.com.tr |

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

## İçeriği güncelleme

Metinlerin ve projelerin tamamı tek dosyada: **`Services/PortfolioContent.cs`**.
Her metin `new("Türkçe", "English")` şeklinde iki dillidir. Yol haritasındaki işler
`Roadmap` listesinde tutulur, böylece biten işlerle karışmaz.

- **Portre:** `wwwroot/img/profile/halil-mert-develi.webp` dosyasını aynı adla değiştirmen yeterli (kare, ~460px).
- **Proje görselleri:** `wwwroot/img/projects/` (WebP).
- **GitHub token (opsiyonel):** Rate limit için Vercel'de `GitHub__Token` ortam değişkeni tanımlanabilir. Sadece sunucuda kullanılır, tarayıcıya gönderilmez.

---

## Klasör yapısı

```text
├── Portfolio.csproj              # Visual Studio giriş noktası
├── Program.cs                    # Pipeline: sıkıştırma, cache, güvenlik başlıkları
├── Models/                       # İçerik ve GitHub view modelleri
├── Services/
│   ├── PortfolioContent.cs       # Tüm portföy içeriği (TR + EN)
│   ├── GitHubService.cs          # Önbellekli GitHub API + fallback
│   └── Icons.cs                  # Inline SVG ikonlar
├── Pages/
│   ├── Index.cshtml(.cs)         # "/" ve "/en"
│   └── Shared/Sections/          # Hero, vaka çalışması, projeler… partial'ları
├── wwwroot/
│   ├── css/site.css
│   ├── js/site.js
│   ├── img/                      # Portre, proje ekranları, OG görseli
│   ├── robots.txt · sitemap.xml · favicon.svg
├── Dockerfile                    # Linux VPS / Docker
├── Dockerfile.vercel             # Vercel container image
├── vercel.json                   # Vercel service + rewrites
├── web.config                    # Windows / IIS
├── deploy/
├── HOSTING.md                    # Canlıya alma (Vercel + domain)
└── README.md
```

---

## Canlıya alma (özet)

- **Canonical domain:** [halilmertdeveli.com.tr](https://halilmertdeveli.com.tr)  
- **Çalışan şimdilik:** [yazilim-sitesi.vercel.app](https://yazilim-sitesi.vercel.app)  
- **Dosyalar:** `Dockerfile.vercel` + `vercel.json`  
- DNS adımları: **[HOSTING.md](./HOSTING.md)**

Detaylı adımlar: **[HOSTING.md](./HOSTING.md)**

```bash
# Yerel Docker (VPS alternatifi)
docker build -t hmd-portfolio .
docker run --rm -p 8080:8080 hmd-portfolio
```

---

## İletişim

**Halil Mert Develi** · İstanbul  

- Mail: [halilmertdeveliii@gmail.com](mailto:halilmertdeveliii@gmail.com)  
- GitHub: [github.com/HalilMertDeveli](https://github.com/HalilMertDeveli)

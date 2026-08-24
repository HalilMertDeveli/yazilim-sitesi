# Halil Mert Develi — Kişisel Portföy / CV

ASP.NET Core 8 (Razor Pages) ile kurulmuş **tek sayfalık**, koyu temalı kişisel tanıtım sitesi. Animasyonlu hero, GitHub **Repositories** adımı (`@HalilMertDeveli` canlı API + bu site kartı), ClearPay / bitirme / LED analizleri ve Mevora’ya açık vitrin.

## Yerel çalıştırma

Gereksinim: [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
dotnet restore
dotnet run --urls http://0.0.0.0:45217
```

Tarayıcı: [http://127.0.0.1:45217](http://127.0.0.1:45217)

## GitHub’a ekleme (Repositories)

```bash
export GH_TOKEN=ghp_xxx   # repo yetkili classic PAT
bash scripts/publish-github.sh
```

Repo adı varsayılan: `HalilMertDeveli/kisisel-portfolyo` → profilde [Repositories](https://github.com/HalilMertDeveli?tab=repositories) altında görünür.

## Canlıya alma (Azure yok)

Alan adı + VPS veya Windows ASP.NET Core hosting al. Detay: [HOSTING.md](./HOSTING.md)

```bash
docker build -t hmd-portfolio .
docker run --rm -p 8080:8080 hmd-portfolio
```

## Cursor Cloud ortamı

`.cursor/environment.json` — .NET 8 install + port `45217` terminal.

## Yapı

| Yol | Açıklama |
| --- | --- |
| `Pages/Index.cshtml` | CV / portföy tek sayfa |
| `wwwroot/css/site.css` | Koyu tema + animasyonlar |
| `wwwroot/js/site.js` | Typewriter, particles, tilt |
| `scripts/publish-github.sh` | GitHub Repositories publish |
| `HOSTING.md` | Domain + hosting alış listesi |
| `Dockerfile` / `web.config` / `deploy/nginx.conf.example` | Canlıya alma |

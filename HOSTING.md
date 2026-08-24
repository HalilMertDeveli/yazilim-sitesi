# Canlıya alma — Azure YOK

Bu site **ASP.NET Core 8**. Sadece HTML hosting olmaz. Domain + (VPS veya Windows ASP.NET Core hosting) gerekir.

Uygulama buna göre ayarlı: `ForwardedHeaders`, `web.config` (IIS), `Dockerfile`, `deploy/nginx.conf.example`.

---

## Sırayla al (tek tek)

### 1) Şimdi al: Alan adı (domain)

Örnek: `halilmertdeveli.com` / `hmd.dev` — ne bulursan.

Nereden: Nic.tr, Turhost, Natro, Namecheap, Cloudflare Registrar…

Alınca bana **sadece domain adını** yaz (örn. `ornek.com`). DNS’e henüz dokunma.

---

### 2) Sonra al: Hosting (birini seç)

**A — Önerilen: Linux VPS**  
- 1 vCPU, **1 GB RAM**, 20 GB disk yeterli  
- Ubuntu 22.04 / 24.04  
- Örnek: Hetzner CX22, Contabo, DigitalOcean Basic, Turhost VPS  

Alınca bana ver: **sunucu IP** + **SSH kullanıcı/şifre veya key** (güvenli şekilde).  
Ben Docker + nginx + SSL kurulumunu yönlendiririm.

**B — Alternatif: Windows hosting**  
- Pakette açıkça **ASP.NET Core 8** / .NET 8 yazmalı  
- Klasik “ASP.NET” (sadece Framework) yetmez  

Alınca bana ver: **FTP/panel bilgisi** + .NET Core desteklediğini doğrulayan ekran görüntüsü/metin.

---

### 3) Birlikte yapacağız (alma, ayar)

| Adım | Ne |
| --- | --- |
| DNS | Domain `A` kaydı → VPS/hosting IP |
| SSL | Let’s Encrypt / panel SSL (HTTPS) |
| Publish | `dotnet publish` veya Docker image |
| `PublicBaseUrl` | `appsettings.Production.json` içine senin domain |

---

## Almaman gerekenler

- Azure App Service / Azure Domain  
- Ayrı veritabanı (bu portföy şu an DB’siz)  
- Pahalı “sınırsız” paketler — 1 GB VPS yeter

---

## Teknik not (benim tarafım hazır)

```bash
# Linux VPS
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 --name portfolio hmd-portfolio

# Windows / IIS
dotnet publish -c Release -o ./publish
```

Domain’i alınca yaz → 2. adıma geçeriz.

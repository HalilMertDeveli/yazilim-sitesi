# Canlıya alma — Azure yok, domain + hosting

Bu uygulama ASP.NET Core 8 Razor Pages. Statik HTML hosting yetmez; ya **Linux VPS + Docker/nginx**, ya da **Windows ASP.NET Core / IIS** destekli hosting gerekir.

## Senin alman gerekenler (sırayla)

1. **Alan adı (domain)**  
   Örn. `halilmertsites.com` — Nic.tr, Namecheap, Cloudflare Registrar, Turhost domain vb.

2. **Hosting (birini seç)**  
   - **Önerilen:** Linux VPS (1 vCPU / 1 GB RAM yeterli başlangıç) — Hetzner, Contabo, DigitalOcean, Turhost VPS  
   - **Alternatif:** Windows hosting + **ASP.NET Core 8** / IIS desteği açıkça yazan paket (klasik “ASP.NET” paylaşımlı paket bazen sadece Framework dönemi olur — Core yazdığından emin ol)

3. **DNS yönetimi**  
   Domain’i hosting/VPS IP’sine `A` kaydı ile bağla (`@` ve isteğe `www`).

4. **SSL (HTTPS)**  
   Let’s Encrypt (Certbot veya Cloudflare proxy) — çoğu VPS panelinde tek tık.

Azure App Service / Azure Domain **gerekmiyor**.

## Bu repoda hazır olanlar

| Dosya | Ne işe yarar |
| --- | --- |
| `Dockerfile` | VPS’te `docker build` + `docker run -p 8080:8080` |
| `deploy/nginx.conf.example` | Domain → uygulama reverse proxy |
| `web.config` | Windows / IIS publish çıktısı |
| `Program.cs` | `ForwardedHeaders` (HTTPS proxy arkası) |

## VPS + Docker (kısa)

```bash
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 --name portfolio hmd-portfolio
# nginx conf örneğini domain’inle kopyala, certbot ile SSL al
```

## Windows hosting

```bash
dotnet publish -c Release -o ./publish
```

`publish` klasörünü panele yükle; `web.config` yanında gelsin. Application Pool → **No Managed Code** + ASP.NET Core Module V2.

## Domain’i alınca bana yaz

Aldığın domain + hosting tipini (VPS mi, Windows mu) söyle; `nginx` / DNS / publish adımlarını senin seçimine göre netleştiririm.

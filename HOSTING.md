# Canlıya alma — Natro

Bu site **ASP.NET Core 8** (Razor Pages). WordPress / sadece HTML hosting yetmez.

Satın almayı **Natro** üzerinden yapacaksan aşağıdaki sırayı izle.

---

## Natro’dan sırayla al

### 1) Şimdi: Alan adı (domain)

Natro → **Alan Adı** / Domain kaydı  
Örnek: `halilmertdeveli.com`

Alınca bana **sadece domain adını** yaz. DNS’e henüz dokunma.

---

### 2) Sonra: Hosting — hangisini al?

#### Önerilen: Natro **XCloud VPS** (Linux)

Paylaşımlı Windows hosting’de Natro tarafında ASP.NET Core / Application Pool kısıtları sık görülüyor. Bu proje için en temiz yol VPS.

| | |
| --- | --- |
| Paket | **XCloud Mini** (başlangıç için yeter) |
| Kaynak | 1 vCPU · **1 GB RAM** · 20 GB SSD |
| İşletim sistemi | **Ubuntu 22.04** veya **24.04** (Linux) |
| Panel | İstersen yok / Lite — Docker kuracağız |

Link: [Natro VPS / XCloud](https://www.natro.com/sunucu-kiralama/vps-cloud-server)

Alınca bana ver:
- sunucu **IP**
- **root / SSH** şifresi veya key  
→ Docker + nginx + SSL kurulumunu yönlendiririm.

#### Alternatif: Natro Windows Hosting

Sadece şunu alırsan dene:
- Paket açıklamasında açıkça **.NET Core / ASP.NET Core / .NET 8** yazmalı  
- Klasik “ASP.NET” / sadece .NET Framework **yetmez**

Satın almadan Natro destek’e sor:  
*“ASP.NET Core 8 Razor Pages yayınlayabilir miyim? Ayrı Application Pool var mı?”*  
Cevap net “evet” değilse **VPS al**.

---

### 3) Almaman gerekenler

- Azure  
- Ayrı SQL / veritabanı (bu site DB’siz)  
- Pahalı “sınırsız” paylaşımlı paket (Core için riskli)

---

### 4) Sen alınca birlikte yapacağız

1. Domain DNS → VPS IP (`A` kaydı)  
2. SSL (Let’s Encrypt / panel)  
3. Siteyi publish / Docker ile koyma  
4. Telefondan `https://senin-domainin.com` testi  

---

## Teknik (hazır)

```bash
# Linux VPS (Natro XCloud)
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 --name portfolio hmd-portfolio

# Windows IIS (Core destekliyse)
dotnet publish -c Release -o ./publish
```

`web.config`, `Dockerfile`, `deploy/nginx.conf.example` repoda hazır.

---

## Özet checklist

- [ ] Natro’dan **domain** al → bana adını yaz  
- [ ] Natro’dan **XCloud Mini (Ubuntu)** al → IP + SSH ver  
- [ ] Birlikte DNS + SSL + yayın  

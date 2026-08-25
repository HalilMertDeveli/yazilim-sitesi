# Canlıya alma — İsimTescil (domain) + Natro (hosting)

Bu site **ASP.NET Core 8** (Razor Pages). WordPress / sadece HTML hosting yetmez.

| | |
| --- | --- |
| **Alan adı** | **`halilmertdeveli.com.tr`** (alındı) |
| **Canlı URL** | `https://halilmertdeveli.com.tr` |
| **Hosting** | Natro XCloud Mini (Linux VPS) — henüz bekleniyor |

---

## 1) Domain — tamam

- Domain: **halilmertdeveli.com.tr**
- Production ayarı: `appsettings.Production.json` → `Site:PublicBaseUrl`
- nginx örneği: `deploy/nginx.conf.example`

**Şimdilik DNS’e dokunma** (A kaydı / NS). VPS IP’si gelince birlikte bağlayacağız.

---

## 2) Sıradaki: Natro VPS (sunucu)

### Önerilen: Natro **XCloud Mini** (Linux VPS)

| | |
| --- | --- |
| Paket | **XCloud Mini** |
| Kaynak | 1 vCPU · **1 GB RAM** · 20 GB SSD |
| OS | **Ubuntu 22.04** veya **24.04** |

Link: [Natro VPS / XCloud](https://www.natro.com/sunucu-kiralama/vps-cloud-server)

Alınca bana yaz: **IP adresi** + **SSH (root şifre veya key)**

> Not: Sadece domain aldın; siteyi yayınlamak için ayrıca bir **sunucu (VPS)** gerekir. Paylaşımlı “sınırsız Windows” paketlerde ASP.NET Core sık sorun çıkarır — bu proje için Linux VPS daha güvenli.

Eğer İsimTescil / Natro’da domain ile birlikte bir hosting paketi de aldıysan, panelde **IP** veya **sunucu bilgisi** var mı bak; varsa onu da yaz.

---

## 3) Birlikte: Domain → sunucu (İsimTescil DNS)

VPS IP’si elimizde olunca İsimTescil’de:

1. Giriş → **Kontrol Paneli** → **Domain Yönetimi**
2. **halilmertdeveli.com.tr** → **Detaylı Yönetim**
3. **DNS Yönetimi**
4. **A kaydı**: `@` → VPS **IP**  
   İsteğe bağlı: `www` → aynı IP (veya CNAME → `@`)
5. Yayılmayı bekle (dakika–saat)
6. SSL (Let’s Encrypt) + Docker ile siteyi koyarız

Rehber: [Domain DNS yönlendirme](https://www.isimtescil.net/bilgibankasi/domain-dns-yonlendirme)

---

## Almaman gerekenler

- Azure  
- Ayrı veritabanı (bu site DB’siz)  
- Domain’i alır almaz rastgele NS değiştirme (IP gelince yapacağız)

---

## Checklist

- [x] Domain: **halilmertdeveli.com.tr**  
- [ ] Natro **XCloud Mini (Ubuntu)** → IP + SSH ver  
- [ ] Birlikte DNS (İsimTescil) + SSL + yayın  

```bash
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 \
  -e Site__PublicBaseUrl=https://halilmertdeveli.com.tr \
  --name portfolio hmd-portfolio
```

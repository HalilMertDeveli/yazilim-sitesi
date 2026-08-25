# Canlıya alma — İsimTescil (domain) + Natro (hosting)

Bu site **ASP.NET Core 8** (Razor Pages). WordPress / sadece HTML hosting yetmez.

Senin planın:
1. **Alan adı** → [İsimTescil](https://www.isimtescil.net/)
2. **Hosting** → Natro (önerilen: XCloud VPS)

---

## 1) Şimdi: İsimTescil’den domain al

1. [isimtescil.net](https://www.isimtescil.net/) → beğendiğin adı ara (örn. `halilmertdeveli.com`)
2. Sepete ekle → satın al / tescil et
3. Alınca bana **sadece domain adını** yaz (örn. `ornek.com`)

**Şimdilik DNS’e dokunma.** Hosting IP’si gelince yönlendireceğiz.

---

## 2) Sonra: Natro’dan hosting al

### Önerilen: Natro **XCloud Mini** (Linux VPS)

| | |
| --- | --- |
| Paket | **XCloud Mini** |
| Kaynak | 1 vCPU · **1 GB RAM** · 20 GB SSD |
| OS | **Ubuntu 22.04** veya **24.04** |

Link: [Natro VPS / XCloud](https://www.natro.com/sunucu-kiralama/vps-cloud-server)

Alınca bana ver: **IP** + **SSH (root şifre veya key)**

> Natro paylaşımlı Windows “sınırsız” paketlerde ASP.NET Core sık sorun çıkarıyor. Bu proje için VPS daha güvenli.

---

## 3) Birlikte: Domain’i sunucuya bağlama (İsimTescil paneli)

Hosting IP’si elimizde olunca İsimTescil’de:

1. Giriş → **Kontrol Paneli** → **Domain Yönetimi**
2. Domain yanında **Detaylı Yönetim**
3. **DNS Yönetimi** → müşteri özel DNS / kayıt ekle
4. **A kaydı**: `@` (ve isteğe `www`) → Natro VPS **IP**
5. Birkaç dakika–birkaç saat yayılır
6. SSL (Let’s Encrypt) + siteyi Docker ile koyarız

(İsimTescil DNS rehberi: [Domain DNS yönlendirme](https://www.isimtescil.net/bilgibankasi/domain-dns-yonlendirme))

---

## Almaman gerekenler

- Azure  
- Ayrı veritabanı (bu site DB’siz)  
- Domain’i alır almaz rastgele NS değiştirme (IP gelince yapacağız)

---

## Checklist

- [ ] İsimTescil’den **domain** → bana adı yaz  
- [ ] Natro **XCloud Mini (Ubuntu)** → IP + SSH ver  
- [ ] Birlikte DNS (İsimTescil) + SSL + yayın  

```bash
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 --name portfolio hmd-portfolio
```

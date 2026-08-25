# Canlıya alma — Vercel (önerilen) + domain

Bu site **ASP.NET Core 8** (Razor Pages). Vercel’de **Docker container** olarak yayınlanır.

| | |
| --- | --- |
| **Alan adı** | **`www.halilmertdeveli.com.tr`** (alındı) |
| **Canlı URL** | `https://www.halilmertdeveli.com.tr` |
| **Hosting** | **Vercel** (container) — kodda hazır |
| **GitHub** | https://github.com/HalilMertDeveli/yazilim-sitesi |

---

## 1) Vercel’e bağla (hızlı yol)

Repo’da zaten var:

- `Dockerfile.vercel` — .NET 8 build + runtime
- `vercel.json` — container service + tüm trafiği uygulamaya yönlendirir
- `Program.cs` — Vercel `$PORT` dinler

### A) Dashboard (en kolay)

1. [vercel.com](https://vercel.com) → GitHub ile giriş (**HalilMertDeveli**)
2. **Add New… → Project** → `yazilim-sitesi` reposunu Import
3. Framework preset: **Other** (otomatik `vercel.json` okunur)
4. **Deploy**

Environment (Project → Settings → Environment Variables):

| Name | Value |
| --- | --- |
| `PORT` | `8080` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Site__PublicBaseUrl` | `https://www.halilmertdeveli.com.tr` |

### B) CLI

```bash
npm i -g vercel
vercel login
vercel link
vercel env add PORT
# 8080
vercel --prod
```

---

## 2) Domain’i Vercel’e bağla

1. Vercel → Project → **Settings → Domains**
2. Önce **`www.halilmertdeveli.com.tr`** ekle (ana adres)
3. İstersen apex **`halilmertdeveli.com.tr`** ekle → Vercel’de **Redirect to www** seç
4. İsimTescil DNS’te Vercel’in verdiği kayıtları uygula (genelde):
   - **CNAME** `www` → `cname.vercel-dns.com`  
   - **A** `@` → `76.76.21.21` (apex için)
5. SSL Vercel’de otomatik gelir

> DNS’i Vercel’e yönlendirmeden önce paneldeki talimatları bire bir uygula; Vercel bazen proje için özel kayıt gösterir.

---

## 3) Alternatif: Natro VPS (isteğe bağlı)

Vercel kullanmazsan Linux VPS + Docker:

```bash
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 \
  -e Site__PublicBaseUrl=https://www.halilmertdeveli.com.tr \
  --name portfolio hmd-portfolio
```

Detaylı VPS yolu: Natro **XCloud Mini (Ubuntu)** → IP + SSH → nginx + Let’s Encrypt.

---

## Checklist

- [x] Domain: **www.halilmertdeveli.com.tr**  
- [x] GitHub public: [yazilim-sitesi](https://github.com/HalilMertDeveli/yazilim-sitesi)  
- [x] Vercel dosyaları: `Dockerfile.vercel` + `vercel.json`  
- [ ] Vercel’de Import + Deploy  
- [ ] Domain → Vercel DNS (`www` ana, apex → www yönlendirme)  
- [ ] (Opsiyonel) Natro VPS  

## Almaman gerekenler

- Azure  
- Ayrı veritabanı (bu site DB’siz)  
- Domain’i alır almaz rastgele NS değiştirme (Vercel kaydı netleşince yap)

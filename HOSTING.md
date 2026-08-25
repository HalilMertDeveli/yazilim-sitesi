# Canlıya alma — Vercel (önerilen) + domain

Bu site **ASP.NET Core 8** (Razor Pages). Vercel’de **Docker container** olarak yayınlanır.

| | |
| --- | --- |
| **Alan adı** | **`halilmertdeveli.com.tr`** (alındı) |
| **Canlı URL** | `https://halilmertdeveli.com.tr` |
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
| `Site__PublicBaseUrl` | `https://halilmertdeveli.com.tr` |

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

1. Vercel → Project → **Settings → Domains** → `halilmertdeveli.com.tr` ekle  
2. İsimTescil DNS’te Vercel’in verdiği kaydı uygula (genelde):
   - **A** `@` → `76.76.21.21`  
   - veya **CNAME** `www` → `cname.vercel-dns.com`  
3. SSL Vercel’de otomatik gelir

> DNS’i Vercel’e yönlendirmeden önce paneldeki talimatları bire bir uygula; Vercel bazen proje için özel kayıt gösterir.

---

## 3) Alternatif: Natro VPS (isteğe bağlı)

Vercel kullanmazsan Linux VPS + Docker:

```bash
docker build -t hmd-portfolio .
docker run -d --restart unless-stopped -p 8080:8080 \
  -e Site__PublicBaseUrl=https://halilmertdeveli.com.tr \
  --name portfolio hmd-portfolio
```

Detaylı VPS yolu: Natro **XCloud Mini (Ubuntu)** → IP + SSH → nginx + Let’s Encrypt.

---

## Checklist

- [x] Domain: **halilmertdeveli.com.tr**  
- [x] GitHub public: [yazilim-sitesi](https://github.com/HalilMertDeveli/yazilim-sitesi)  
- [x] Vercel dosyaları: `Dockerfile.vercel` + `vercel.json`  
- [ ] Vercel’de Import + Deploy  
- [ ] Domain → Vercel DNS  
- [ ] (Opsiyonel) Natro VPS  

## Almaman gerekenler

- Azure  
- Ayrı veritabanı (bu site DB’siz)  
- Domain’i alır almaz rastgele NS değiştirme (Vercel kaydı netleşince yap)

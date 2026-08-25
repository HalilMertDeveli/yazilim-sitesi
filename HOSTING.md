# Canlıya alma — Vercel + domain

Bu site **ASP.NET Core 8** (Razor Pages). Vercel’de **Docker container** olarak yayınlanır.

| | |
| --- | --- |
| **Production domain (Vercel)** | **`www.halilmertdeveli.com.tr`** |
| **Apex** | `halilmertdeveli.com.tr` → **308 redirect → www** |
| **Canlı URL** | `https://www.halilmertdeveli.com.tr` |
| **Hosting** | **Vercel** (container, Hobby, **HMD TEAM**, proje `yazilim-sitesi`) |
| **GitHub** | https://github.com/HalilMertDeveli/yazilim-sitesi |
| **DNS paneli** | İsimTescil → **DnsEnable** |
| **Önizleme (çalışıyor)** | https://yazilim-sitesi.vercel.app |

Kod: `Site:PublicBaseUrl` = `https://www.halilmertdeveli.com.tr`

---

## Invalid Configuration — şimdi ne yapacaksın?

Vercel Domains’te kırmızı **Invalid Configuration** görünmesinin sebebi: DnsEnable hâlâ **A → `93.89.230.125`** (İsimTescil). Vercel’in istediği CNAME yok.

### İsimTescil DnsEnable’da (tam değerler — Vercel kartından)

1. [isimtescil.net](https://www.isimtescil.net) → **halilmertdeveli.com.tr** → **DNS Yönetimi / DnsEnable**
2. **Sil:**
   - **A** `www` → `93.89.230.125`
   - **A** `@` → `93.89.230.125` (apex için aşağıda yenisini yaz)
3. **Ekle / kaydet:**

| Tip | Name / Host | Value |
| --- | --- | --- |
| **CNAME** | `www` | **`d0c3035e77d2cff7.vercel-dns-017.com`** |
| **A** | `@` | Apex satırında Vercel’in gösterdiği IP *(Refresh’e basıp apex kartındaki A değerini kopyala; yoksa geçici `76.76.21.21`)* |

> `www` için **A kaydı bırakma** — sadece CNAME olsun.  
> CNAME değerinin sonundaki nokta panellerde genelde yazılmaz.

4. Kaydet → Vercel Domains’te **Refresh** → **Valid Configuration** (5–30 dk) + SSL otomatik.

Kontrol:

```bash
dig @8.8.8.8 CNAME +short www.halilmertdeveli.com.tr
# beklenen: d0c3035e77d2cff7.vercel-dns-017.com.
```

`yazilim-sitesi.vercel.app` zaten **Valid** — site orada çalışıyor; domain DNS düzelince `www` de açılır.

---

## Vercel env (Production)

| Name | Value |
| --- | --- |
| `PORT` | `8080` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Site__PublicBaseUrl` | `https://www.halilmertdeveli.com.tr` |

---

## Checklist

- [x] Vercel deploy (`yazilim-sitesi.vercel.app` Valid)  
- [x] Domains’te `www` = Production, apex → www redirect  
- [ ] DnsEnable: `www` **CNAME** → `d0c3035e77d2cff7.vercel-dns-017.com`  
- [ ] Eski **A `93.89.230.125`** kayıtlarını sil  
- [ ] Vercel’de **Refresh** → Valid + SSL  

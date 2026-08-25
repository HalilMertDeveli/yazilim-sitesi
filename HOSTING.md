# Canlıya alma — Vercel + domain

Bu site **ASP.NET Core 8** (Razor Pages). Vercel’de **Docker container** olarak yayınlanır.

| | |
| --- | --- |
| **Alan adı** | **`halilmertdeveli.com.tr`** |
| **Canlı URL** | `https://halilmertdeveli.com.tr` |
| **Hosting** | **Vercel** (container) |
| **GitHub** | https://github.com/HalilMertDeveli/yazilim-sitesi |
| **DNS paneli** | İsimTescil → **DnsEnable** (`tr.dnsenable.com` / `eu.dnsenable.com`) |

Kodda hazır: `Dockerfile.vercel`, `vercel.json`, `Site:PublicBaseUrl` = `https://halilmertdeveli.com.tr`.

---

## Şu anki durum (production deploy sonrası)

Production deploy tamamsa kalan iş **sadece domain bağlamak**:

1. Vercel’e domain ekle  
2. İsimTescil DnsEnable’da A kaydını Vercel’e çevir  

Şu an DNS apex **`93.89.230.125`** (İsimTescil) → **502**. Site, Vercel A kaydına geçene kadar açılmaz.

**Nameserver değiştirme.** NS zaten DnsEnable; sadece A/CNAME kayıtlarını güncelle.

---

## A) Vercel — domain ekle (Production)

1. [vercel.com](https://vercel.com) → GitHub ile giriş (**HalilMertDeveli**)
2. Projeni aç (`yazilim-sitesi` veya deploy ettiğin proje)
3. **Settings → Domains**
4. **Add** → `halilmertdeveli.com.tr` yaz → ekle  
   - İstenirse `www.halilmertdeveli.com.tr` de ekle → **Redirect to** `halilmertdeveli.com.tr`
5. Domain kartında Vercel’in verdiği DNS değerlerini kopyala (kaynak doğru burasıdır)

Env (Settings → Environment Variables → Production):

| Name | Value |
| --- | --- |
| `PORT` | `8080` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Site__PublicBaseUrl` | `https://halilmertdeveli.com.tr` |

---

## B) İsimTescil / DnsEnable — DNS değiştir

Panel: [isimtescil.net](https://www.isimtescil.net) → domain → **DNS Yönetimi** (DnsEnable).

### 1) Apex (`halilmertdeveli.com.tr`) — zorunlu

Mevcut **A** kaydı:

| Tip | Host | Şu anki değer | Yapılacak |
| --- | --- | --- | --- |
| **A** | `@` (veya boş / `halilmertdeveli.com.tr`) | `93.89.230.125` | **Sil veya düzenle** |

Yeni kayıt (Vercel Domain kartındaki IP’yi kullan; çoğu projede):

| Tip | Host | Değer | TTL |
| --- | --- | --- | --- |
| **A** | `@` | **`76.76.21.21`** *(veya karttaki IP)* | 300 / otomatik |

> Yeni Vercel projelerinde IP farklı olabilir (ör. `216.198.79.x`). **Her zaman Domain kartındaki A değerini yaz.**

### 2) `www` — önerilir

| Tip | Host | Değer |
| --- | --- | --- |
| **CNAME** | `www` | Karttaki hedef (ör. `cname.vercel-dns.com` veya `xxxx.vercel-dns-0xx.com`) |

`www` için ayrı bir **A → 93.89.230.125** varsa onu sil; CNAME ile çakışmasın.

### 3) Dokunma

- **NS** kayıtları (`tr.dnsenable.com`, `eu.dnsenable.com`) — olduğu gibi kalsın  
- E-posta **MX** / **TXT** varsa silme  

Kaydet → 5–30 dk (bazen birkaç saat) bekle. Vercel’de durum **Valid Configuration** olunca SSL otomatik gelir.

Kontrol:

```bash
dig +short A halilmertdeveli.com.tr
# beklenen: 76.76.21.21 (veya karttaki IP — artık 93.89.230.125 olmamalı)
```

Tarayıcı: `https://halilmertdeveli.com.tr`

---

## İlk kez proje yoksa (Import)

1. Vercel → **Add New… → Project** → `yazilim-sitesi` Import  
2. Framework: **Other** (`vercel.json` okunur)  
3. Env’leri yukarıdaki gibi ekle → **Deploy**  
4. Deploy yeşil olunca **A + B** adımlarına geç  

CLI (opsiyonel, `vercel login` gerekir):

```bash
npm i -g vercel
vercel login
vercel link
vercel --prod
vercel domains add halilmertdeveli.com.tr
```

---

## Checklist

- [x] Domain: **halilmertdeveli.com.tr**  
- [x] GitHub: [yazilim-sitesi](https://github.com/HalilMertDeveli/yazilim-sitesi)  
- [x] Kod: `Dockerfile.vercel` + `vercel.json` + `PublicBaseUrl`  
- [ ] Vercel Production deploy yeşil  
- [ ] Settings → Domains → `halilmertdeveli.com.tr`  
- [ ] DnsEnable: A `@` → Vercel IP (`93.89.230.125` kalkmalı)  
- [ ] Vercel’de Valid + `https://halilmertdeveli.com.tr` açılıyor  

## Almaman gerekenler

- Azure / ayrı veritabanı  
- NS’leri rastgele Vercel’e çevirmek (DnsEnable A/CNAME yeterli)  
- Eski `93.89.230.125` A kaydını bırakmak (502 sebebi)

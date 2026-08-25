# Canlıya alma — Vercel + domain

Bu site **ASP.NET Core 8** (Razor Pages). Vercel’de **Docker container** olarak yayınlanır.

| | |
| --- | --- |
| **Alan adı** | **`halilmertdeveli.com.tr`** (birincil) |
| **www** | `www.halilmertdeveli.com.tr` → apex’e yönlendir (opsiyonel ama önerilir) |
| **Canlı URL** | `https://halilmertdeveli.com.tr` |
| **Hosting** | **Vercel** (container, Hobby, **HMD TEAM**, proje `yazilim-sitesi`) |
| **GitHub** | https://github.com/HalilMertDeveli/yazilim-sitesi |
| **DNS paneli** | İsimTescil → **DnsEnable** (`tr.dnsenable.com` / `eu.dnsenable.com`) |
| **Önizleme (çalışıyor)** | https://yazilim-sitesi.vercel.app |

Kodda hazır: `Dockerfile.vercel`, `vercel.json`, `Site:PublicBaseUrl` = `https://halilmertdeveli.com.tr`.

---

## Teşhis (2026-08-25) — kök neden DNS

| Kontrol | Sonuç |
| --- | --- |
| `dig @8.8.8.8 A halilmertdeveli.com.tr` | **`93.89.230.125`** (İsimTescil park/hosting IP) |
| `dig @8.8.8.8 A www.halilmertdeveli.com.tr` | **`93.89.230.125`** (A; CNAME yok) |
| NS (whois) | `tr.dnsenable.com` / `eu.dnsenable.com` — **doğru, değiştirmeyin** |
| `http://halilmertdeveli.com.tr` | **502 Bad Gateway** (Pingora) |
| `https://halilmertdeveli.com.tr` | **bağlantı zaman aşımı** |
| `https://yazilim-sitesi.vercel.app` | **200 OK** — container deploy sağlıklı |

**Sonuç:** Site kodunda / Vercel deploy’da değil; trafik hâlâ İsimTescil IP’sine gidiyor. Vercel trafiği **almıyor**.  
`93.89.230.125` kalkıp Vercel A/CNAME gelene kadar canlı domain **açılmaz**.

---

## Şimdi yapılacaklar (sıra)

1. **Vercel** → domain ekle (Settings → Domains)  
2. **İsimTescil DnsEnable** → eski A kayıtlarını sil / Vercel değerlerini yaz  
3. 5–30 dk bekle → Vercel **Valid Configuration** + otomatik SSL  

Nameserver’ları Vercel’e çevirmeye **gerek yok**.

---

## A) Vercel — Domains (tıklama adımları)

1. [vercel.com](https://vercel.com) → GitHub ile giriş (**HalilMertDeveli**)
2. Team: **HMD TEAM** → proje: **`yazilim-sitesi`**
3. Üst menü / sol: **Settings** → **Domains**
4. **Add** kutusuna yaz: `halilmertdeveli.com.tr` → **Add**
5. İstenirse **Add** ile `www.halilmertdeveli.com.tr` ekle  
   - **Redirect to** / primary: **`halilmertdeveli.com.tr`** (apex birincil olsun — `PublicBaseUrl` ile aynı)
6. Domain kartında Vercel’in gösterdiği kayıtları not et (kaynak doğruluk burasıdır):
   - Apex için **A** değeri (çoğu projede `76.76.21.21`; yenilerde `216.198.79.x` olabilir)
   - `www` için **CNAME** hedefi (`cname.vercel-dns.com` veya `xxxx.vercel-dns-0xx.com`)

Production env (Settings → Environment Variables → **Production**):

| Name | Value |
| --- | --- |
| `PORT` | `8080` |
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `Site__PublicBaseUrl` | `https://halilmertdeveli.com.tr` |

---

## B) İsimTescil / DnsEnable — DNS (tıklama + kayıtlar)

### Panele gir

1. [isimtescil.net](https://www.isimtescil.net) → giriş  
2. Domain listesinden **`halilmertdeveli.com.tr`**  
3. **DNS Yönetimi** / **DnsEnable DNS** (zone kayıtları)

### 1) Sil / değiştir — eski İsimTescil kayıtları

Şu an her ikisi de **A → `93.89.230.125`**. Bunlar 502’nin sebebi.

| Tip | Host | Şu anki değer | Yapılacak |
| --- | --- | --- | --- |
| **A** | `@` (veya boş / `halilmertdeveli.com.tr`) | `93.89.230.125` | **Sil** veya düzenle → Vercel IP |
| **A** | `www` | `93.89.230.125` | **Sil** (yerine CNAME gelecek) |

`www` için hem A hem CNAME bırakma — çakışır.

### 2) Yaz — Vercel kayıtları

| Tip | Host / İsim | Değer | TTL |
| --- | --- | --- | --- |
| **A** | `@` | **`76.76.21.21`** *(veya Domain kartındaki A IP)* | 300 veya otomatik |
| **CNAME** | `www` | **`cname.vercel-dns.com`** *(veya karttaki CNAME hedefi)* | 300 veya otomatik |

> **Karttaki değer farklıysa kartı kullan.** Genel Vercel dokümantasyonu: apex **A → `76.76.21.21`**, subdomain **CNAME → `cname.vercel-dns.com`** (proje bazlı hedef değişebilir).

### 3) Dokunma

- **NS:** `tr.dnsenable.com`, `eu.dnsenable.com` — kalsın  
- Var olan **MX** / e-posta **TXT** — silme  

**Kaydet.**

### 4) Yayılım + SSL

- Genelde **5–30 dakika**, bazen birkaç saat  
- Vercel Domains’te durum **Valid Configuration** olunca SSL otomatik  
- Tarayıcı: `https://halilmertdeveli.com.tr`

Kontrol:

```bash
dig @8.8.8.8 +short A halilmertdeveli.com.tr
# beklenen: 76.76.21.21 (veya karttaki IP — ASLA 93.89.230.125 olmamalı)

dig @8.8.8.8 +short CNAME www.halilmertdeveli.com.tr
# beklenen: cname.vercel-dns.com. (veya karttaki hedef)
```

---

## İlk kez proje yoksa (Import)

1. Vercel → **Add New… → Project** → `yazilim-sitesi` Import  
2. Framework: **Other** (`vercel.json` okunur)  
3. Env’leri yukarıdaki gibi ekle → **Deploy**  
4. Deploy yeşil olunca **A + B**  

CLI (bu ortamda genelde `VERCEL_TOKEN` yok; kendi makinenizde):

```bash
npm i -g vercel
vercel login
vercel link   # HMD TEAM / yazilim-sitesi
vercel --prod
vercel domains add halilmertdeveli.com.tr
vercel domains add www.halilmertdeveli.com.tr
```

---

## Checklist

- [x] Domain: **halilmertdeveli.com.tr**  
- [x] GitHub: [yazilim-sitesi](https://github.com/HalilMertDeveli/yazilim-sitesi)  
- [x] Kod: `Dockerfile.vercel` + `vercel.json` + `PublicBaseUrl`  
- [x] `yazilim-sitesi.vercel.app` → 200 (deploy OK)  
- [ ] Vercel Settings → Domains → apex (+ isteğe bağlı www → apex redirect)  
- [ ] DnsEnable: A `@` → Vercel IP; `www` A(`93.89…`) sil → CNAME  
- [ ] Vercel **Valid** + `https://halilmertdeveli.com.tr` açılıyor  

## Almaman gerekenler

- Azure / ayrı veritabanı  
- NS’leri rastgele Vercel’e çevirmek (DnsEnable A/CNAME yeterli)  
- Eski **`93.89.230.125`** A kayıtlarını bırakmak (502 / HTTPS timeout sebebi)  
- ASP.NET kodunda “domain bug” aramak — sorun DNS yönlendirmesi  

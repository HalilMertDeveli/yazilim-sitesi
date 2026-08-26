# Domain geçişi — Vercel + İsimTescil

**Canonical production URL:** `https://halilmertdeveli.com.tr`  
**Çalışan deployment (bozma):** https://yazilim-sitesi.vercel.app  
**Vercel project:** HMD TEAM → `yazilim-sitesi`  
**Yöntem:** A/CNAME (nameserver değiştirme — Method B yok)

---

## Mevcut durum (2026-08-26 06:54–06:59 UTC — dış işlemler sonrası)

Kullanıcı İsimTescil + Vercel Domains dış işlemlerini “tamamlandı” işaretledi; `VERCEL_TOKEN` **atlanmış**.

| Kontrol | Sonuç |
| --- | --- |
| `yazilim-sitesi.vercel.app` | **HTTP/2 200** — production ayakta (bozulmadı) |
| WHOIS Domain Servers | `tr.dnsenable.com` / `us.dnsenable.com` / `eu.dnsenable.com` |
| Public DNS (Google + Cloudflare, 90 sn sonra tekrar) | Delegation hâlâ **Vercel NS** (`ns1/ns2.vercel-dns.com` → `198.51.44.13` / `198.51.45.13`) → **REFUSED** → **SERVFAIL** |
| `https://halilmertdeveli.com.tr` / `www` / HTTP | **Bağlantı yok** (`curl` boş / fail) |
| DnsEnable NS’e doğrudan dig (bu VM) | Timeout (egress); public DoH yine Vercel REFUSED |
| Vercel MCP `list_projects` | Hâlâ boş — domain status API yok |
| Kod / canonical | `https://halilmertdeveli.com.tr` (uygulama tarafı hazır) |

**Kök neden (katman: DNS nameserver delegation — A/CNAME değil):**  
WHOIS DnsEnable diyor; internetteki resolver’lar ise hâlâ Vercel NS’e gidiyor ve zone **REFUSED**. Bu durumda DnsEnable’daki A/CNAME kayıtları hiç sorulmaz → SSL / Valid / site açılmaz.

**Sonraki blokaj:** İsimTescil NameServer ekranı + DNS satırları + Vercel Domains status’unun kanıtı (ekran görüntüsü / metin). NS gerçekten DnsEnable’a yayılmadan ADIM 5+ devam edemez.

---

## SENİN YAPMAN GEREKENLER (İsimTescil + Vercel)

### Adım 0 — Vercel’den GERÇEK değerleri kopyala

1. Aç: https://vercel.com/hmd-team/yazilim-sitesi/settings/domains  
2. `halilmertdeveli.com.tr` satırında **Edit**:
   - **Production** yap (primary)
   - Redirect **kapalı** (www’ye yönlendirme olmasın)
3. `www.halilmertdeveli.com.tr` satırında **Edit**:
   - **Redirect to** → `halilmertdeveli.com.tr`
4. Her iki satırda **View DNS configuration** aç  
5. Gösterilen **Type / Name / Value** tablosunu olduğu gibi kullan (uydurma yok)

Daha önce Vercel kartında görülen (senin ekran görüntünden, proje-spesifik) `www` değeri:

| Type | Name | Value |
| --- | --- | --- |
| CNAME | `www` | `d0c3035e77d2cff7.vercel-dns-017.com` |

Apex **A** değeri için **View DNS configuration** satırındaki IP’yi kullan.  
(Vercel notu: legacy `76.76.21.21` hâlâ çalışır; kart farklı IP gösteriyorsa **kartı** kullan.)

### Adım 1 — İsimTescil NameServer (DnsEnable kalsın)

NameServer ekranında şunlar olmalı (Vercel NS **olmamalı**):

- `tr.dnsenable.com`
- `eu.dnsenable.com`
- (isteğe `us.dnsenable.com`)

Vercel `ns1`/`ns2` varsa **DnsEnable’a geri al** → Güncelle → 1–24 saat yayılım bekle.

### Adım 2 — İsimTescil DNS kayıtları (A / CNAME)

**DNS Yönetimi / DnsEnable zone** ekranında (nameserver ekranı değil):

| Host | Type | Current (bilinen) | Required | Action |
| --- | --- | --- | --- | --- |
| `@` | A | eski park IP / yok / bozuk | **Vercel Apex kartındaki A IP** | DEĞİŞTİR / EKLE |
| `www` | A | varsa `93.89…` | — | **SİL** (CNAME ile çakışmasın) |
| `www` | CNAME | yok / yanlış | **`d0c3035e77d2cff7.vercel-dns-017.com`** *(veya karttaki CNAME)* | EKLE / DEĞİŞTİR |
| `@` | MX | varsa | aynı | **DOKUNMA** |
| `@` | TXT (SPF/DKIM/DMARC) | varsa | aynı | **DOKUNMA** |
| NS | — | DnsEnable | DnsEnable | **DOKUNMA** (Vercel NS’e çevirme) |

Kaydet → Vercel Domains → **Refresh**.

### Adım 3 — Doğrulama

```bash
dig @8.8.8.8 NS halilmertdeveli.com.tr +short
dig @8.8.8.8 A halilmertdeveli.com.tr +short
dig @8.8.8.8 CNAME www.halilmertdeveli.com.tr +short
```

Beklenen:

- NS → `*.dnsenable.com` (Vercel NS değil)
- A `@` → Vercel IP (SERVFAIL / REFUSED olmamalı)
- CNAME `www` → `d0c3035e77d2cff7.vercel-dns-017.com.`
- Vercel Domains → **Valid Configuration** + SSL
- `https://halilmertdeveli.com.tr` → site
- `https://www.halilmertdeveli.com.tr` → apex’e 308
- `https://yazilim-sitesi.vercel.app` → hâlâ çalışır

---

## Kod / env (bu repoda)

| Öğe | Durum |
| --- | --- |
| `Site:PublicBaseUrl` | `https://halilmertdeveli.com.tr` |
| Canonical / og:url | PublicBaseUrl |
| ForwardedHeaders | Ayarlı |
| AllowedHosts | `*` |
| OAuth | Yok |
| Env (Vercel Production) önerisi | `PORT=8080`, `ASPNETCORE_ENVIRONMENT=Production`, `Site__PublicBaseUrl=https://halilmertdeveli.com.tr` |

`yazilim-sitesi.vercel.app` kodda hardcoded production URL değil; dokümantasyon önizleme linki olarak kalabilir.

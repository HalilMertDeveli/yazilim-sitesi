# Domain geçişi — Vercel + İsimTescil

**Canonical / ana domain (tek):** `https://halilmertdeveli.com.tr`  
**Çalışan deployment (bozma):** https://yazilim-sitesi.vercel.app  
**www:** yalnızca apex’e redirect (ana domain değil)  
**Nameserver:** DnsEnable kalsın (Vercel NS’e çevirme — Method A: A/CNAME)

---

## Mevcut durum (2026-08-26 07:40 UTC — canlı ölçüm)

| Kontrol | Sonuç |
| --- | --- |
| `yazilim-sitesi.vercel.app` | **HTTP/2 200**, Server: Vercel |
| Canlı HTML `canonical` / `og:url` | `https://halilmertdeveli.com.tr/` |
| WHOIS / public NS | `tr.dnsenable.com` / `us.dnsenable.com` / `eu.dnsenable.com` |
| Apex `A` (Google/CF) | **`93.89.226.17`** — AS51557 **İsimTescil** (park/hosting) |
| `www` | CNAME → `halilmertdeveli.com.tr` → aynı park IP |
| HTTP `@` (Host header → park IP) | **200** İsimTescil park sayfası (IIS) — **Vercel değil** |
| HTTPS `@` → park IP | TLS handshake **fail** |
| Vercel CLI / MCP proje erişimi | **Yok** (token yok; `list_projects` boş) |

**Kök neden (katman: DNS A/CNAME hedefi):** Nameserver artık sağlıklı. Apex hâlâ İsimTescil park IP’sine bakıyor; Vercel’e işaret etmiyor. Bu yüzden domain Vercel production’a bağlanmıyor.

**Nameserver değiştirme:** Gerekmiyor ve istenmiyor. Sadece DnsEnable zone içinde A/CNAME düzelt.

---

## Vercel’de yapılacaklar (agent API’siz — sen veya `VERCEL_TOKEN`)

1. Aç: https://vercel.com/hmd-team/yazilim-sitesi/settings/domains  
2. `halilmertdeveli.com.tr` ekli değilse ekle → **Production**, redirect **kapalı**  
3. `www.halilmertdeveli.com.tr` → **Redirect to** `halilmertdeveli.com.tr`  
4. Her satırda **View DNS Records** → Type / Name / Value’yu kopyala (**uydurma IP yok**)  
5. `yazilim-sitesi.vercel.app` **silme**

CLI login (opsiyonel): agent `vercel login` bekliyorsa https://vercel.com/oauth/device kodunu onayla.

---

## İsimTescil’de yapılacaklar (A/CNAME — NS’e dokunma)

### Dokunma

- NameServer (DnsEnable kalsın)
- MX / SPF / DKIM / DMARC / diğer TXT (varsa)

### Değiştir (değerler = Vercel kartından)

| Host | Type | Current (ölçülen) | Required | Action |
| --- | --- | --- | --- | --- |
| `@` | A | `93.89.226.17` (İsimTescil) | **Vercel apex kartındaki A IP** | DEĞİŞTİR |
| `www` | CNAME | `halilmertdeveli.com.tr` | **Vercel www kartındaki CNAME** | DEĞİŞTİR |
| `www` | A (varsa) | — | — | SİL (CNAME ile çakışmasın) |

Kaydet → Vercel Domains → Refresh → Valid + SSL bekle.

---

## Kod / env (repo)

| Öğe | Durum |
| --- | --- |
| `Site:PublicBaseUrl` | `https://halilmertdeveli.com.tr` |
| Canonical / og:url | PublicBaseUrl |
| ForwardedHeaders | Ayarlı |
| AllowedHosts | `*` |
| OAuth / CORS callbacks | Yok |
| `yazilim-sitesi.vercel.app` kodda | Sadece README/HOSTING |

Production env önerisi (Vercel UI): `PORT=8080`, `ASPNETCORE_ENVIRONMENT=Production`, `Site__PublicBaseUrl=https://halilmertdeveli.com.tr`

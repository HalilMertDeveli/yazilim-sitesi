# Domain geçişi — Vercel + İsimTescil

**Canonical / ana domain:** `https://halilmertdeveli.com.tr`  
**Çalışan deployment (bozma):** https://yazilim-sitesi.vercel.app  
**www:** yalnızca apex’e 308 redirect  
**Yöntem:** A/CNAME (DnsEnable NS — nameserver değiştirme)

---

## Durum (2026-08-26 07:55–07:57 UTC)

### Vercel (CLI login sonrası — yapıldı)

| Öğe | Değer |
| --- | --- |
| Team | HMD TEAM (`hmd-team`) |
| Project | `yazilim-sitesi` (`prj_omnbkugC5pUBhD1RyKxCjN9Uhw8N`) |
| Production URL | https://yazilim-sitesi.vercel.app (**200 OK**) |
| Domains | `halilmertdeveli.com.tr` (primary), `www` → apex **308**, `yazilim-sitesi.vercel.app` |
| Ownership verified | `true` (her ikisi) |
| DNS misconfigured | `true` (hâlâ park IP) |
| Production env API | boş (Dockerfile’da `Site__PublicBaseUrl` var) |

### Public DNS (henüz düzelmedi)

| Kayıt | Current | Vercel required |
| --- | --- | --- |
| NS | DnsEnable ✓ | DnsEnable kalsın |
| `@` A | `93.89.226.17` (İsimTescil park) | **rank1:** `216.198.79.1` + `64.29.17.1` *(API)*; alt: `76.76.21.21` *(CLI)* |
| `www` | CNAME → apex | CNAME → **`d0c3035e77d2cff7.vercel-dns-017.com`** |

HTTP `@` → İsimTescil park sayfası. HTTPS park IP’de TLS fail.

---

## İsimTescil — yapman gerekenler

NameServer’a **dokunma**. DNS Zone’da:

1. `@` A `93.89.226.17` sil / değiştir → Vercel A (yukarıdaki rank 1 tercih)
2. `www` CNAME’i `d0c3035e77d2cff7.vercel-dns-017.com` yap
3. `www` A varsa sil
4. MX / SPF / DKIM / DMARC’a dokunma
5. Kaydet → **「DNS kayıtlarını girdim」**

---

## Kod

`Site:PublicBaseUrl` = `https://halilmertdeveli.com.tr` — ek kod değişikliği gerekmedi.

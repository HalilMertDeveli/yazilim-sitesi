# Canlıya alma — Vercel + DNS (acil düzeltme)

## Neden `https://halilmertdeveli.com.tr` açılmıyor?

| | |
| --- | --- |
| DNS şu an | **A → `93.89.230.125`** (İsimTescil) |
| Sonuç | **502** / site yok |
| Vercel site | **çalışıyor:** https://yazilim-sitesi.vercel.app |
| Kod | Hazır — sorun sadece DNS |

Tarayıcı İsimTescil IP’sine gidiyor; Vercel’e hiç ulaşmıyor. Bunu **ben panelinden değiştiremem** — sen İsimTescil DnsEnable’da kayıtları değiştirince düzelir.

---

## 1) İsimTescil’de DNS (zorunlu — şimdi)

1. https://www.isimtescil.net → giriş  
2. **halilmertdeveli.com.tr** → **DNS Yönetimi** / **DnsEnable**  
3. Şunları **sil**:

| Tip | Host | Değer |
| --- | --- | --- |
| A | `@` | `93.89.230.125` |
| A | `www` | `93.89.230.125` |

4. Şunları **ekle** (Vercel Domains kartındaki değerler):

| Tip | Host | Değer |
| --- | --- | --- |
| **A** | `@` | **`76.76.21.21`** *(apex satırında Refresh → farklı IP yazıyorsa onu yaz)* |
| **CNAME** | `www` | **`d0c3035e77d2cff7.vercel-dns-017.com`** |

5. **Kaydet**  
6. Vercel → Domains → her domainde **Refresh**  
7. 5–30 dk bekle → **Valid Configuration** + yeşil kilit (HTTPS)

NS’lere (`tr.dnsenable.com` / `eu.dnsenable.com`) dokunma.

---

## 2) Vercel Domains (senin ekranın)

Şu an Production = `www`, apex → www redirect. İkisi de Invalid çünkü DNS eski.

DNS düzelince:

- `https://www.halilmertdeveli.com.tr` açılır  
- `https://halilmertdeveli.com.tr` Vercel’de **308 → www** yapar (senin ayarın)

Apex’i birincil istersen: Domains → `halilmertdeveli.com.tr` → **Edit** → Production yap; `www` → Redirect to apex.

---

## 3) Site env (Production)

| Key | Value |
| --- | --- |
| `PORT` | `8080` |
| `Site__PublicBaseUrl` | `https://www.halilmertdeveli.com.tr` *(veya apex birincilse `https://halilmertdeveli.com.tr`)* |

---

## Kontrol

```bash
dig @8.8.8.8 A +short halilmertdeveli.com.tr
# 93.89.230.125 OLMAMALI → 76.76.21.21 (veya Vercel IP)

dig @8.8.8.8 CNAME +short www.halilmertdeveli.com.tr
# d0c3035e77d2cff7.vercel-dns-017.com.
```

**Şimdilik siteyi buradan gör:** https://yazilim-sitesi.vercel.app  

DnsEnable’da eski A’yı silmeden canlı domain **açılmaz** — kod tarafında başka yapılacak iş yok.

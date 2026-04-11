# OF Linux Scraper — Project Log

## Goal
Scrape content from OnlyFans on Linux (Fedora), since SCrawler is Windows-only.

---

## Status: BLOCKED on auth
OF invalidates sessions aggressively. User logs in via X.com OAuth which
leaves `auth_uid_<id>` / `auth_uniq_<id>` / `auth_hash` cookies empty.
ofscraper's `check_auth` → `me.scrape_user` fails → "auth failed quitting on error".

---

## Work Done

### Session 1 (Gemini)
- Created `of_scraper.py` — Python scraper mimicking SCrawler's signing logic
- Signing: fetches dynamic rules from DATAHOARDERS/dynamic-rules, SHA1 + checksum
- Basic flow: cookies → get user info → fetch 50 media items → download

### Session 2 (Claude)
**Bugs fixed in `of_scraper.py`:**
- Timestamp: originally seconds, should be **milliseconds** (confirmed from ofscraper `create_sign`)
- `remove_headers` field in rules respected
- `checksum_constants` (plural, per-index offsets) applied
- Multiple media source paths (`source.source`, `files.source.url`, `files.full.url`)
- Full pagination via `tailMarker` cursor loop
- Multiple rules source fallbacks
- Switched to `curl_cffi` for Firefox TLS fingerprint impersonation

### Session 3 (Claude) — Wine attempt
- Installed Wine 11.0 Staging + winetricks + .NET 4.6.1
- SCrawler launched but stuck in "wine configuration updating" 30+ min
- Uninstalled Wine — dead end

### Session 4 (Claude) — ofscraper route
- `pip install ofscraper` → 2.6.4.1 (banner) / 3.14.7 (dunamai git read)
- Needed `gcc-c++` and `python3-devel` for faust-cchardet build
- Patched `constants.py` DYNAMIC URL: deviint (404) → DATAHOARDERS
- Patched `utils/auth.py` `get_request_auth` to handle `format` field
  (DATAHOARDERS uses `format`, old code expected `start`/`end`)
- Wrote `~/.config/ofscraper/main_profile/auth.json` manually
  (this version has no `auth` subcommand)
- Used M-rcus OnlyFans-Cookie-Helper JSON to extract correct values
- **Key finding:** user_agent must match live browser (Firefox 149, not 135)
- **Key finding:** auth_uid_ is EMPTY for X.com OAuth logins (not missing — OF just doesn't set it)

---

## Current auth.json
`~/.config/ofscraper/main_profile/auth.json`
```json
{
  "auth": {
    "user_agent": "Mozilla/5.0 (X11; Linux x86_64; rv:149.0) Gecko/20100101 Firefox/149.0",
    "app-token": "33d57ade8c02dbc5a333db99ff9ae26a",
    "x-bc": "54175fa2598f42e21e02586fce10a75449d327b4",
    "auth_id": "19933344",
    "sess": "uu1bppn8jee5nvlhnpe508bpqb",
    "auth_uid_": ""
  }
}
```

### Session 5 (Gemini + Caveman)
- Verified `curl_cffi` successfully bypasses Cloudflare WAF. Missing headers return `[29141B1D]`.
- Added logic to fetch fresh `__cf_bm` / `_cfuvid` cookies before scraping to prevent CF block.
- Fixed signature `format` parsing to match Datahoarders `{:x}` and SCrawler logic.
- Timestamp correctly uses Unix milliseconds `str(round(time.time() * 1000))`.
- **Result:** API consistently returns `401 Unauthorized "Please refresh the page"`. 
- **Conclusion:** The signature is likely perfect, but the session cookie (`sess`) is fully invalidated on the server-side, OR the absence of `auth_uid_<id>` for X.com OAuth accounts strictly prevents API access outside the browser. A fresh manual login (Email/Password) is absolutely required to proceed.

## Patched ofscraper files
- `/home/mario/.local/lib/python3.14/site-packages/ofscraper/constants.py`
  DYNAMIC → `https://raw.githubusercontent.com/DATAHOARDERS/dynamic-rules/main/onlyfans.json`
- `/home/mario/.local/lib/python3.14/site-packages/ofscraper/utils/auth.py`
  `get_request_auth` now handles both `format` field and legacy `start`/`end`

## Flow trace (for resume)
1. `ofscraper -u baddslayer -o timeline`
2. `commands/scraper.py::scrapper()` → `check_auth()`
3. `check_auth` → `init.getstatus(headers)` → `me.scrape_user(headers)`
4. `scrape_user` hits `/api2/v2/users/me` with signing → **fails** (exception)
5. Retries 10x (tenacity), then `getstatus` returns "DOWN"
6. Logs "Auth Failed", calls `auth.make_auth()` interactively
7. In non-TTY context: silent exit with "auth failed quitting on error"

## Next steps to try
- [ ] Capture actual HTTP response from `/api2/v2/users/me` call
      (patch `me.py::_scraper_user_helper` to print/log `r.status_code` + `r.text`)
- [ ] Verify sess cookie still valid via raw curl:
      `curl -H "user-agent: Mozilla/5.0 ... Firefox/149.0" -H "cookie: sess=...; auth_id=19933344" https://onlyfans.com/api2/v2/users/me`
      (will fail signing but should show CF / session status)
- [ ] Re-login to OF in Firefox with email/password (not X.com OAuth) to get
      a complete cookie set including `auth_uid_<id>`
- [ ] Check for Cloudflare `__cf_bm` cookie — may need to forward it too
- [ ] Consider running ofscraper inside firefox-profile-aware wrapper

---

## Better Tools Available

### 1. ofscraper (in progress)
**Repo:** https://github.com/datawhores/OF-Scraper
Full-featured Python CLI. Blocked on auth — see Status above.

### 2. yt-dlp (limited)
Only free/public content. No signing.

### 3. SCrawler via Windows VM
Proxmox available. Spin up lightweight Windows VM, run natively.
**Recommended fallback** — Wine already proven to not work.

### 4. SCrawler via Wine — TRIED, FAILED
Stuck at "wine configuration updating". Uninstalled.

---

## Dynamic Rules Sources (from DynamicRulesAll.txt)
```
https://github.com/DATAHOARDERS/dynamic-rules/blob/main/onlyfans.json       ← works
https://github.com/DIGITALCRIMINAL/dynamic-rules/blob/main/onlyfans.json
https://github.com/datawhores/onlyfans-dynamic-rules/blob/main/dynamicRules.json
https://github.com/riley-access-labs/onlyfans-dynamic-rules-1/blob/main/dynamicRules.json
```

---

## Key Files
| File | Purpose |
|------|---------|
| `of_scraper.py` | Gemini/Claude custom scraper (deprioritized) |
| `INIT.md` | This file |
| `../SCrawler/API/OnlyFans/` | VB reference for signing |
| `~/.config/ofscraper/main_profile/auth.json` | ofscraper auth config |
| `~/.config/ofscraper/logging/main_profile_*/` | ofscraper debug logs |
| `~/onlyfans_cookies.json` | Extracted Firefox cookies |

## User context
- auth_id: `19933344`
- Logs in via X.com OAuth (complicates cookie extraction)
- Targets: `baddslayer` (paid, was `bdsmae`), `sinfuldeeds` (free)
- OF has been aggressively invalidating sessions every scrape attempt

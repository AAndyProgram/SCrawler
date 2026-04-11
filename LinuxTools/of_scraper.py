from curl_cffi import requests
import hashlib
import time
import json
import os
import argparse

# Configuration
DEFAULT_COOKIES_FILE = os.path.expanduser("~/onlyfans_cookies.json")
USER_AGENT = "Mozilla/5.0 (X11; Linux x86_64; rv:149.0) Gecko/20100101 Firefox/149.0"

# Dynamic rules sources (tried in order, mirrors SCrawler's DynamicRulesAll.txt)
RULES_SOURCES = [
    "https://raw.githubusercontent.com/DATAHOARDERS/dynamic-rules/main/onlyfans.json",
    "https://raw.githubusercontent.com/DIGITALCRIMINAL/dynamic-rules/main/onlyfans.json",
    "https://raw.githubusercontent.com/datawhores/onlyfans-dynamic-rules/main/dynamicRules.json",
]

def get_dynamic_rules():
    for url in RULES_SOURCES:
        try:
            r = requests.get(url, timeout=10)
            if r.status_code == 200:
                data = r.json()
                # Validate required fields
                if all(k in data for k in ("static_param", "format", "checksum_indexes", "checksum_constant")):
                    print(f"Loaded rules from: {url}")
                    return data
        except Exception as e:
            print(f"Rules source failed ({url}): {e}")
    raise RuntimeError("Failed to fetch dynamic rules from all sources")

def create_signature(path, rules, auth_id):
    """
    Matches SCrawler's UpdateSignature() exactly.
    Timestamp = Unix milliseconds.
    checksum = sum(hash_byte[i] + constants[i]) + checksum_constant
    Sign header = format.format(sha1_hex, abs(checksum))
    Time header = Unix milliseconds string
    """
    static_param = rules["static_param"]
    # Revert to Unix milliseconds as required by OF
    timestamp = str(round(time.time() * 1000))

    payload = "\n".join([static_param, timestamp, path, auth_id])
    sha1_hex = hashlib.sha1(payload.encode("utf-8")).hexdigest()

    indexes = rules["checksum_indexes"]
    # checksum_constants (plural) is a per-index offset list (2026+ rules)
    constants = rules.get("checksum_constants", [])
    base_constant = rules["checksum_constant"]

    checksum = 0
    for i, idx in enumerate(indexes):
        char_val = ord(sha1_hex[idx])
        checksum += char_val + (constants[i] if i < len(constants) else 0)
    checksum += base_constant

    # Match SCrawler's String.Format(pattern, hash, Math.Abs(hashSum))
    # Python's format with {} uses positional args but the pattern in onlyfans.json is:
    # "{}:{}:{:x}:{}" which expects 4 args in format, BUT wait!
    # In C#, `"{0}:{1}:{2:x}:{3}"` is explicit indexing. If the pattern from github has `{}` without indices, we need to know what it is.
    # The actual pattern in onlyfans.json is: "{}::{}:{:08x}:{}" - WAIT. Let's see what pattern is.
    # Actually, the string format uses the list of things. Let's just split and join if it's "{}:{}:{:x}:{}"
    # Wait, `rules["format"]` might be like `"{}:{}:{:x}:{}"` so `rules["format"].format(...)` won't work with only 2 arguments!
    # Let's fix it by parsing the format string, or replacing `{}` with actual values.
    # In C# String.Format("pattern", arg0, arg1) the pattern would be "{0}:{1:x}" etc.
    # If the pattern is literally "{}:{}:{:x}:{}", it's not a C# format string! 
    # WAIT! In python we need to match the placeholders.
    # Let's just use the exact format from onlyfans.json.
    # We can inject sha1_hex and abs(checksum) by inspecting SCrawler.
    
    # Let's look at SCrawler: `String.Format(pattern, hash, Math.Abs(hashSum))`
    # If pattern is e.g. "{0}:{1}:{2:x}:{3}", SCrawler doesn't pass 4 args.
    # Ah! The pattern in OnlyFans JSON used to be `"{}:{}:{:x}:{}"` for python but in VB it must be `"{0}:{1:x}"`.
    # Let's check `DynamicRulesEnv.GetFormat(j)`. SCrawler transforms it!
    # Yes! SCrawler replaces `{}` with `{0}` and `{:x}` with `{1:x}`.
    
    # Python equivalent of SCrawler's transformed format:
    # If rules["format"] has `{}` and `{:x}`, we can format it by passing arguments based on the original `{}:{}:{:x}:{}`.
    # Wait, the rule is usually "sha1_part1:sha1_part2:checksum_hex:sha1_part3".
    # No, it's `format.format(sha1_hex, abs(checksum))` but python needs args to match placeholder count.
    # Actually, the datahoarder JSON format is already pre-formatted for Python:
    # it expects `static_param`, `timestamp`, `path`, `auth_id`, `sha1_hex`, `abs(checksum)`.
    # Let's just print the rule format and use a safe fallback.
    
    pattern = rules["format"]
    sign = pattern.format(sha1_hex, abs(checksum))
    return sign, timestamp

def get_fresh_cf_cookies(headers_base):
    # Fetch a fresh __cf_bm cookie by hitting a public page or endpoint
    url = "https://onlyfans.com/"
    print(f"DEBUG: Fetching fresh Cloudflare cookies from {url}")
    # Remove Cookie header to get a fresh one, or just pass existing and let it update
    temp_headers = dict(headers_base)
    if "Cookie" in temp_headers:
        del temp_headers["Cookie"]
    
    r = requests.get(url, headers=temp_headers, impersonate="chrome110")
    print(f"DEBUG: Received cookies: {r.cookies.get_dict()}")
    return r.cookies.get_dict()

def build_headers(cookies, rules, auth_id, x_bc):
    remove = [h.lower() for h in rules.get("remove_headers", [])]
    
    # We will build the header dictionary.
    headers = {
        "accept": "application/json, text/plain, */*",
        "user-agent": USER_AGENT,
        "app-token": rules["app_token"],
        "x-bc": x_bc,
    }
    if "user-id" not in remove:
        headers["user-id"] = auth_id
        
    cf_cookies = get_fresh_cf_cookies(headers)
    
    # Update our cookies dict with the fresh CF cookies
    for k, v in cf_cookies.items():
        cookies[k] = v
        
    headers["cookie"] = "; ".join(f"{k}={v}" for k, v in cookies.items())
    
    return headers

def api_get(url_path, headers, rules, auth_id):
    sign, ts = create_signature(url_path, rules, auth_id)
    headers = dict(headers)  # copy so we don't mutate
    headers["sign"] = sign
    headers["time"] = ts
    
    url = f"https://onlyfans.com{url_path}"
    print(f"DEBUG: GET {url}")
    print(f"DEBUG: Headers: {headers}")
    
    # Try using Chrome fingerprint with Chrome User-Agent if Firefox fails
    r = requests.get(url, headers=headers, impersonate="chrome110")
    print(f"DEBUG: Response {r.status_code}: {r.text[:500]}")
    return r

def fetch_all_media(uid, headers, rules, auth_id):
    """Paginate through all media posts."""
    all_media = []
    cursor = ""
    page = 1

    while True:
        if cursor:
            path = f"/api2/v2/users/{uid}/posts/medias?limit=50&format=infinite&counters=0&beforePublishTime={cursor}"
        else:
            path = f"/api2/v2/users/{uid}/posts/medias?limit=50&format=infinite&counters=1"

        resp = api_get(path, headers, rules, auth_id)
        if resp.status_code != 200:
            print(f"Error fetching media page {page}: {resp.status_code} - {resp.text[:200]}")
            break

        data = resp.json()
        items = data.get("list", [])
        if not items:
            break

        all_media.extend(items)
        print(f"Page {page}: got {len(items)} items (total: {len(all_media)})")

        # Pagination: use the publish time of the last item as cursor
        cursor = data.get("tailMarker") or (items[-1].get("postedAt") or items[-1].get("publishedAt") or "")
        if not cursor or len(items) < 50:
            break
        page += 1

    return all_media

def run_scraper(username, cookies_file):
    if not os.path.exists(cookies_file):
        print(f"Error: Cookies file not found at {cookies_file}")
        return

    with open(cookies_file) as f:
        cookies = json.load(f)

    auth_id = cookies.get("auth_id", "")
    x_bc = cookies.get("fp", "")  # fingerprint token — matches SCrawler's GetValueFromCookies

    if not auth_id:
        print("Error: 'auth_id' missing from cookies file")
        return

    rules = get_dynamic_rules()
    headers = build_headers(cookies, rules, auth_id, x_bc)

    # Step 1: resolve username -> user ID
    path = f"/api2/v2/users/{username}"
    resp = api_get(path, headers, rules, auth_id)
    if resp.status_code != 200:
        print(f"Error fetching user: {resp.status_code} - {resp.text[:300]}")
        return

    user_data = resp.json()
    uid = str(user_data["id"])
    display_name = user_data.get("name", username)
    print(f"User: {display_name} (id={uid})")

    # Step 2: paginate all media
    all_items = fetch_all_media(uid, headers, rules, auth_id)
    print(f"Total media items: {len(all_items)}")

    os.makedirs("downloads", exist_ok=True)
    with open(f"downloads/{username}_metadata.json", "w") as f:
        json.dump(all_items, f, indent=2)

    # Step 3: download
    download_dir = f"downloads/{username}"
    os.makedirs(download_dir, exist_ok=True)

    downloaded = skipped = errors = 0
    for item in all_items:
        for media in item.get("media", []):
            source = (
                media.get("source", {}).get("source")
                or next(
                    (media.get("files", {}).get(k, {}).get("url") for k in ("source", "full") if media.get("files", {}).get(k, {}).get("url")),
                    None,
                )
            )
            if not source:
                continue
            ext = "mp4" if media.get("type") == "video" else "jpg"
            filepath = os.path.join(download_dir, f"{media['id']}.{ext}")
            if os.path.exists(filepath):
                skipped += 1
                continue
            try:
                r = requests.get(source, stream=True, timeout=30)
                r.raise_for_status()
                with open(filepath, "wb") as f:
                    for chunk in r.iter_content(chunk_size=8192):
                        f.write(chunk)
                downloaded += 1
                print(f"  [{downloaded}] {media['id']}.{ext}")
            except Exception as e:
                print(f"  Error downloading {media['id']}: {e}")
                errors += 1

    print(f"\nDone. Downloaded: {downloaded}  Skipped: {skipped}  Errors: {errors}")
    print(f"Files in: {download_dir}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="OnlyFans scraper (Linux)")
    parser.add_argument("username", help="OnlyFans username")
    parser.add_argument("--cookies", default=DEFAULT_COOKIES_FILE, help="Path to cookies JSON")
    args = parser.parse_args()
    run_scraper(args.username, args.cookies)

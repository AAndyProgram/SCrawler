import asyncio
import json
import os
import argparse
import base64
import subprocess
import time
import sys
from playwright.async_api import async_playwright

# Generic User Agent - Playwright will override this based on chosen browser
DEFAULT_UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"

# Platform-agnostic download base
if sys.platform == "win32":
    DOWNLOAD_BASE = os.path.join(os.environ.get("USERPROFILE", "C:\\"), "Downloads", "OnlyFans")
else:
    DOWNLOAD_BASE = os.path.expanduser("~/Downloads/OnlyFans")

class OFScraper:
    def __init__(self, username, profile_dir, user_agent=DEFAULT_UA):
        self.username = username
        self.profile_dir = os.path.abspath(profile_dir)
        self.download_dir = os.path.join(DOWNLOAD_BASE, username)
        self.metadata_dir = os.path.join(self.download_dir, "metadata")
        self.user_agent = user_agent
        os.makedirs(self.metadata_dir, exist_ok=True)
        
        self.captured_media = {} # id -> data
        self.captured_posts = []
        self.captured_messages = []
        self.user_info = {}
        self.uid = None

    async def handle_response(self, response):
        url = response.url
        if response.status != 200: return
        
        try:
            if f"/api2/v2/users/{self.username}" in url and "medias" not in url:
                data = await response.json()
                self.user_info = data
                self.uid = data.get("id")
                print(f"\r  [Capture] User info for {self.username} (UID: {self.uid})")
            elif "/posts/medias" in url:
                data = await response.json()
                items = data.get("list", [])
                self.extract_media(items, "media_tab")
                print(f"\r  [Capture] {len(items)} items from Media tab (Total unique: {len(self.captured_media)})", end="")
            elif "/posts" in url and "medias" not in url:
                data = await response.json()
                items = data.get("list", [])
                self.captured_posts.extend(items)
                self.extract_media(items, "timeline")
                print(f"\r  [Capture] {len(items)} items from Timeline (Total unique: {len(self.captured_media)})", end="")
            elif "/messages" in url:
                data = await response.json()
                items = data.get("list", [])
                self.captured_messages.extend(items)
                self.extract_media(items, "messages")
                print(f"\r  [Capture] {len(items)} items from Messages (Total unique: {len(self.captured_media)})", end="")
        except: pass

    def extract_media(self, items, source_tag):
        for item in items:
            media_list = item.get("media", [])
            for m in media_list:
                m_id = str(m.get("id"))
                if not m_id: continue
                source_url = m.get("source", {}).get("source") or next((m.get("files", {}).get(k, {}).get("url") for k in ("source", "full") if m.get("files", {}).get(k, {}).get("url")), None)
                if source_url and m_id not in self.captured_media:
                    self.captured_media[m_id] = {
                        "id": m_id,
                        "type": m.get("type"),
                        "source": source_url,
                        "captured_from": source_tag,
                        "timestamp": item.get("postedAt") or item.get("createdAt")
                    }

    async def auto_scroll(self, page):
        print("\nAuto-scrolling... (Capturing data as it loads)")
        last_height = await page.evaluate("document.body.scrollHeight")
        attempts = 0
        while True:
            await page.evaluate("window.scrollTo(0, document.body.scrollHeight)")
            await asyncio.sleep(2.5)
            new_height = await page.evaluate("document.body.scrollHeight")
            if new_height == last_height:
                attempts += 1
                if attempts >= 5: break
            else:
                attempts = 0
                last_height = new_height
            if await page.query_selector(".b-loader"): await asyncio.sleep(1)

    async def run(self, mode="media", browser_type="firefox"):
        async with async_playwright() as p:
            if browser_type == "chromium": bt = p.chromium
            elif browser_type == "webkit": bt = p.webkit
            else: bt = p.firefox

            context = await bt.launch_persistent_context(
                self.profile_dir,
                headless=False,
                viewport={'width': 1280, 'height': 720}
            )
            
            page = await context.new_page()
            try:
                from playwright_stealth import Stealth
                await Stealth().apply_stealth_async(page)
            except: pass
            
            page.on("response", self.handle_response)
            
            print(f"Opening OnlyFans via {browser_type}...")
            await page.goto("https://onlyfans.com/")
            
            try:
                await page.wait_for_selector(".b-sidebar", timeout=20000)
            except:
                print("\nLogin required. Please login in the browser window.")
                await asyncio.get_event_loop().run_in_executor(None, input, "Press Enter after you are logged in...")

            if mode == "media":
                print(f"Targeting Media tab for {self.username}")
                await page.goto(f"https://onlyfans.com/{self.username}/media")
                await self.auto_scroll(page)
            elif mode == "messages":
                print(f"Targeting Chats/Messages...")
                await page.goto(f"https://onlyfans.com/my/chats")
                print("Open the chat you want to scrape and scroll up.")
                await asyncio.get_event_loop().run_in_executor(None, input, "Press Enter when done capturing...")
            
            # Metadata & Cookies
            with open(os.path.join(self.metadata_dir, "user_info.json"), "w") as f: json.dump(self.user_info, f, indent=2)
            with open(os.path.join(self.metadata_dir, "media_list.json"), "w") as f: json.dump(list(self.captured_media.values()), f, indent=2)
            
            browser_cookies = await context.cookies()
            cookie_file = os.path.join(self.download_dir, f"cookies_{self.username}.txt")
            with open(cookie_file, "w") as f:
                for c in browser_cookies:
                    domain = c['domain'] if c['domain'].startswith('.') else '.' + c['domain']
                    f.write(f"{domain}\tTRUE\t{c['path']}\t{str(c['secure']).upper()}\t{int(c.get('expires', 0))}\t{c['name']}\t{c['value']}\n")
            
            print(f"\nCapture summary: {len(self.captured_media)} items.")
            await context.close()
            self.download_all(cookie_file)

    def download_all(self, cookie_file):
        media_list = list(self.captured_media.values())
        photos = [m for m in media_list if m["type"] == "photo"]
        videos = [m for m in media_list if m["type"] == "video"]
        queue = photos + videos
        
        print(f"\nDownloading to: {self.download_dir}")
        downloaded = skipped = errors = 0
        for i, m in enumerate(queue):
            subfolder = "Photos" if m["type"] == "photo" else "Videos"
            target_path = os.path.join(self.download_dir, subfolder)
            os.makedirs(target_path, exist_ok=True)
            ext = "mp4" if m["type"] == "video" else "jpg"
            filepath = os.path.join(target_path, f"{m['id']}.{ext}")
            
            if os.path.exists(filepath) and os.path.getsize(filepath) > 0:
                skipped += 1
                continue
                
            print(f"  [{i+1}/{len(queue)}] {m['id']}.{ext}... ", end="", flush=True)
            try:
                # Use curl if available, fallback to simple request
                cmd = ["curl", "-L", "-s", "--fail", "-o", filepath, "-A", self.user_agent, "-b", cookie_file, m["source"]]
                res = subprocess.run(cmd, timeout=600)
                if res.returncode == 0:
                    downloaded += 1
                    print("Success.")
                else:
                    print(f"Failed.")
                    errors += 1
            except:
                print("Error.")
                errors += 1
            time.sleep(0.1)

        print(f"\n--- SESSION FINISHED ---")
        print(f"New: {downloaded} | Existing: {skipped} | Failed: {errors}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="OF Browser Scraper v3")
    parser.add_argument("username", help="OF username")
    parser.add_argument("--profile", default="./of_profile", help="Browser profile directory")
    parser.add_argument("--mode", choices=["media", "messages"], default="media", help="Scrape media tab or messages")
    parser.add_argument("--browser", choices=["firefox", "chromium", "webkit"], default="firefox", help="Browser engine")
    args = parser.parse_args()
    
    scraper = OFScraper(args.username, args.profile)
    asyncio.run(scraper.run(args.mode, args.browser))

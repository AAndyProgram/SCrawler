# OnlyFans Linux Tools for SCrawler

This directory contains Python-based tools for scraping OnlyFans content on Linux, bypassing common blocks encountered with pure CLI tools.

## Prerequisites

- **Python 3.10+**
- **Playwright:** `pip install playwright playwright-stealth`
- **Firefox Engine:** `playwright install firefox`
- **curl:** Used for stable large file downloads.

## Tools

### 1. `of_playwright.py` (Recommended)
Uses a real browser context to capture network traffic and handle signing/Cloudflare automatically.

#### Setup
1.  Launch the script:
    ```bash
    python3 of_playwright.py [USERNAME]
    ```
2.  A Firefox window will open. **Login manually** to OnlyFans.
3.  The session is saved in `./of_profile` so you won't need to login every time.

#### Usage
- **Media Tab:** `python3 of_playwright.py [USERNAME]`
  - Automatically navigates to the media tab and auto-scrolls to the bottom to capture all items.
- **Messages:** `python3 of_playwright.py [USERNAME] --mode messages`
  - Navigate to the chat and scroll manually to capture content.
- **Photos Only:** Add `--photos` flag.

Files are saved to `~/Downloads/OnlyFans/[USERNAME]/`.

### 2. `of_scraper.py` (Legacy)
Signature-based scraper mimicking SCrawler's logic. Requires manual cookie extraction. Useful for targeted API calls but more prone to Cloudflare blocks.

## Troubleshooting

- **[29141B1D] Error:** This is a Cloudflare WAF block. If encountered, use the Playwright tool (`of_playwright.py`) as it uses a real browser environment.
- **Login required:** Ensure you are logged in in the browser window opened by Playwright.
- **Slow Downloads:** Large video files are handled by `curl` for maximum stability.

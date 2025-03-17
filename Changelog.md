# 2025.3.17.0

*2025-03-17*

- Added
  - **TikTok: downloading photos**
- Updated
  - gallery-dl up to version **1.29.2**
- Fixed
  - Sites
    - Facebook: reels aren't downloaded from noname profiles
    - PornHub: newly added users aren't downloading
    - Threads: users aren't updated if there is a pinned post

# 2025.2.25.0

*2025-02-25*

- Added
  - Sites:
    - **Bluesky**
    - Facebook: **`Reels` downloads**
    - OnlyFans: default value for `App-Token`
    - Pinterest: **sub-boards downloading**
    - Threads: ability to manually change `UserName`
    - Twitter:
      - new icon support
      - **sleep timers to fully download large profiles**
  - Feed:
    - ability to invert selection
    - open post URL when double-clicking on subscription image
  - Minor improvements
- Updated
  - yt-dlp up to version **2025.02.19**
  - gallery-dl up to version **1.28.5**
- PluginProvider
  - `IPluginContentProvider`: added property `NameTrue`
- Fixed
  - Sites:
    - Facebook: videos are not downloading
    - LPSG: simplified 403 error
    - PornHub: photos & videos are not downloading
    - Reddit: **token does not update automatically**
    - Threads: **data is not downloading**
  - Minor bugs

# 2025.1.12.0

*2025-01-12*

- Added
  - Sites:
    - YouTube (standalone app):
      - ability to add channel name to file name (`Add channel to file name`)
      - adding channel name and video URL to info file
    - OnlyFans: **built-in usage of DRM keys**
    - Threads: automatically change `heic` extension to `jpg`
    - Twitter: download broadcasts *(user option)*
  - Minor improvements
- Updated
  - yt-dlp up to version **2024.12.23**
  - gallery-dl up to version **1.28.3**
  - **OF-Scraper** up to version **3.12.9** *(you must update it personally)*
- Fixed
  - Sites:
    - DownDetector: fixed 403 error
    - OnlyFans: **DRM videos not downloading**
    - xHamster: some videos are not downloading
    - YouTube: **communities are not downloading** *(see settings in wiki)*
  - Minor bugs

# 2024.11.21.0

*2024-11-21*

- Added
  - Sites:
    - Instagram:
      - setting to skip errors without disabling download *(site settings)*
      - settings to force update of username and/or user information *(user settings)*
      - setting to continue downloading profile if error 560 occurs while downloading user stories *(site settings)*
      - improve username update algorithm
    - YouTube: 404 error handling (community)
  - Main window: add extra buttons for special download (limited and dated) in collection
  - Global settings: ability to change the feed opening shortcut (`Ctrl+F`/`Alt+F` *(Settings - Behavior)*)
  - Minor improvements
- Updated
  - yt-dlp up to version **2024.11.18**
  - gallery-dl up to version **1.27.7**
- Fixed
  - Users: network paths aren't working
  - Main window: in some cases users are not updated in the list
  - Minor bugs

# 2024.10.24.0

*2024-10-24*

- Added
  - YouTube (standalone app):
    - settings `Embed thumbnail (cover)` and `Allow webm formats`
    - changed cover selection for music downloads
    - allow `webm` formats if there are no `mp4` formats via http protocol (issue #211)
  - Sites:
    - Instagram:
      - **ability to manually change username**
      - **mark user as non-existent if user `ID` cannot be obtained**
    - Twitter: **ability to manually change username**
  - Main window: add users search button to 'Info' menu
  - Minor improvements
- Updated
  - yt-dlp up to version **2024.10.22**
  - gallery-dl up to version **1.27.6**
- Fixed
  - YouTube (standalone app): adding incorrect playlist lines
  - Reddit: incorrect UNIX date parsing
  - Can't change data path (issue #206)
  - Minor bugs

# 2024.9.2.0

*2024-09-02*

- Added
  - Instagram: options to enable/disable image extraction from video
  - Feed: **prompt before moving entire feed/session**
  - Main window: hotkeys `Alt+U` and `Ctrl+U` to open the user search form
  - Minor improvements
- Updated
  - gallery-dl up to version **1.27.3**
- Fixed
  - **OnlyFans**: data is not downloading
  - YouTube (SCrawler): incorrect parsing of video page
  - Minor bugs

# 2024.8.10.0

*2024-08-10*

- Added
  - Feed: button to open file folder
- Updated
  - yt-dlp up to version **2024.08.06**
  - gallery-dl up to version **1.27.2**
- Fixed
  - YouTube (standalone app): **video is being parsed using cookies but is not downloading** *(Issue #205)*

# 2024.8.1.0

*2024-08-01*

- Added
  - Minor improvements
- Updated
  - yt-dlp up to version **2024.08.01**

# 2024.7.24.0

*2024-07-24*

- Added
  - YouTube (standalone app)
    - ability to convert non-`AVC` codecs (eg `VP9`) to `AVC` (`Settings` - `Defaults Video` - `Convert non-AVC codecs to AVC`)
    - add the ability to set the playlist creation mode: absolute links, relative links, or both (`Settings` - `Music` - `Create M3U8: creation mode`)
  - Threads: **saved posts downloading**
  - Feed
    - hotkeys `Esc` and `Ctrl+W` to close the form
    - the ability to search for missing files in *special feeds*
  - Scheduler: the ability to execute a script after the scheduler plan is executed *(`Settings` - `Behavior`)*
  - Main window:
    - added hotkey `Ctrl+F` to show the feed
    - changed the hotkey from `Ctrl+F` to `Alt+F` to show the search form
- Updated
  - yt-dlp up to version **2024.07.16**
- Fixed
  - YouTube (standalone app): video files with line breaks in the name do not download correctly
  - OnlyFans: rules parsing bug
  - Minor bugs

# 2024.6.25.0

*2024-06-25*

**ATTENTION! To support downloading of DRM protected videos (OnlyFans), please update OF-Scraper to version [3.10.7](https://github.com/datawhores/OF-Scraper/releases/tag/3.10.7) (download `zip`, not `exe`).**

- Added
  - OnlyFans: **new dynamic rules updating algorithm**
  - Feed: ability to set the last session as the current one
- Updated
  - gallery-dl up to version **1.27.1**
- Fixed
  - Minor bugs

# 2024.6.10.0

*2024-06-10*

- Added
  - YouTube (standalone app): add option to add extracted MP3 to playlist (`Settings` - `Defaults Video` - `Add extracted MP3 to playlist`)
  - Feed
    - settings to show/hide site name and file type from media title
    - ability to move/copy files of a loaded feed/session to another location
    - ability to reset current session
- Fixed
  - Minor bugs

# 2024.6.6.0

*2024-06-06*

**ATTENTION!**
1. **To support downloading of DRM protected videos (OnlyFans), please update OF-Scraper to version [3.10](https://github.com/datawhores/OF-Scraper/releases/tag/3.10) (download `zip`, not `exe`).**
2. **If there is a `OFScraperConfigPattern.json` file in the SCrawler settings folder, replace the text of the file with [this text](https://github.com/AAndyProgram/SCrawler/blob/main/SCrawler/API/OnlyFans/OFScraperConfigPattern.json).**
3. **Set the value to `Dynamic rules` (in the site settings) = `https://raw.githubusercontent.com/Growik/onlyfans-dynamic-rules/main/rules.json`.**

- Added
  - OnlyFans: new OF-Scraper option (`keydb_api`)
  - Minor improvements
- Fixed
  - OnlyFans: **data is not downloading**
  - Minor bugs

# 2024.6.4.0

*2024-06-04*

**If you were using the [`yt-dlp-TTUser`](https://github.com/bashonly/yt-dlp-TTUser) plugin, you should remove it because this plugin was added to yt-dlp itself! Read more [here](https://github.com/AAndyProgram/SCrawler/wiki/Settings#tiktok-requirements).**

- Added
  - Added highlighting of scheduler plans (working, stopped, pending, etc.)
  - YouTube (standalone app): add option to add the video upload date before/after the file name (`Settings` - `Defaults` - `Add date to file name`)
  - Twitter: **`Communities` downloading**
  - Feed: ability to select one of the download sessions and set it as the current session
  - Minor improvements
- Updated
  - yt-dlp up to version **2024.05.27**
  - gallery-dl up to version **1.27.0**
- Fixed
  - Twitter: deleting user directory when redownloading missing posts
  - Minor bugs

# 2024.5.19.0

*2024-05-19*

- Added
  - YouTube (standalone app): add upload date to description (request #192) (`Settings` - `Info` - `Create description files: add upload date`, `Create description files: create without description`).
- Fixed
  - YouTube (SCrawler): advanced settings are not saved when changed

# 2024.5.18.0

*2024-05-18*

- Added
  - YouTube (standalone app): highlight frame rates higher/lower than this value (`Settings` - `Defaults Video` - `Highlight FPS (higher/lower)`).
  - Sites
    - Instagram: 'DownDetector' support to determine if the site is accessible
    - Reddit: change the naming method of video files (hosted on Reddit) to the `YYYYMMDD_HHMMSS` pattern
    - Twitter
      - `Likes` downloading *(user settings)*
      - **changed domain from twitter.com to x.com**
  - Site settings: group options by category
  - Minor improvements
- PluginProvider
  - `PropertyOption` attribute: set category name when `IsAuth = True`
  - `ISiteSettings`: added `UserAgentDefault` property
- Updated
  - gallery-dl up to version **1.27.0-dev**
- Fixed
  - Sites
    - Instagram: incorrect definition of pinned posts
    - Threads: new posts are no longer downloaded from profiles with pinned posts
    - Reddit: bypass error 429 for saved posts
    - Twitter: **data is not downloading due to domain change from twitter.com to x.com**
  - Minor bugs

# 2024.5.4.0

*2024-05-04*

- Added
  - YouTube (standalone app): setting to remove specific characters (`Defaults` - `Remove characters`)
  - Instagram: simplify the `Connection closed` error
  - Users search: add `Friendly name` to search results
- Fixed
  - YouTube (standalone app): incorrect download processing when the file name ends with a dot (Issue #188)
  - The program is freezes when editing users in some cases
  - Sites
    - Reddit: token update error
    - Threads: unable to obtain credentials (`ID`)

# 2024.4.26.0

*2024-04-26*

- Added
  - Site settings: the values that can be extracted from cookies immediately populate fields
  - Feed: ability to load the last session of the current day (if it exists) as the current session after restarting SCrawler
  - Users search: include friendly name matches in search result
- Updated
  - gallery-dl up to version **1.26.9**
- Fixed
  - xHamster: **saved posts aren't downloading**

# 2024.4.14.0

*2024-04-14*

- Fixed
  - Facebook: can't get tokens

# 2024.4.13.0

*2024-04-13*

- Added
  - Minor improvements
- PluginProvider
  - IPluginContentProvider: added `ResetHistoryData` function
- Fixed
  - Sites
    - TikTok: remove last download date when erasing history data
    - YouTube: remove last download date when erasing history data
    - Instagram: **saved posts aren't downloading**

# 2024.4.10.0

*2024-04-10*

**For those of you who use TikTok, it is highly recommended to update TikTok plugin (using [these instructions](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-install-yt-dlp-ttuser-plugin)) to the latest version!**

**ATTENTION! This version includes changes to downloading groups (including the scheduler) and the settings file. Once you start using it, you won't be able to downgrade. I recommend making a backup of your SCrawler settings folder. It is also recommended to check all of your download groups settings and scheduler plans settings.**

- Added
  - Sites
    - TikTok: more settings for standalone downloader
    - Instagram:
      - automatically reset download options after updating credentials
      - **ability to extract an image (if it exists) from a video that contains a static image**
      - updated reels downloading function
      - GraphQL support
      - request timers (per request)
    - OnlyFans:
      - **download stories**
      - option to disable timeline downloading
    - Reddit: added `Check image` setting (unchecked by default) to make Reddit downloading faster
  - **YouTube (standalone app)**:
    - **the ability to add downloaded item(s) to a playlist**
    - **the ability to create a playlist from downloaded music**
    - calculation and display of the actual size of downloaded files
    - **the ability to change audio bitrate** *(you can also set the default bitrate in  `Defaults Audio` - `Bitrate`)*
    - **embed thumbnail in the extracted audio (`mp3` only) as cover art** (Settings: `Defaults Audio` - `Embed thumbnail (extracted files)`)
    - Exclude `drc` *(dynamic range compression)* from parsing results
  - Standalone downloader: 
    - allow thumbnail to be saved with file
    - calculation and display of the actual size of downloaded files
  - Feed:
    - add hotkeys: `Home`, `End`, `Up`, `Page Up`, `Down`, `Page Down`
    - ability to save/load views
  - Scheduler: **`All` and `Default` options removed.** *Only `Disabled`, `Specified` and `Groups` are available.*
  - Download groups: **filter users who have been (not)downloaded in the last `x` days** *(download groups, advanced filter (main window), scheduler)*.
  - Main window:
    - ability to save/load views (`View` - `Save/Load view`)
    - **all filter options from the `View` menu have now been moved to `Filter`**
  - Settings:
    - ability to confirm mass download using the `F6` key *(to protect against accidental pressing) (`Settings` - `Behavior`)*
    - the ability to find the program environment
    - default headers (`Settings` - `Headers`)
  - Added the ability to display group users *(works in scheduler, scheduler plans, download groups)*
  - Added exclusion of last 3 days from deleting session files
  - Other improvements
- Updated
  - **yt-dlp up to version 2024.04.09**
- PluginProvider
  - Add `PropertyValueEventArgs` class
  - PropertyValue:
    - add `Checked` parameter
    - add `OnCheckboxCheckedChange` handler
  - ISiteSettings:
    - add `CMDEncoding`, `EnvironmentPrograms` properties
    - add `EnvironmentProgramsUpdated` function
- Fixed
  - Sites
    - PornHub: some videos won't download
    - xHamster:
      - some videos are missing when downloading creators
      - user videos aren't downloading
    - **JustForFans: fixed video downloading**
    - TikTok (standalone downloader): files with long names aren't downloaded
  - Users: incorrect decision to set last update date, which resulted in an incorrect last download date for some users
  - Feed: a scrolling bug where the feed scrolls up after returning to it
  - Minor bugs

# 2024.2.25.0

*2024-02-25*

- Added
  - A `Feed` button has been added to notifications
  - Feed:
    - ability to merge multiple special feeds into one
    - ability to select all/none media
    - ability to add to a special feed(s) with removal from the current one
    - the name of the loaded feed is now displayed in the form title
    - `Refresh` button now refreshes the loaded feed
    - ability to move/copy media
  - Scheduler: the ability to move tasks (higher, lower) *(just a view attribute, doesn't affect the scheduler)*
  - YouTube (standalone app): add `Open file` to the context menu
  - YouTube (standalone app): **the ability to edit each playlist item**
  - YouTube (standalone app): **embed thumbnail in the audio/video as cover art** (Settings: `Defaults Audio` - `Embed thumbnail`; `Defaults Video` - `Embed thumbnail (video)`)
  - Instagram: the `csrftoken` can now be automatically extracted from cookies
  - Instagram: remove `x-ig-www-claim` from settings
  - Threads: the `csrftoken` can now be automatically extracted from cookies
  - Threads: simplify 500 error when updating tokens
  - Facebook: simplify token update errors
  - OnlyFans: handle 500 error
  - Plugins: added `ReplaceInternalPluginAttribute` attribute
  - Other improvements
- Fixed
  - Main window: incorrect sorting of profiles and collections
  - Standalone downloader: url array form doesn't show scrollbars
  - Feed: image rendering bug
  - YouTube (standalone app): audio codec does not change when changing audio/video in the video options form
  - Instagram: error downloading single post
  - TikTok: files with long names aren't downloaded
  - Minor bugs

# 2024.1.26.0

*2024-01-26*

- Added
  - YouTube (standalone app): **the ability to reduce video FPS**
  - TikTok: the ability to use a regex to clean the title
  - YouTube (SCrawler): the ability to ignore community errors
- Fixed
  - Instagram: stories (user) downloading with the wrong aspect ratio for some users
  - Minor bugs

# 2024.1.20.0

*2024-01-20*

- Added
  - Instagram: **the ability to download reels**
  - LPSG: handle 404 error

# 2024.1.18.0

*2024-01-18*

- Fixed
  - Main window: incorrect collection sorting
  - xHamster: some user videos were not downloaded
  - YouTube (standalone app): URL array form doesn't show scrollbars
  - Minor bugs

# 2024.1.12.1

*2024-01-12*

- Added
  - YouTube (SCrawler): data downloading by dates
  - Feed: ability to merge multiple session feeds into one
  - Feed: remove session number from special feeds
- Fixed
  - **Instagram**: stories (user) downloading with the wrong aspect ratio for some users
  - YouTube: incorrect opening of a post from the feed
  - YouTube: wrong date to data parsing

# 2024.1.12.0

*2024-01-12*

- Added
  - Feed: added a prompt before clearing the current session
  - xHamster: creators
  - YouTube communities: add error to log
  - Added scheduler to tray menu
  - Other improvements
- Fixed
  - Feed: there is no option to create a new feed when adding checked items
  - **Instagram**: downloading of tagged posts
  - xHamster: profiles are not downloading
  - Minor bugs

# 2023.12.27.0

*2023-12-27*

- Added
  - Notification of new log data
  - OnlyFans: **OF-Scrapper support to download DRM protected videos**
  - Other improvements
- Fixed
  - The default options are changed (`Favorite`, `Temporary`, etc.) when changing an account for a created user
  - When changing the account for a created user, the new account does not apply to that user until SCrawler is restarted
  - Saved posts: session file is not updated when new data is added
  - Minor bugs

# 2023.12.15.0

*2023-12-15*

- Fixed
  - Twitter: some twitter profiles don't download completely
  - Minor bugs

# 2023.12.14.0

*2023-12-14*

- Added
  - YouTube: options `Create thumbnail files (video)` and `Create thumbnail files (music)`
  - YouTube: `Select all` and `Select none` buttons

# 2023.12.13.0

*2023-12-13*

- Added
  - YouTube (standalone app): additional options for downloading channels
- Updated
  - gallery-dl up to version 1.26.4
- Fixed
  - Feed: saved posts are added to the end of the feed
  - xHamster: some videos won't download

# 2023.12.10.0

*2023-12-10*

- Updated
  - gallery-dl up to version 1.26.4-dev
- Fixed
  - Twitter: data is not downloading

# 2023.12.7.0

*2023-12-07*

- Added
  - Saved posts: add downloaded saved posts to the feed
  - **YouTube (SCrawler): the ability to download YouTube user community feeds**
  - Main window: add `Alt+A` hotkey to show scheduler
  - Main window: add `Alt+P` hotkey to show progress form
  - YouTube: check of adding a URL if it has already been downloaded
  - YouTube: ability to check for a new version at start
  - **Updater**
- Fixed
  - Standalone downloader: URL files are not deleted along with the file
  - Minor bugs

# 2023.11.25.0

*2023-11-25*

- Fixed
  - Reddit: missing refresh token button in the settings form

# 2023.11.24.0

*2023-11-24*

For those of you who use TikTok, I recommend updating [TikTok plugin](https://github.com/bashonly/yt-dlp-TTUser) to the latest version using [these instructions](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-install-yt-dlp-ttuser-plugin).

- Added
  - Automation: manual task option
  - Scheduler: add scheduler name to form title
  - Feeds: update when users' location and/or basic information changes
  - Reddit: special notification for error 429
  - TikTok: ID, username and friendly name extraction from data
  - TikTok: new option `Use video date as file date`
  - YouTube: absolute path for a single playlist
- Updated
  - yt-dlp up to version 2023.11.16
- Fixed
  - Scheduler: scheduler change error
  - Twitter: JSON deserialization error
  - xHamster, XVideos, PornHub, ThisVid: incorrect parsing of search queries
  - YouTube: the file name is not changed manually
  - YouTube: path not set when adding array to download
  - Minor bugs

# 2023.11.17.0

*2023-11-17*

- Added
  - **Facebook**
  - **Multi-account**
  - **Special feeds**
  - Site settings: option `Download saved posts`
  - Standalone downloader: support for multiple account
  - PornHub: add playlists downloading
  - YouTube: ability to download subtitles **and** `CC` if they both exists
  - Other improvements
- PluginProvider
  - `IDownloadableMedia`: added `AccountName` property
  - `IPluginContentProvider`: added `AccountName` property
  - `ISiteSettings`: added properties: `AccountName`, `Temporary`, `AvailableText`, `DefaultInstance`; added functions: `Clone`, `Update`, `Delete`; removed `Load` function; implement `IDisposable` interface
  - `PropertyValue`: added functions: `BeginInit`, `EndInit`, `Clone`
  - `Attributes.DoNotUse` - add `Value` field
- Fixed
  - Instagram: handling 401 error
  - OnlyFans: handling 401 error
  - xHamster: handling 503 error
  - xHamster: incorrect parsing of search queries
  - XVideos: incorrect parsing of search queries
  - ThisVid: incorrect parsing of search queries
  - PornHub: incorrect parsing of search queries
  - Automation: handle automation start error (in some cases) when changing scheduler
  - Minor bugs

# 2023.10.10.0

*2023-10-10*

- Added
  - Notification if the user has disabled downloading from the site
  - Standalone downloader: new setting `Create URL files`
  - Changed the sessions naming method to be more intuitive
  - Settings that allow the user to change the number of saved session (`Settings` - `Feed` - `Store session data`)
  - **YouTube: new settings `Create URL files` and `Create description files`**
  - YouTube: added the `Clear selected` button
  - YouTube: group the `Clear and remove` buttons in the menu
- Fixed
  - Reddit: unable to save settings without OAuth data
  - JustForFans: rewritten m3u8 downloader
  - JustForFans: downloading of missing posts
  - JustForFans: download to the date
  - JustForFans: corrupted files
  - Threads: new token is not saved if it was received during download
  - ThisVid: parsing stops when new videos are added
  - YouTube: file name is missing when destination is changed by selecting one of the saved locations
  - YouTube: missing files still appear in the list
  - Collections: labels are removed when creating a new collection
  - Standalone downloader: cached thumbnail is not removed when item is removed from the list
  - Minor bugs

# 2023.10.1.0

*2023-10-01*

- Added
  - **Threads.net**
  - YouTube: add URL standardization
- Fixed
  - UserEditor: disable updating labels if they haven't changed
  - Collections: incorrect updating of colors and labels when adding a new user
  - RedGifs: incorrect handling of error 410
  - Mastodon: hide error 503
  - JustForFans: some profiles won't download
  - Minor bugs

# 2023.9.21.0

*2023-09-21*

- Fixed
  - PornHub: videos are not downloading

# 2023.9.20.0

*2023-09-20*

- Added
  - **Instagram: user active (non-pinned) stories (Issue #17)**
  - Reddit: reduce the number of token updates (refresh the token if there are Reddit users in the download queue)
  - YouTube (standalone app): priority download protocol *(`Settings` - `Defaults` - `Protocol`)* (you can now select the default protocol you want to download media on: `Any`, `https`, `m3u8`))
  - Automation: ability to change schedulers (`Download` - `Automation` - `Script icon`)
  - Collections: update colors for the added user
- Fixed
  - YouTube: can't detect `shorts` links
  - Incorrect MD5 validation initial value
  - Instagram: handle error 500
  - Collections: update labels only for the added user

# 2023.8.27.0

*2023-08-27*

- Added
  - **JustForFans**
  - Advanced download (`Download` - `Download (advanced)`)
  - Advanced filter (`View` - `Advanced filter`)
  - Auto downloader: cloning plans
  - Feed: add button to go to custom page
  - Special log for non-existent users
  - Twitter: group 'limit' notifications
  - Ability to set custom color for subscription users
  - Other improvements
- Fixed
  - Auto downloader: new plan date display bug
  - Auto downloader: downloading stuck
  - Minor bugs

# 2023.8.6.0

*2023-08-06*

- Added
  - The ability to remove user data and/or download history for redownload
  - **Subscription** mode
  - Settings to change the program title and information in the program information
  - Settings for saving video thumbnail along with the file or in the cache (temporary cache or permanent cache)
  - A bug report form to create a bug report or say something nice to the developer :blush:
  - Prevent adding site-specific labels when adding to a collection
  - Ability to select custom user highlighting in the main window and feed.
  - Add a notification to the log if the user is not found on the site
  - Added visualization of users download queue
  - Ability to set more than one global paths
  - Improve user paths changing: now you can also simply move the user/collection to another global location
  - Ability to move multiple user/collection to another location
  - Download groups: added `Subscription` options
  - Download groups: the ability to set the number of users to download
  - Auto downloader: new group options
  - Auto downloader: additional skip options
  - Auto downloader: added force start
  - Feed: press `Ctrl+G` to go to a specific page
  - Feed: added site icon to post
  - Feed: always using `Friendly name` instead of `UserName` if it exists
  - Missing posts: the ability to delete all missing posts
  - Standalone downloader: add the ability to store download locations and quickly select after
  - Standalone downloader: add `Ctrl+O` hotkey to select destination path
  - Standalone downloader: add `Alt+O` hotkey to select destination path and save it to download locations
  - User editor: ability to hide/show site-specific labels in collection editing mode
  - Main window: filters by subscription and user
  - Instagram: if the user is not found on the site, SCrawler will check for a new user name
  - OnlyFans: handling of `504` and `429` errors
  - OnlyFans: the `sec-ch-ua` header is now optional
  - OnlyFans: ability to download 'Highlights" and media from chats
  - PathPlugin: incorrect detection of path existence
  - PornHub: completely rewritten videos parser
  - PornHub: now you choose which videos you want to download (uploaded, tagged, private, favorites)
  - PornHub: subscription mode
  - PornHub: ability to download search queries and search categories
  - Reddit: ability to set the number of concurrent downloads
  - Reddit: added bearer token (optional)
  - Reddit: added OAuth authorization (optional)
  - Reddit: options to use the bearer token for the timeline and/or saved posts
  - Reddit: option to disable the use of cookies for the timeline
  - ThisVid: now you can also download user's favorite videos
  - ThisVid: ability to download search queries, search categories and search tags
  - ThisVid: subscription mode
  - Twitter: new options: `Use the appropriate model`, `New endpoint: search`, `New endpoint: profiles`, `Abort on limit`, `Download already parsed` and `Media Model: allow non-user tweets`
  - Twitter: new user option `Force apply`
  - xHamster: ability to download search queries, search categories and search tags
  - xHamster: subscription mode
  - xHamster: pornstars download
  - XVideos: ability to download search queries, search categories and search tags
  - XVideos: subscription mode
  - YouTube: added `Output path: ask for a name` and `Output path: auto add` settings
  - YouTube: added the ability to store download locations and quickly select after
  - YouTube: subscription mode
  - Plugins.Attributes: added `DependentFields` attribute
  - Plugins.Attributes: replace `Dependencies` with `Arguments` (`PropertyUpdater` attribute)
  - Plugins.IPluginContentProvider: added `Options` and `IsSubscription` properties
  - Plugins.ISiteSettings: added `SubscriptionsAllowed` property
  - Plugins.ExchangeOptions: added `Options` field
  - Plugins: added `ExitException`
  - Other improvements
- Updated
  - gallery-dl up to version 1.25.8
  - yt-dlp up to version 2023.07.06
  - LibVLCSharp up to 3.7.0
  - VideoLAN up to 3.0.18
- Fixed
  - **TikTok** supported again!
  - Auto downloader: excluded labels and sites in default mode are not respected
  - Download info: does not remember the last size and location
  - Download info: hide unnecessary error
  - Feed: `webm` photos not showing
  - Search users: incorrect search by name
  - OnlyFans: incorrect parsing of username containing dots
  - OnlyFans: incorrect error handler
  - Reddit: Handling error 502 (Reddit data not downloading)
  - RedGifs: incorrect behavior when updating token
  - Twitter: gifs are not downloading
  - xHamster: some channels cannot be downloaded or are not fully downloaded
  - YouTube: re-saving elements when loading a video list
  - YouTube: files were not deleted when the delete button was clicked
  - YouTube: a bug that caused the video to redownload
  - Minor bugs

# 2023.6.19.0

*2023-06-19*

- Added
  - **OnlyFans**
  - YouTube: make the playlists parsing progress more informative
  - YouTube: add `Add` button to tray
  - YouTube: add `Ctrl+Click` on tray icon to add download
  - YouTube: add setting `Download on click in tray: show form`
  - Minor improvements to progress bars
  - Other improvements
- Fixed
  - YouTube: incorrect sorting algorithm
  - LPSG: some files didn't download
  - Reddit: downloaded gifs are static (Issue #141)
  - xHamster: videos are not downloading or downloading incorrectly (Issue #144)
  - Progress bar bugs
  - Minor bugs

# 2023.6.9.0

*2023-06-09*

- Fixed
  - YouTube: opening paths to downloaded playlists and channels
  - Twitter: make the algorithm faster
  - Make progress more informative

# 2023.6.8.0

*2023-06-08*

- Added
  - YouTube: append artist name to music playlist output path
  - YouTube: save thumbnail path for playlist and channel
- Fixed
  - YouTube: opening paths to downloaded playlists and channels
  - Twitter: profile not fully downloaded
  - Corrected form size for small monitors (Issue #136)

# 2023.6.5.0

*2023-06-05*

- Added
  - **Instagram**: add additional authorization headers
  - Setting to prevent user icon and banner from downloading (Request #129)
  - Add standalone downloader to tray context menu
  - YouTube downloader: added `Replace modification date` property
  - Minor improvements
- Fixed
  - Fascist **Twitter**: posts not downloading (new API)
  - Main window: refill bug when the number of filtered profiles = 0
  - Standalone downloader: new items are not added to the queue
  - Standalone downloader: bug when not downloaded videos do not appear in the list when loading the program
  - Standalone downloader: add videos array not working
  - Saved posts: remove main progress perform when downloading saved posts
  - Minor bugs

# 2023.5.12.0

*2023-05-12*

- Added
  - Advanced progress (make progress bars more informative)
  - User metrics calculation
  - Reddit: improve parsing function
  - PornHub: add `Download UHD` option
- Fixed
  - MD5 GIF hash bug
  - Mastodon: handle 'Forbidden' error
  - Mastodon: bug in parsing non-user posts
  - Pinterest: remove cookies requirement for saved posts
  - PornHub: resolutions issue
  - Reddit: missing & broken images bug
  - Main window: collection pointing bug 

# 2023.4.28.0

*2023-04-28*

- Added
  - **YouTube**
  - **YouTube Music**
  - **Mastodon**
  - **Pinterest**
  - **ThisVid**
  - **YouTube downloader (standalone app)**
  - Redesigned standalone downloader and update environment
  - Added icons to download progress
  - Added icons to saved posts downloader
  - **Cookies**: new ways to add cookies. You can now export cookies using the browser extension and then import them into SCrawler!
  - User creation: ability to extract the user's URL from the buffer and apply parameters if found
  - User creation: simplified way to create new users (`Ctrl+Insert` to create a new user with default parameters from clipboard URL)
  - Ability to customize the placement of ffmpeg (and other) files
  - Ability to customize the command line encoding
  - New notification options for standalone downloader
  - Reddit: now it can download saved crossposts
  - RedGifs: added `UserAgent` option
  - Other improvements
- Removed
  - User creation: remove the 'Channel' checkbox because it confuses people
  - Removed an ability to open SCrawler with `-v` argument
  - All ways to create users except URL. You can only properly create a user using the user's URL.
- Plugins
  - Added `IDownloadableMedia` interface
  - Removed `Channel` option from all functions and enums
  - ISiteSettings: added `GetSingleMediaInstance` function
  - IPluginContentProvider: added `DownloadSingleObject` function
  - IPluginContentProvider: added tokens to `GetMedia` and `Download` functions
  - IPluginContentProvider: removed `GetSpecialData` function
  - UserMediaTypes: added `Audio` and `AudioPre` enums
- Fixed
  - LPSG: attachments not downloading (Issue #114)
  - Twitter: saved posts not downloading (Issue #119)
  - XVIDEOS: saved posts not downloading
  - Deleting labels file
  - PornHub: hide unnecessary errors (Issue #116)
  - PornHub: photo galleries bug (Issue #115)
  - Minor bugs

# 2023.3.5.0

*2023-03-05*

- Fixed
  - A bug in the new way of naming `SavedPosts` data files.
  - An error that could occur during Twitter MD5 comparison.
  - A bug in the ffmpeg file parts concatenation algorithm that could occur in some cases.

# 2023.3.1.0

*2023-03-01*

- Added
  - **Path plugin.** Now you can add paths. *This may be suitable if you want to add a collection of media data to a specific user collection.*
  - MainWindow: setting a background image
  - MainWindow: setting background color and font color
  - Feed: setting background color and font color
  - Feed: (Request #108) center the image in the feed grid
  - Users: the ability to use user site name (if it exists) as a friendly name (on supported sites: Reddit, Twitter, Instagram)
  - Users: the ability to update user site name every time
  - Twitter: ability to download images using MD5 comparison to protect against duplicate downloads *(this may be suitable for the users who post the same image many times)*
  - Twitter: one-time duplicate image removal option
  - XHamster: (Request #107) added channels downloading
- Updated
  - Updated ffmpeg to version [5.1.2](https://github.com/GyanD/codexffmpeg/releases/tag/5.1.2)
- Fixed
  - PornHub: (Issue #106) unicode titles
  - (Issue #106) problem with non-Latin characters
  - ffmpeg: maximum input length error when merging parts of files

# 2023.2.5.0

*2023-02-05*

- Added
  - The ability to configure UserAgent
- Fixed
  - (Issue #101) Failed download Gfycat video in some cases

# 2023.1.27.0

*2023-01-27*

- Added
  - Advanced Twitter options for GIFs
  - Changing the icon of the user creation form based on the selected site
- Fixed
  - Pinned Instagram posts reload every time
- Plugins
  - Added
    - `Interaction` option to the `Provider` attribute
    - `IPropertyProvider` interface

# 2023.1.24.1

*2023-01-24*

- Added
  - Icon for standalone downloader
- Fixed
  - (Issue #100) some Imgur albums won't download

# 2023.1.24.0

*2023-01-24*

- Fixed
  - (Issue #100) Imgur albums not downloading
  - When deleting a collection with the 'ban' option, users in the collection are not banned

# 2023.1.2.0

*2023-01-02*

- Added
  - RedGifs: an ability to customize token refresh interval
  - RedGifs: token refresh interval changed from 24 hours to 12 hours
  - Updated labels collection
- Fixed
  - PornHub: bug in the downloader
  - PornHub: download additional non-user videos
  - Reddit: bug in standalone downloader
  - Fixed a bug in the user list loading algorithm
  - Notifications: pressing any button opens SCrawler

# 2022.12.27.0

*2022-12-27*

- Added
  - XVideos: added downloading 'Quickies'
  - Instagram: added more enable/disable options
- Fixed
  - XVideos not downloading (sorry, I broke it in a previous release)

# 2022.12.26.0

*2022-12-26*

**ATTENTION!**

**Instagram requirements changed. Headers and cookies are now required to download Timeline, Stories and Saved posts; hash to download tagged posts. Please update your credentials.**

**Instagram tagged posts no longer provide the total amount of tagged posts. I've corrected the tagged posts notification, but now I can't tell how many requests will be spent on downloading tagged posts. And from now on, one request will be spent on downloading each tagged post, because Instagram doesn't provide complete information about the tagged post with the site's response. In this case, if the number of tagged posts is 1000, 1000 requests will be spent. Be careful when downloading them. I highly recommend that you forcefully disable the downloading of tagged posts for a while.**

- Added
  - Updated user loading algorithm
  - Channels button to tray context menu
  - (Request #96) Add FFmpeg to x86 version
- Fixed
  - PornHub wrong behavior when downloading images
  - Unable open XVideos user profile
  - Cannot delete multiple collections at once
  - Can't focus user from the download info form
  - Instagram downloader not working
  - (Issue #69) **RedGifs data is not downloading**. Again.
  - Minor bugs

# 2022.11.16.0

*2022-11-16*

**ATTENTION! This version makes changes to the base SCrawler user configuration file. Since you started using this version, you still can downgrade. BUT! Once you add a virtual collection or a virtual user to a collection, you won't be able to downgrade without losing data.**

- Added
  - **PornHub**
  - **XHamster**
  - An ability to download saved XVIDEOS posts
  - Download indicator. While downloading, the rainbow tray icon changed to a blue arrow.
  - Collections: the ability to edit a collection using a form
  - Collections: the ability to create a **`virtual collection`** and add a **`virtual user`** to a real collection
  - Collections: an easier way to added users to a collection
  - Collections: an easier way to create collections
  - Added icons for channels form context menu buttons
  - More convenient change of user labels from the context menu of the user list
  - Notifications: complete transition from default notifications to ToastNotifications
  - Notifications: when you click on the notification that some of the channels are downloaded, the channels form opens
  - Notifications: when you click on the notification that all users are downloaded, the main window form opens
  - Notifications: when you click on the notification that the saved posts are downloaded, the saved posts form opens
  - Import users
  - Minor improvements
- Plugins
  - Added
    - `TaskGroup` attribute
    - `IUserMedia` interface
  - Changed
    - `GetUserUrl` and `GetUserPostUrl` functions: `String UserName` and `String UserID` changed to ` IPluginContentProvider User`
- Fixed
  - Collections editor: new added collections are still not added to the top of the collections list
  - Users search form doesn't remember last size
  - Minor bugs

# 2022.10.23.0

*2022-10-23*

- Added
  - RedGifs token Auto-Renewal
  - Download groups: ability to select sites
  - Download groups: ability to exclude labels and sites
  - AutoDownloader: ability to exclude labels and sites in ```All```, ```Default``` and ```Specified``` modes
  - The ```Download All``` button turns blue when pause is enabled
  - Updated Twitter status codes
  - Minor improvements
- Fixed
  - Updated Twitter status codes
  - AutoDownloader: incorrect next run date in scheduler task information
  - AutoDownloader: minor bugs
  - (Issue #69) **RedGifs data is not downloading**. Requires token.
  - Minor bugs

# 2022.10.18.0

*2022-10-18*

- Added
  - **TikTok** ([limited](https://github.com/AAndyProgram/SCrawler/wiki/Settings#tiktok-limits))
  - **Search form** (```Ctrl+F```)
  - Feed improvements
  - Ability to save the download session for viewing later
  - Ability to download user, excluding from the feed (use the ```Ctrl``` key with a button click of with a hot key press)
  - Ability to disable the notification about the absence of the ffmpeg.exe file
  - Extended user information with labels  
  - Advanced AutoDownloader pause options
  - Added pause buttons to tray icon and AutoDownloader form
  - Additional Instagram protection
  - Advanced notification management
  - Silent mode (temporarily disable notification)
  - Excluding users whose profiles do not exist from downloading with groups and AutoDownloader
  - Minor improvements
- Updated
  - Grouped all download buttons into one menu
  - **Finished missing posts**. You can now download missing posts if they exist.
  - PluginProvider: added ```BeginEdit``` and ```EndEdit``` methods
  - PluginProvider: ```GetSpecialData``` return type changed from ```IEnumerable(Of PluginUserMedia)``` to ```IEnumerable```
  - XVIDEOS and LPSG plugins are moved from libraries to SCrawler
- Fixed
  - (Issue #69) **RedGifs data is not downloading**. Requires cookies and token.
  - Some minor bugs when deleting a collection
  - Feed: start video playing may cause the program to freeze (strange behavior of the vlc library)
  - Feed: videos hosted on Reddit not showing up in feed
  - Feed: minor bugs
  - Collection users were not banned when deleted with the ban option
  - When trying to delete multiple collections, each collection asked for confirmation to delete
  - Minor bugs

# 2022.9.24.0

*2022-09-24*

- Added
  - Ability to copy user data to another destination
  - Ability to add 'Session' and 'Date' values to the post title in the feed
  - Minor feed improvements
  - The newly created collection will now appear at the top of the list (after reopening the form)
  - Ability to add multiple users at a time to the collection.
- Fixed
  - Autodownloader opens a compressed image instead of a full one
  - Incorrect resizing of the feed grid after deleting a media file
  - Incorrect behavior when deleting/removing a user from a collection.
  - An incorrect function that displayed the number of spent Instagram requests.
  - Bug in the XVIDEOS downloader
  - Minor bugs

# 2022.9.17.0

*2022-09-17*

- Added
  - Added two date filters to filter users (in range, not in range)
  - (Request #71) Download data for a specific date range
  - The ability to disable site downloading (in the site settings form)
- Updated
  - Plugins
- Fixed
  - (Issue #71) ```Download data to the date``` doesn't work for Twitter
  - Download data for a specific date range doesn't work for multiple users
  - Incorrect feed sorting algorithm
  - Minor bugs

# 2022.9.16.0

*2022-09-16*

- Fixed
  - Failed to get video thumbnail for channel video post
  - Incorrect rendering of the 'Feed' table when the number of columns is more than one
  - Minor design bugs

# 2022.9.13.0

*2022-09-13*

- Added
  - Video duration to the feed
- Fixed
  - (Issue #70) Instagram posts not downloading if there are pinned posts that have already been downloaded
  - Minor bugs

# 2022.9.10.0

*2022-09-10*

- Fixed
  - The memory is still leaking. This time because of the video. *Using WMP was not the best choice.*

# 2022.9.8.1

*2022-09-08*

- Fixed
  - Unexpected memory leak when using the 'Feed' form

# 2022.9.8.0

*2022-09-08*

- Added
  - **Feed** (feed of downloaded media files)
  - Missing posts tracking and management
  - Simple scheduler notifications
- Fixed
  - (Issue #67) Saved Instagram posts not downloading

# 2022.8.28.0

*2022-08-28*

- Added
  - RedGifs icon
- Fixed
  - Incorrect number of posts displayed in the Reddit channels downloader.

# 2022.8.22.0

*2022-08-22*

- Added
  - Ability to enable/disable the display of the downloaded image in toast notifications (AutoDownloader)
  - Ability to enable/disable the display of the user icon in toast notifications (AutoDownloader)
  - Downloading with standalone video downloader has been moved to a separate thread
- Fixed
  - (Issue #35) The file name does not change only by date
  - (Issue #62) Internal library error
  - AutoDownloader option ```Show notifications``` not saved
  - Minor bugs

# 2022.7.7.0

*2022-07-07*

- Added
  - **Scheduler** (creating multiple automation tasks)
  - Automation startup delay
  - Download ```webp``` in ```jpg``` format
  - Development: the ability to create a label control, that provides some information
- Removed
  - Instagram auto-fill hash from cookies
- Updated
  - Plugins
- Fixed
  - ```Stop``` option not working properly
  - In some cases, Twitter image is not downloading
  - Minor bugs

# 2022.6.10.0

*2022-06-10*

**Attention! From now on, Instagram requires Cookies, Hash and authorization headers!**

- Fixed
  - Can't get Instagram user ID

# 2022.6.6.0

*2022-06-06*

- Added
  - Ability to pause automation
- Fixed
  - GIFs from Twitter not downloading
  - Not quite correct algorithm for stopping automation

# 2022.6.3.0

*2022-06-03*

Changed version numbering method. From now on, new versions will be numbered by release date (YYYY.M.D)

**Attention! Starting with this release, SCrawler may not work on windows 7 and 8 or may not work correctly. All future releases will only be guaranteed to work on windows 10 and 11.**

- Added
  - **Automation** (downloading data automatically every ```X``` minutes)
  - Expanded settings for Instagram tagged posts that are downloaded for the first time.
- Fixed
  - Videos hosted on Reddit that are downloaded via m3u8 playlists are missing an audio track.
  - Instagram hash not able to be auto-filled from cookies

# 3.0.0.10

*2022-05-23*

- Added
  - **Downloading groups**
  - **Download saved Twitter posts** (bookmarks)
  - Ability to enable/disable progress form opening at the start of downloading
  - Ability to enable/disable Info form opening at the start of downloading
  - The ability to disable the opening of forms Info and Progress at the start of downloads if it was once closed
  - Focusing the main window when opening Info or Progress forms
  - Ability to execute a script/command when closing SCrawler
  - Ability to execute a script/command after all downloads are completed
  - Minor improvements
- Fixed
  - Instagram tagged data not downloading (now requires one more parameter **x-csrftoken** to download tagged data)
  - In some cases, Instagram Stories cannot be downloaded due to forbidden Windows characters
  - Separate Instagram posts were not downloading via the Video Downloader form.
  - In some cases, an Imgur video hosted on Reddit won't download
  - Gfycat data not downloading from saved Reddit posts
  - In some cases, the date and time are not added to the filename
  - Unable to download photos from Twitter in full resolution (4K)

# 3.0.0.9

*2022-04-24*

- Added
  - Excluded labels
  - Ability to disable user grouping
  - Ability to show groups of user sites when filtering by labels
- Fixed
  - Removed adding "No Parsed" internal label when not needed
  - Redownloading Instagram Stories

# 3.0.0.8

*2022-04-19*

- Added
  - Script mode ```command```
  - Disabled Instagram error 403 (Forbidden) logging for downloading tagged data
- Fixed
  - The script does not run after the user download is complete

# 3.0.0.7

*2022-04-14*

- Added
  - Ability to run a script after the user download is complete
  - Hotkey ```F2``` for additional options in the user creation form
- Fixed
  - (Issue #32) In some cases, Date and Time are still not added for Stories and Tagged Photos
  - (Issue #33) Instagram Stories downloading error
  - LPSG downloader does not download all content

# 3.0.0.6

*2022-04-04*

- Added
  - ```GoTo Start``` channels button
  - ```GoTo End``` channels button
- Fixed
  - In some cases, saved Reddit posts didn't fully download
  - Incorrect Reddit accessibility check algorithm
  - Incorrect behavior of the main progress bar when downloading saved posts
  - (Issue #25) Date and Time not added for Stories and Tagged Photos

# 3.0.0.5

*2022-04-02*

- Added
  - ```New```, ```Hot```, ```Top``` Reddit channel and user download modes

# 3.0.0.4

*2022-03-26*

- Fixed
  - External plugins do not save information about downloaded files
  - The user cannot be added to the collection if a special path has been specified.

# 3.0.0.3

*2022-03-24*

- Added
  - Download all by specific sites
  - Download all, ignoring the ```Ready for download``` option
  - Download all by specific sites, ignoring the ```Ready for download``` option
- Fixed
  - (Issue #19) Typo in default Instagram settings (Post limit timer)
  - Typo when applying "Download UHD" in XVIDEOS plugin
  - The sites filter does not work unless the "Fast profiles loading" option is enabled.

# 3.0.0.2

*2022-03-22*

- Added
  - **LPSG** site plugin
  - **XVIDEOS** site plugin
- Updated
  - Plugin provider
- Fixed
  - Minor bugs

# 3.0.0.1

*2022-03-20*

- Added
  - Download data up to a specific date
  - Update and Reset functions in the plugin (ISiteSettings)
  - PostsDateLimit propperty in the plugin (IPluginContentProvider)
- Fixed
  - The donation button redirects to a broken link
  - In some cases, an error occurs when fast loading images
  - In some cases, cookies are not saved
  - Some design fixes
  - Minor bugs

# 3.0.0.0

*2022-03-17*

**Attention! This version of the program makes changes user data file (Users.xml). Once you start using this version, you will not be able to use previous versions of the program. Therefore, it is highly recommended to archive the program settings folder and archive the users' data files (you can use the [```ArchiveSCrawlerUsersDataFiles.bat```](Tools/ArchiveSCrawlerUsersDataFiles.bat) tool to archive the data files of all users).**

- Added
  - **PLUGINS SUPPORT**
  - **Gfycat** site support
  - Description of Twitter and Reddit user profiles
  - Filter users by profile status "Suspended"
  - Filter users by profile status "Deleted"
  - Filter profiles that haven't downloaded new data since specific date
  - Collections that contain non-existent profiles will be marked in blue
  - Ability to find and activate a user in the main window from the Info form
  - Ability to copy user images from all channels you have when adding a user from a channel
  - Reddit default option "Get user media only" if now also used when creating new users from channels
  - Ability to update user description every time
  - ```Enter``` hotkey in the download info form to open the user's folder
  - ```Enter``` hotkey in the main window to open the user's folder
  - Channel statistics are supplemented by "existing users"
  - ```Up``` and ```Down``` navigation buttons in the Info form
  - ```Find``` button on the Info form to find the user in the main window
  - "Details" view mode
  - Fast loading of profiles in the main window. **Be careful with this setting. Fast loading leads to the highest CPU usage.**
  - Reddit availability check with DownDetector
  - Ability to [open folders with a specific program](https://github.com/AAndyProgram/SCrawler/wiki/Settings#folder-command)
  - (Request #16) Ability to remove a user from the collection without deletion
  - (Request #17) **Instagram Tagged** photos downloading
  - (Request #17) **Instagram Stories** downloading
  - Deleting data to recycle bin
- Updated
  - "List" view mode
- Fixed
  - Twitter reloads existing media
  - Reddit saved posts downloader downloads all posts every time
  - Minor bug that caused Instagram tasks timers to run longer
  - A library error that in some cases leads to a fatal program error
  - (Issue #16) Cannot delete a user that is in the collection.

At the requests of some users, I added [screenshots](ProgramScreenshots) of the program and added screenshots to [ReadMe](README.md) and the [guide](https://github.com/AAndyProgram/SCrawler/wiki).

# 2.0.0.4

*2022-02-07*

**Removed compatibility of program settings with version 1.0.0.4 and lower.**

**If your program version is 1.0.0.4 and lower, it is strongly recommended that you upgrade to release 2.0.0.1 to update the program settings (and run the program). Then update to this release. Otherwise, you will have to configure the program settings again**

**If your program version is 1.0.1.0 or higher, you should not pay attention to this message.**

- Added
  - Ability to specify the path to store saved posts
- Fixed
  - **Error when specifying network paths**
  - Minor bugs

# 2.0.0.3

*2022-02-02*

**Removed compatibility of program settings with version 1.0.0.4 and lower.**

**If your program version is 1.0.0.4 and lower, it is strongly recommended that you upgrade to release 2.0.0.1 to update the program settings (and run the program). Then update to this release. Otherwise, you will have to configure the program settings again**

**If your program version is 1.0.1.0 or higher, you should not pay attention to this message.**

- Added
  - The "Get User Media Only" setting is now available for Reddit. If checked then "CrossPosts" will be skipped, otherwise "CrossPosts" will be included.
- Fixed
  - In some cases, the program did not parse all Reddit posts.
  - Collection ignored when validated when creating a new user
  - Incorrect number of Instagram profiles downloads per session

# 2.0.0.2

*2022-01-23*

**This is the last release that supports program settings of version 1.0.0.4 and lower. Compatibility of program settings with version 1.0.0.4 and lower will be removed in future releases. It is strongly recommended that you upgrade to this release before future releases. Otherwise, you will have to configure the program settings again. If your program version is 1.0.1.0 or higher, you should not pay attention to this message.**

- Added
  - Tray icon
  - Close program to tray
  - Close confirmation dialog
  - **Separated thread for downloading Instagram profiles**
  - **Wait timers to bypass Instagram error "Too Many Requests" (429)**
  - **Downloading saved Instagram posts** *(requires a second InstaHash)*
  - Downloading saved posts (from Reddit and Instagram) form
  - Tray notification when download is complete (Instagram notification separate from other)
  - Downloading not downloaded Instagram posts when a 429 error is encountered and/or the user stops downloading
  - Separate progress bar for downloading Instagram profiles
  - Clear information about downloaded profiles of the current session in the "Download info form"
  - Increased the number of Instagram posts (from 12 to 50) received per request
  - Channels' statistics
  - **RedGifs profiles support**
- Fixed
  - The program was showing incorrect information about the total numbers of images and videos downloaded when a Reddit user was created from a channel

# 2.0.0.1

*2021-12-29*

- Added
  - Download individual Imgur media files (use the "Download video" form).
- Fixed
  - Incorrect filling of user parameters in the user creation form
  - In some cases, the global settings cannot be saved.

# 2.0.0.0

*2021-12-27*

- Added
  - **Instagram**
  - Filter by site
  - Group for regular channels in the main window
  - Ability to change user/collection path
  - Imgur albums downloading
  - NSFW Imgur content bypass (requires 'ClientID')
  - Special user folder
  - Remove user while keeping data
  - Disabled overriding user preferences when creating a new user if it already exists in the destination (in case of deleting a user with saving data).
  - **Saved Reddit posts downloading**
- Fixed
  - Suspended profiles do not change status if the profile is no longer suspended
  - Limited download for Twitter not implemented

# 1.0.1.0

*2021-12-20*

- Added
  - Extended site settings
  - Non-existend users will be marked in red
  - Suspended users' profiles will be marked in yellow
  - Automatically disable 'Ready for download' if user does not exist.
  - Ability to disable MD5 check when downloading regular (added to the main window) channels
  - Ability to create a user from a channel with the default option 'Ready to download' (setting in the 'Settings')
  - Ability to change default 'Temporary' parameter on create a user from a channel (setting in the 'Settings')
  - Advanced defaults for each site (download images, download videos and temporary)
  - By checking the 'Temporary' checkbox in the user creation form, the 'Ready for download' checkbox became unchecked
  - Automatically disable 'Ready for download' if profile does not exists or has been deleted
- Change
  - Removed extended twitter invalid credentials error and replaced with a simple line in the log
  - Redesigned settings form
- Fixed
  - In some cases, the image of the channel post is not copied to the user's folder
  - Users in the main window are not refreshed if new users are added by a list that includes banned and/or unrecognized users.
  - Minor bugs

# 1.0.0.4

*2021-12-12*

- Added
  - Full channels support (you can now add channel (subreddit) for standard download)
  - ```Ready for download``` now available for collections and can be changed for multiple user
- Fixed
  - Images hosted on Imgur won't download

# 1.0.0.3

*2021-12-11*

- Fixed
  - Custom "Download videos" option is not saved
  - The "Download all" button is not activated after changing modes

# 1.0.0.2

*2021-12-10*

- Added
  - Ability to choose what types of media you want to download (images only, videos only, both)
  - Ability to name files by date
- Fixed
  - In some cases, the "Stop" button is not activated after download start

# 1.0.0.1

*2021-12-09*

- Added
  - Limited download if user added from the channel
  - Forced limited download for any user
  - x86 compatibility
  - Coping user image posted in the channel (if user added from the channel)
  - Check for new version at start setting
  - Removing currently downloading user
  - Change maximum count of along downloading tasks of users
  - Change maximum count of along downloading tasks of channels
- Removed
  - Reparse not downloaded content (left from the older versions)
- Fixed
  - ```No Label``` and ```No Parsed``` labels does not shown in the labels list
  - User list does not refresh by labels change in the main window
  - Disabled collection editing
  - Collection name does not show in some operations
  - Error (in some cases) on add to collection
  - Wrong some Reddit videos parsing
  - Wrong some Reddit images parsing

# 1.0.0.0

*2021-12-07*

Initial release
# 2023.6.8.0

*2023-06-08*

- Added
  - YouTube: append artist name to music playlist output path
  - YouTube: save thumbnail path for playlist and channel
- Fixed
  - YouTube: opening paths to downloaded playlists and channels
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
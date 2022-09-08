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
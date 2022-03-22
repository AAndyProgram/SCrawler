# 3.0.0.2

- Added
  - **LPSG** site plugin
  - **XVIDEOS** site plugin
- Updated
  - Plugin provider
- Fixed
  - Minor bugs

# 3.0.0.1

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

**Removed compatibility of program settings with version 1.0.0.4 and lower.**

**If your program version is 1.0.0.4 and lower, it is strongly recommended that you upgrade to release 2.0.0.1 to update the program settings (and run the program). Then update to this release. Otherwise, you will have to configure the program settings again**

**If your program version is 1.0.1.0 or higher, you should not pay attention to this message.**

- Added
  - Ability to specify the path to store saved posts
- Fixed
  - **Error when specifying network paths**
  - Minor bugs

# 2.0.0.3

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

- Added
  - Download individual Imgur media files (use the "Download video" form).
- Fixed
  - Incorrect filling of user parameters in the user creation form
  - In some cases, the global settings cannot be saved.

# 2.0.0.0

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

- Added
  - Full channels support (you can now add channel (subreddit) for standard download)
  - ```Ready for download``` now available for collections and can be changed for multiple user
- Fixed
  - Images hosted on Imgur won't download

# 1.0.0.3

- Fixed
  - Custom "Download videos" option is not saved
  - The "Download all" button is not activated after changing modes

# 1.0.0.2

- Added
  - Ability to choose what types of media you want to download (images only, videos only, both)
  - Ability to name files by date
- Fixed
  - In some cases, the "Stop" button is not activated after download start

# 1.0.0.1

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

Initial release
# :rainbow_flag: Happy LGBT Pride Month :tada:

# Social networks crawler

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/AAndyProgram/SCrawler)](https://github.com/AAndyProgram/SCrawler/releases/latest)
[![GitHub license](https://img.shields.io/github/license/AAndyProgram/SCrawler)](https://github.com/AAndyProgram/SCrawler/blob/main/LICENSE)
[![FAQ](https://img.shields.io/badge/FAQ-green)](FAQ.md)
[![GUIDE](https://img.shields.io/badge/GUIDE-green)](https://github.com/AAndyProgram/SCrawler/wiki)
[![How to support](https://img.shields.io/badge/HowToSupport-green)](HowToSupport.md)

A program to download photo and video from [any site](#supported-sites) (e.g. Reddit, Twitter, Instagram).

Do you like this program? Consider adding to my coffee fund by making a donation to show your support. :blush:

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/andyprogram)

**Bitcoin**: bitcoin:BC1Q0NH839FT5TA44DD7L7RLR97XDQAG9V8D6N7XET

![Main window](ProgramScreenshots/MainWindow.png)
![Channels window](ProgramScreenshots/Channels.png)

# What can program do:
- Download pictures and videos from users' profiles and subreddits:
  - Reddit images;
  - Reddit galleries of images;
  - Reddit videos (downloading Reddit hosted video is going through ffmpeg (**ffmpeg only works with the x64 program**));
  - Redgifs videos (https://www.redgifs.com/);
  - Twitter images and videos;
  - Instagram images and videos;
  - Instagram tagged posts;
  - Instagram stories;
  - Imgur images, galleries and videos;
  - Gfycat videos;
  - [Other](#supported-sites) supported sites
- Parse [channel and view data](https://github.com/AAndyProgram/SCrawler/wiki/Channels)
- Download [saved Reddit, Twitter and Instagram posts](https://github.com/AAndyProgram/SCrawler/wiki/Home#saved-posts)
- Add users from parsed channel
- **Advanced user management**
- **Automation** (downloading data automatically every ```X``` minutes)
- Labeling users
- Create download groups
- Adding users to favorites and temporary
- Filter exists users by label or group
- Selection of media types you want to download (images only, videos only, both)
- Download a special video, image or gallery
- Making collections (grouping users into collections)
- Specifying a user folder (for downloading data to another location)
- Changing user icons
- Changing view modes
- ...and many others...

# Supported sites

- **Reddit**
- **Twitter**
- **Instagram**
- RedGifs
- Imgur
- Gfycat
- LPSG
- XVIDEOS
- [Other sites](Plugins.md)

# How does it works:

First, the program downloads the full profile. After the program downloads only new posts. The program remembers downloaded posts.

## Reddit

The program parses all user posts, obtain MD5  images hash and compares them with existing ones to remove duplicates. Then the media will be downloaded.

## Other sites

The program parses all user posts and compares file names with existing ones to remove duplicates. Then the media will be downloaded.

You can read about Instagram restrictions [here](https://github.com/AAndyProgram/SCrawler/wiki/Settings#instagram-limits)

## How to request a new site

Read [here](CONTRIBUTING.md#how-to-request-a-new-site) about

# Requirements

- Windows 10, 11 with NET Framework 4.6.1 or higher (v4.6.1 must be installed). You can check version compatibility with this [tool](Tools/NET.FrameworkVersion.ps1).
- Authorization [cookies](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-set-up-cookies) and [tokens](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-twitter-tokens) for Twitter (if you want to download data from Twitter)
- Authorization [cookies](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-set-up-cookies) and [Hash](https://github.com/AAndyProgram/SCrawler/wiki/Settings#instagram) for Instagram (if you want to download data from Instagram), [Hash 2](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-instagram-hash-2) for saved Instagram posts, Instagram [stories authorization headers](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-instagram-stories-authorization-headers) for Stories and Tagged data
- ffmpeg library for downloading videos hosted on Reddit (you can download it from the [official repo](https://github.com/GyanD/codexffmpeg/releases/tag/2021-01-12-git-ca21cb1e36) or [from my first release](https://github.com/AAndyProgram/SCrawler/releases/download/1.0.0.0/ffmpeg.zip)). **ffmpeg only works with the x64 version of the program.**

# Guide

**Full guide you can find [here](https://github.com/AAndyProgram/SCrawler/wiki)**

# Installation

**Just download the [latest release](https://github.com/AAndyProgram/SCrawler/releases/latest), unzip the program archive to any folder, copy the file ```ffmpeg.exe``` into it and enjoy.** :blush:

**Don't put program in the ```Program Files``` system folder (this is portable program and program settings are stored in the program folder)**

# Updating

Just download [latest](https://github.com/AAndyProgram/SCrawler/releases/latest) version and unpack it into the program folder. **Before starting a new version, I recommend making a backup copy of the program settings folder.**

# How to build from source

1. Delete the "PersonalUtilities" project from the solution.
1. Delete the "PersonalUtilities.Notifications" project from the solution.
1. Add the latest versions of the ```PersonalUtilities.dll``` and ```PersonalUtilities.Notifications.dll``` libraries (from the [latest release](https://github.com/AAndyProgram/SCrawler/releases/latest)).
1. Import PersonalUtilities.Functions for the whole project.

# How to make a plugin

Read about how to make plugin [here](https://github.com/AAndyProgram/SCrawler/wiki/Plugins).

# How to support

Read more about how to support the program [here](HowToSupport.md).

# Settings and usage

The program has an intuitive interface.

You need to set up authorization for Twitter and Instagram:
- Authorization [cookies](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-set-up-cookies) and [tokens](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-twitter-tokens) for **Twitter** (if you want to download data from Twitter)
- Authorization [cookies](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-set-up-cookies) and [Hash](https://github.com/AAndyProgram/SCrawler/wiki/Settings#instagram) for **Instagram** (if you want to download data from Instagram), [Hash 2](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-instagram-hash-2) for **saved Instagram posts**, Instagram [stories authorization headers](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-find-instagram-stories-authorization-headers) for **Stories** and **Tagged data**

Just add a user profile and **click the ```Start downloading``` button**.

You can add users by patterns:
- https://www.instagram.com/SomeUserName
- https://twitter.com/SomeUserName
- https://reddit.com/user/SomeUserName
- https://reddit.com/r/SomeSubredditName
- https://www.redgifs.com/users/SomeUserName
- u/SomeUserName
- r/SomeSubredditName
- SomeUserName (in this case, you need to select the user's site)
- SomeSubredditName

Read more about adding users and subreddits [here](https://github.com/AAndyProgram/SCrawler/wiki/Users)

![Add user](ProgramScreenshots/CreateUserClear.png)

# Using program as just video downloader

Create a shortcut for the program. Open shortcut properties. In the ```Shortcut``` tab, in the ```Target``` field, just add the letter ```v``` at the end across the space.

Example: ```D:\Programs\SCrawler\SCrawler.exe v```

![Separate video downloader](ProgramScreenshots/SeparateVideoDownloader.png)

# Contact me

[![matrix](https://img.shields.io/badge/Matrix-%40andyprogram%3Amatrix.org-informational)](https://matrix.to/#/@andyprogram:matrix.org)

# Social networks crawler

A program to download photo and video from Reddit, Twitter, Instagram, [etc](#supported-sites).

Do you like this program? Consider adding to my coffee fund by making a donation to show your support. :)

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/andyprogram)

# What can program do:
- Download pictures and videos from users' profiles and subreddits:
  - Reddit images;
  - Reddit galleries of images;
  - Redgifs hosted videos (https://www.redgifs.com/);
  - Reddit hosted videos (downloading Reddit hosted video is going through ffmpeg (**ffmpeg only works with the x64 program**));
  - Twitter images and videos;
  - Instagram images and videos.
- Parse [channel and view data](https://github.com/AAndyProgram/SCrawler/wiki/Channels).
- Download [saved Reddit and Instagram posts](https://github.com/AAndyProgram/SCrawler/wiki/Home#saved-posts).
- Add users from parsed channel.
- Labeling users.
- Filter exists users by label or group.
- Selection of media types you want to download (images only, videos only, both)

# Supported sites

- Reddit
- Twitter
- Instagram
- RedGifs
- Imgur

# How does it works:

## Reddit

The program parses all user posts, obtain MD5  images hash and compares them with existing ones to remove duplicates. Then the media will be downloaded.

## Other sites

The program parses all user posts and compares file names with existing ones to remove duplicates. Then the media will be downloaded.

You can read about Instagram restrictions [here](https://github.com/AAndyProgram/SCrawler/wiki/Settings#instagram-limits)

## How to request a new site

Read [here](https://github.com/AAndyProgram/SCrawler/blob/main/CONTRIBUTING.md#how-to-request-a-new-site) about

# Requirements:

- Windows 7, 8, 9, 10, 11 with NET Framework 4.6.1 or higher
- Authorization cookies and tokens for Twitter (if you want to download data from Twitter)
- Authorization cookies Instagram (if you want to download data from Instagram)
- ffmpeg library for downloading videos hosted on Reddit (you can download it from the [official repo](https://github.com/GyanD/codexffmpeg/releases/tag/2021-01-12-git-ca21cb1e36) or [from my first release](https://github.com/AAndyProgram/SCrawler/releases/download/1.0.0.0/ffmpeg.zip)). **ffmpeg only works with the x64 version of the program.**
- **Don't put program in the ```Program Files``` system folder (this is portable program and program settings are stored in the program folder)**
- **Just unzip the program archive to any folder, copy the file ```ffmpeg.exe``` into it and enjoy. :)**

# How to build from source

1. Delete the "PersonalUtilities" project from the solution.
2. Add the latest version of the "PersonalUtilities.dll" library (from the [latest release](https://github.com/AAndyProgram/SCrawler/releases/latest)).
3. Import PersonalUtilities.Functions for the whole project.

# Updating

Just download [latest](https://github.com/AAndyProgram/SCrawler/releases/latest) version and unpack it into the program folder. Before starting a new version, I recommend making a backup copy of the program settings folder.



# Settings and usage

The program has an intuitive interface.

Just add a user profile and click the ```Start downloading``` button.

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

**Full guide you can find [here](https://github.com/AAndyProgram/SCrawler/wiki)**

## Using program as just video downloader

Create a shortcut for the program. Open shortcut properties. In the ```Shortcut``` tab, in the ```Target``` field, just add the letter ```v``` at the end across the space.

Example: ```D:\Programs\SCrawler\SCrawler.exe v```

# Contact me

[Element messenger](https://element.io/): @andyprogram:matrix.org
https://matrix.to/#/@andyprogram:matrix.org

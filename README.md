# Social networks crawler

Program for downloading photo and video from Reddit and Twitter

Enjoying the tool? Considering adding to my coffee fund :)

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/andyprogram)

# What can program do:
- Download pictures and videos from users' profiles:
  - Reddit images;
  - Reddit galleries of images;
  - Redgifs hosted videos (https://www.redgifs.com/);
  - Reddit hosted videos (downloading Reddit hosted video is going through ffmpeg);
  - Twitter images;
  - Twitter videos.
- Parse channel and view data.
- Add users from parsed channel.
- Labeling users.
- Filter exists users by label or group.

# How does it works:

## Reddit

The program parsing all user's posts, gathering pictures' MD5 hash and compare with existing for remove duplicates. Then media will be downloaded.

## Twitter

The program parsing all user's posts and compare file names with existing for remove duplicates. Then media will be downloaded.

# Requirements:

- Windows 7, 8, 9, 10, 11 with NET Framework 4.6.1 or higher
- Authorization cookies and tokens for Twitter (if you want to download data from Twitter)
- Don't put program in the ```Program Files``` system folder (this is portable program and program settings are stored in the program folder)
- Just unpack program archive in any folder you want, copy ```ffmpeg.exe``` into and enjoy. :-)

# Settings and usage

The program has an intuitive interface.

Just add user profile and press ```Start downloading``` button.

Users can be added by patterns:
- https://twitter.com/SomeUserName
- https://reddit.com/user/SomeUserName
- u/SomeUserName
- SomeUserName (in this case you must to choose user site)

More about users adding here

## Using program as just video downloader

Create a shortcut for the program. Open shortcut properties. On the ```Shortcut``` tab in ```Target``` field just add ```v``` at the end through the space.

Example: ```D:\Programs\SCrawler\SCrawler.exe v```

**Join our Discord server**: https://discord.gg/uFNUXvFFmg
<br/>*You can get help faster there!*

# Docs
- Basic info: https://github.com/AAndyProgram/SCrawler/blob/main/README.md
- **GUIDE**: https://github.com/AAndyProgram/SCrawler/wiki/
- Settings: https://github.com/AAndyProgram/SCrawler/wiki/Settings
- Discord: https://discord.gg/uFNUXvFFmg

Most of your questions are already answered. All settings, functions, buttons and everything else described in the guide.

# Backup
I strongly recommend you to **regularly** create backup copies of the settings files. **An [example script](https://github.com/AAndyProgram/SCrawler/blob/main/Tools/ArchiveSCrawlerUsersDataFiles.bat) for this** on GitHub (you **should adapt** it to your environment, and you can use it when [SCrawler is closed](https://github.com/AAndyProgram/SCrawler/wiki/Settings#behavior)).

**This way you'll always have the latest backup of your settings files and can restore it if something goes wrong!**

# How to report a problem
1. **Post your problem [here](https://github.com/AAndyProgram/SCrawler/issues) or in the [help channel](https://discord.com/channels/1124032649682493462/1124281838056259614) on our Discord server**
2. Attach the **profile URLs or links** that you cannot download.
3. Attach the **LOG** if it exists.
4. Attach **the environment information** copied from SCrawler (click the top right info button in the main window, then the `Environment` button, then the `Copy` button, and paste the copied text into the message).
5. *Add screenshots to illustrate the problem (**optional**)*

**ATTENTION! Issues without URLs will be closed without a response!**

# Most frequently questions about SCrawler

**If something doesn't download, always check the [SITE'S REQUIREMENTS](https://github.com/AAndyProgram/SCrawler/wiki/Settings#sites-requirements) before asking questions!**

*How to use: find your problem in the list and read the answer.*

## General questions
- **PROFILES**
    - I added a profile but **nothing downloaded** :arrow_forward: check your cookies and [site requirements](https://github.com/AAndyProgram/SCrawler/wiki/Settings#sites-requirements). If there are any optional fields that you don't fill in, do so. Still nothing works - [report it](#how-to-report-a-problem)!
    - User downloading failed :arrow_forward: check your credentials and **[SITES REQUIREMENTS](https://github.com/AAndyProgram/SCrawler/wiki/Settings#sites-requirements)**. If all settings are set and nothing works, [report it](#how-to-report-a-problem). **Don't forget to attach the LOG.**
    - [How to redownload user](https://github.com/AAndyProgram/SCrawler/wiki#redownload-user)
    - How to **add profile** to download :arrow_forward: copy the **[profile URL](https://github.com/AAndyProgram/SCrawler/wiki#add-user)** and press `Insert` or `Ctrl+Insert`. **ALWAYS PASTE THE USER PROFILE URL**. After that select this user and press `F5` or click the `Download selected` button.
- How to download **[saved posts](https://github.com/AAndyProgram/SCrawler/wiki#saved-posts)**
- **[HOW TO ADD COOKIES](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-set-up-cookies)**
- [How to report a problem](#how-to-report-a-problem)
- I want you to **add the site** to SCrawler :arrow_forward: **I'm not currently accepting requests to add new sites**, but you can [create a plugin](https://github.com/AAndyProgram/SCrawler/wiki/Plugins) (for your site) for SCrawler.
- What language is SCrawler written in :arrow_forward: `vb.net`
- I don't know `vb.net` and I can't write a plugin :arrow_forward: you can write a plugin in `C#`
- I have a suggestion, will it be added :arrow_forward: maybe if it interested me.
- How to name files using a pattern (e.g. `Site_PostID_Name.jpg`) :arrow_forward: **there is no such functionality and there are no such plans**.
- **DON'T CHANGE THE DEFAULT SITE SETTINGS UNLESS YOU KNOW EXACTLY WHAT YOU'RE DOING!** SCrawler already has all the default settings to work. You only need to add credentials (where [required](https://github.com/AAndyProgram/SCrawler/wiki/Settings#sites-requirements)).
- My computer shut down while SCrawler was running and now **SCrawler won't start or some users are missing** :arrow_forward: restore user settings from [backup](#backup).
- Installation, update and configuration
    - How to install: https://github.com/AAndyProgram/SCrawler#installation
    - How to update: https://github.com/AAndyProgram/SCrawler#updating
    - What file executes the program: **`SCrawler.exe`**
    - Where to find binaries: https://github.com/AAndyProgram/SCrawler/releases/latest
	- [How to build from source](https://github.com/AAndyProgram/SCrawler/blob/main/CONTRIBUTING.md#how-to-build-from-source)
    - [Video how to configure](#video-how-to-configure)
- **Antivirus**
    - **Antivirus detects SCrawler as a virus** :arrow_forward: SCrawler doesn't contain any viruses at all. All code is posted on GitHub. You can review it. I have nothing to hide. SCrawler just downloads pictures and videos. That's all. If you trust SCrawler, you should just add it to the antivirus exceptions, as I did. Sometimes antiviruses identify SCawler as a virus. This is usually related to the number of files being edited (users' settings files) and the number of files being downloaded. In this case, the antivirus can also remove these files, which will damage users' settings. **If you don't trust SCrawler, just delete it.**
    - **Antivirus detects gallery-dl as a virus** :arrow_forward: it's a trustworthy program that is trusted by thousands of people around the world. Antiviruses identify some builds as containing viruses, but this is not true. **If you don't trust gallery-dl, you can simply delete it. But if you delete it, you won't be able to download [Twitter & Pinterest](https://github.com/AAndyProgram/SCrawler/wiki/Settings#gallery-dl).** You should decide for yourself.

## Sites questions

*How to use: find the site you need in the list and read the answer.*

- Reddit: don't use credentials at all or configure [OAuth](https://github.com/AAndyProgram/SCrawler/wiki/Settings#how-to-get-reddit-credentials). **Reddit profiles can be downloaded without any credentials at all. Subreddits require OAuth! If nothing downloads, use OAuth!** Don't use OAuth token to download saved posts (use cookies only).
- **META** (**Instagram**, Threads, Facebook): you need **cookies** and fill in **all fields**
- **Instagram [TIPS](https://github.com/AAndyProgram/SCrawler/wiki/Settings#instagram-tips)**
- **Instagram saved posts**: I don't consider questions like "I have 10k saved posts and only 1000 were downloaded". Download posts, remove them from saved posts, delete the `Saved posts` **settings folder**, repeat.
- TikTok: works via yt-dlp. If something doesn't download, we need to wait until yt-dlp fixes it. TikTok doesn't require cookies to download.
- Porn sites: **COOKIES**!
- ThisVid: https://github.com/AAndyProgram/SCrawler/wiki/Settings#thisvid-faq
- **OnlyFans**: cookies + **all fields** + [OF-Scraper (download the correct version that I pointed)](https://github.com/AAndyProgram/SCrawler/wiki/Settings#of-scraper) & [mp4decrypt](https://www.bento4.com/downloads/) & **DRM keys** to download DRM protected videos. [OF-Scraper support](https://github.com/AAndyProgram/SCrawler/wiki/Settings#of-scraper-support). Also read [this](https://github.com/AAndyProgram/SCrawler/wiki/Settings#onlyfans-faq)
- **JustForFans**: **THE VIDEO ISN'T DOWNLOADING AT THE MOMENT** ([Issue](https://discord.com/channels/1124032649682493462/1205547615199039551/1231349555132366870))

## Other questions

### Does the program remember the last download and check for new posts, downloading only new posts, or does the program download the entire profile every time
The program stored posts IDs in users' folders. For the first time, the program downloads the entire profile. All subsequent times the program will check for new posts and download **only new posts**!

### Does this program have a GUI or CLI, and will a CLI be added in the future
This is a GUI program and **NO**, <u>CLI will not be added</u>

### How to remove the label
There is no functionality to remove an individual label. You can open the `Labels.txt` file in the program settings folder and delete any label you want. You also can delete this file (`Labels.txt`). In this case, when SCrawler is launched, the list of labels will be populated only with existing labels (from the user data files).

### How to remove a user from the blacklist
Just add that user back to the program. In the dialog box that opens, click the `Add and remove from blacklist` button.

### You lost me. Your program is too complicated.
**I'm fine with that**. If the program is too complicated for you or you can't configure it, I can only suggest you find another (easier) program. I really don't mind! The program is free. I develop SCrawler for myself and publish it on GitHub because people found my program useful. If someone can't use it or doesn't like it, I'm okay with it.

### Add a step-by-step guide or video on how to use the program
**NO!** The guide fully covers all the functionality of SCrawler! If you don't respect my work, I don't waste my time. If you want, you can create a video tutorial and send it to me. Then I'll add it. All options and their purposes are described on the wiki. The wiki also contains a description of all the settings and how to configure them. For complex settings there is a step-by-step guide. Read the [main](README.md) information and [GUIDE](https://github.com/AAndyProgram/SCrawler/wiki/) and you won't have any problems. I've developed a program with an intuitive interface. There is a `Settings` button, download buttons, a context menu that appears when you right-click on a user, and other controls. Anyone can use it.

**There is already a [video](#video-how-to-configure) example of how to configure a site.**

# Video how to configure

**The following video was recorded by a user who loves SCrawler and demonstrates how to add credentials using Instagram as an example:**

[![How to configure](https://img.youtube.com/vi/XDn7zG4I700/0.jpg)](https://www.youtube.com/watch?v=XDn7zG4I700)
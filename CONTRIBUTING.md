# Contributor's Guide

I welcome requests! Follow these steps to contribute:

1. Find an [issue](https://github.com/AAndyProgram/SCrawler/issues) that needs assistance.
1. Let me know you are working on it by posting a comment on the issue.
1. If you find an error in the code, please provide a link to the file and the line number.
1. If you have a code change suggestion, you can post a replacement code block. I also accept pull requests.

# How to build from source

1. Delete the "PersonalUtilities" project from the solution.
1. Delete the "PersonalUtilities.Notifications" project from the solution.
1. Add the latest versions of the ```PersonalUtilities.dll``` and ```PersonalUtilities.Notifications.dll``` libraries (from the [latest release](https://github.com/AAndyProgram/SCrawler/releases/latest)).
1. Import PersonalUtilities.Functions for the whole project.

**Always use the correct "PersonalUtilities.dll" library. You must download this library from the release of the code you downloaded.**

# How to request a new site

1. Check [issues](https://github.com/AAndyProgram/SCrawler/issues) (open and [closed](https://github.com/AAndyProgram/SCrawler/issues?q=is%3Aissue+is%3Aclosed)) and [discussions](https://github.com/AAndyProgram/SCrawler/discussions) to find your issue. Perhaps I have already answered your request.
1. If you don't find anything, create a new issue with your request. I usually reply as soon as possible (within the next few hours).

# Requirements for new site requests

**Attention! I'll add a new site only if I'm interested. I also have a life, and any development takes time.**

- Post a link to the site's API
- Post request URLs **without OAuth** authentication
- Post a **complete cURL** request which provides the required information (JSON is better)

**I don't use OAuth authentication** in my application, so if it's not too hard to make a new parsing algorithm **without OAuth** authorization, I can start developing it in the coming days. Otherwise, I need time to figure out how to do it.

If I'm interested in a site you want to add, it may be added in future releases.

# Sites I will never develop

- Facebook

# Sites requested by users

- TikTok
  - API for receiving data without authorization was not found. Therefore, I don't have time to start developing this site parsing algorithm. If anyone knows of requests that may collect data without OAuth authentication, please let me know.
  
# Contact me

[![matrix](https://img.shields.io/badge/Matrix-%40andyprogram%3Amatrix.org-informational)](https://matrix.to/#/@andyprogram:matrix.org)

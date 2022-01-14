# Contributor's Guide

I welcome pull requests! Follow these steps to contribute:

1. Find an [issue](https://github.com/AAndyProgram/SCrawler/issues) that needs assistance.
2. Let me know you are working on it by posting a comment on the issue.
3. If you find an error in the code, please provide a link to the file and the line number.
4. If you have a suggestion to change the code, you can post a block of code to replace. I don't currently have time to learn pull requests, so it might work this way.

# How to build from source

1. Delete the "PersonalUtilities" project from the solution.
2. Add the latest version of the "PersonalUtilities.dll" library (from the [latest release](https://github.com/AAndyProgram/SCrawler/releases/latest)).
3. Import PersonalUtilities.Functions for the whole project.

**Always use the correct "PersonalUtilities.dll" library. You must download this library from the release of the code you downloaded.**

# How to request a new site

1. Check [issues](https://github.com/AAndyProgram/SCrawler/issues) (open and [closed](https://github.com/AAndyProgram/SCrawler/issues?q=is%3Aissue+is%3Aclosed)) and [discussions](https://github.com/AAndyProgram/SCrawler/discussions) to find your issue. Perhaps I have already answered your request.
2. If you don't find anything, create a new issue with your request. I usually reply as soon as possible (within the next few hours).
    - If I'm interested in a site you want to add, it may be added in future releases.
      - If the site has an API that does not require authorization, it may be added in the coming releases.
      - You can make it faster by posting a link to the API. I don't use OAuth authentication in my application, so if it's not too hard to make a new parsing algorithm without OAuth authorization, I can start developing it in the coming days. Otherwise, I need time to figure out how to do it.
      - If the site does not have an API that does not require authorization, this may take some time.
    - If I'm **not** interested in the site you want to add, you can pay to have it added by making a donation of approximately $10. **But before that, you still need to create an issue. If I'm not interested, you can offer me a deal to develop it for money. I'll check the site you want to add, check the availability of the API and tell you how much time I need to develop it and the price. If you agree, I will do it.** [![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/andyprogram)
    
    
# Sites I will never develop

- Facebook

# Contributor's Guide

Follow these steps to contribute:
1. Find an [issue](https://github.com/AAndyProgram/SCrawler/issues) that needs assistance.
1. Let me know you're working on this by posting a comment on this issue.
1. If you find a bug in the code, please provide a link to the file and line number.
1. If you have a code change suggestion, you can post a replacement code block.<!-- I also accept pull requests.-->

# How to report a problem

**[Read here](https://github.com/AAndyProgram/SCrawler/blob/main/FAQ.md#how-to-report-a-problem)**

# How to build from source
1. Delete the `PersonalUtilities` project from the solution.
1. Delete the `PersonalUtilities.Notifications` project from the solution.
1. The following libraries must be added to project references with the '**Copy to output folder**' option:
    - `PersonalUtilities.dll`
    - `PersonalUtilities.Notifications.dll`
    - `Microsoft.Toolkit.Uwp.Notifications.dll`
    - `System.ValueTuple.dll`
1. Import `PersonalUtilities.Functions` for the whole project.

**Always use the correct libraries. You must download libraries from the same release date as the code commit date.**

# How to request a new site

**I'm currently not accepting requests to develop new sites.**

1. Check [issues](https://github.com/AAndyProgram/SCrawler/issues) (open and [closed](https://github.com/AAndyProgram/SCrawler/issues?q=is%3Aissue+is%3Aclosed)) and [discussions](https://github.com/AAndyProgram/SCrawler/discussions) to find your issue. Perhaps I have already answered your request.
1. If you don't find anything, create a new issue with your request.

# Requirements for new site requests

**Attention! I'll add a new site only if I'm interested. I also have a life, and any development takes time.**

- Post a link to the site's API
- Post request URLs **without OAuth** authentication
- Post a **complete cURL** request which provides the required information (JSON is better)

**I don't use OAuth authentication** in my application, so if it's not too hard to make a new parsing algorithm **without OAuth** authorization, I can start developing it in the coming days. Otherwise, I need time to figure out how to do it.

If I'm interested in a site you want to add, it may be added in future releases.

# Sites I will never develop
- Tumblr
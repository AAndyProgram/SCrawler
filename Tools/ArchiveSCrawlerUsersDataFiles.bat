REM This script is written for WinRAR. It only archives user data files. Not user content!
REM Replace 'd:\Downloads\SocialNetworks\SCrawlerBackup.rar' with the archive destination.
REM Replace 'd:\Downloads\SocialNetworks\' with the path to your SCrawler data folder.
REM THIS SCRIPT IS NOT SUITABLE FOR 7ZIP OR OTHER ARCHIVING PROGRAMS.
REM But I believe 7Zip also has CLI commands

REM This line archives SCrawler settings files.
"C:\Program Files\WinRAR\WinRAR.exe" a -r -ep1 -o+ -ag_YYYYMMDD_HHMMSS -m5 -tl "D:\MyPrograms\SCrawler\Backup\Settings.rar" "D:\MyPrograms\SCrawler\Settings\"

REM This line archives SCrawler users' settings files.
"C:\Program Files\WinRAR\WinRAR.exe" a -r -ep1 -o+ -ag_YYYYMMDD_HHMMSS -m5 -tl -n*.txt -n*.xml "D:\MyPrograms\SCrawler\Backup\SCrawlerBackup.rar" "D:\MyPrograms\SCrawler\Data\"